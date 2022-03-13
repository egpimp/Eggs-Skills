using EggsSkills.Orbs;
using EntityStates;
using EntityStates.Huntress.HuntressWeapon;
using RoR2;
using RoR2.Orbs;
using UnityEngine;

namespace EggsSkills.EntityStates
{
    class ClusterBombArrow : BaseState
    {
        //Animator
        private Animator animator;

        //Helps find assets
        private readonly FireSeekingArrow assetRef = new FireSeekingArrow();

        //Base cast time
        private readonly float baseDuration = 1f;
        //Post-Attack speed cast time
        private float duration;

        //Orb for the firing
        private GenericDamageOrb genericDamageOrb;

        //Hurtbox of the target
        private HurtBox target;

        //Muzzle to fire from
        private Transform muzzle;

        public override void OnEnter()
        {
            base.OnEnter();
            //Grab the tracking component
            HuntressTracker huntressTracker = base.GetComponent<HuntressTracker>();
            //Setup the orb to fire
            this.genericDamageOrb = new HuntressBombArrowOrb();
            //Find the target
            this.target = huntressTracker.GetTrackingTarget();
            //Determine the cast-time
            this.duration = baseDuration / attackSpeedStat;
            //Get the transform of the model
            Transform modelTransform = GetModelTransform();
            //If the model transform exists
            if(modelTransform)
            {
                //Grab the child locator
                ChildLocator childLocator = modelTransform.GetComponent<ChildLocator>();
                //Find the muzzle with the given string
                this.muzzle = childLocator.FindChild(this.assetRef.muzzleString);
                //Nab the animator
                this.animator = modelTransform.GetComponent<Animator>();
            }
            //Play the sound based on attack speed
            Util.PlayAttackSpeedSound(this.assetRef.attackSoundString, base.gameObject, base.attackSpeedStat);
            //Make sure body exists and do the thing where it looks towards where you aim
            if(base.characterBody) base.characterBody.SetAimTimer(duration + 1f);
            //Play the animations
            base.PlayCrossfade("Gesture, Override", "FireSeekingShot", "FireSeekingShot.playbackRate", this.duration, this.duration * 0.2f / this.attackSpeedStat);
            base.PlayCrossfade("Gesture, Additive", "FireSeekingShot", "FireSeekingShot.playbackRate", this.duration, this.duration * 0.2f / this.attackSpeedStat);
        }
        private void Fire()
        {
            //Set the damage to the player damage
            this.genericDamageOrb.damageValue = base.characterBody.damage;
            this.genericDamageOrb.isCrit = base.RollCrit();
            //Set the team of the orb to the player team
            this.genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
            //Set the attacker to the player
            this.genericDamageOrb.attacker = base.gameObject;
            //If the target still exists by the time we fire
            if (this.target)
            {
                //Apply the muzzle flash effect
                EffectManager.SimpleMuzzleFlash(this.assetRef.muzzleflashEffectPrefab, base.gameObject, this.assetRef.muzzleString, false);
                //Set the origin to the muzzle position
                this.genericDamageOrb.origin = this.muzzle.position;
                //Set the target of the orb to the target we found
                this.genericDamageOrb.target = this.target;
                //Add the orb (Fire it)
                if (base.isAuthority) OrbManager.instance.AddOrb(this.genericDamageOrb);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Idk what is really checked here other than the network check, but it applies at the end of the animation so it works
            if (this.animator.GetFloat("FireSeekingShot.fire") > 0f && base.isAuthority)
            {
                //Handle the firing
                Fire();
                //Set the next state
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //Can be interrupted by any more-important skills
            return InterruptPriority.PrioritySkill;
        }
    }
}
