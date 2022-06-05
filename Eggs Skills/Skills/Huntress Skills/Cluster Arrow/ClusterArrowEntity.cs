using EggsSkills.Orbs;
using EntityStates;
using EntityStates.Huntress.HuntressWeapon;
using RoR2;
using RoR2.Orbs;
using UnityEngine;
using UnityEngine.Networking;

namespace EggsSkills.EntityStates
{
    internal class ClusterBombArrow : BaseState
    {
        //SkillS++
        public static float spp_damageMult = 1f;
        public static int spp_bombletBonus = 0;

        //Animator
        private Animator animator;

        //Was glaive throw attempted?
        private bool wasArrowAttempted = false;
        //Has the glaive been thrown successfully?
        private bool wasArrowFired = false;

        //Helps find assets
        private readonly FireSeekingArrow assetRef = new FireSeekingArrow();

        //Base cast time
        private readonly float baseDuration = 1f;
        //Post-Attack speed cast time
        private float duration;

        //Hurtbox of the target
        private HurtBox target;

        //Muzzle to fire from
        private Transform muzzle;

        public override void OnEnter()
        {
            base.OnEnter();
            //Grab the tracking component
            HuntressTracker huntressTracker = base.GetComponent<HuntressTracker>();
            //Find the target
            if(huntressTracker && base.isAuthority) target = huntressTracker.GetTrackingTarget();
            //Determine the cast-time
            duration = baseDuration / base.attackSpeedStat;
            //Get the transform of the model
            Transform modelTransform = base.GetModelTransform();
            //If the model transform exists
            if(modelTransform)
            {
                //Grab the child locator
                ChildLocator childLocator = modelTransform.GetComponent<ChildLocator>();
                //Find the muzzle with the given string
                muzzle = childLocator.FindChild(assetRef.muzzleString);
                //Nab the animator
                animator = modelTransform.GetComponent<Animator>();
            }
            //Play the sound based on attack speed
            Util.PlayAttackSpeedSound(this.assetRef.attackSoundString, base.gameObject, base.attackSpeedStat);
            //Make sure body exists and do the thing where it looks towards where you aim
            if(base.characterBody) base.characterBody.SetAimTimer(duration + 1f);
            //Play the animations
            base.PlayCrossfade("Gesture, Override", "FireSeekingShot", "FireSeekingShot.playbackRate", duration, duration * 0.2f / attackSpeedStat);
            base.PlayCrossfade("Gesture, Additive", "FireSeekingShot", "FireSeekingShot.playbackRate", duration, duration * 0.2f / attackSpeedStat);
        }

        public override void OnExit()
        {
            base.OnExit();
            //If it wasn't even attempted, try again
            if(!wasArrowAttempted) Fire();
            //If for some reason it never even fired, refresh the stock
            if (!wasArrowFired && NetworkServer.active) base.skillLocator.secondary.AddOneStock();
        }

        private void Fire()
        {
            //Keeps it on network only, and doesn't allow multiple arrow attempts
            if (!NetworkServer.active || wasArrowAttempted) return;
            //We have officially now made the attempt
            wasArrowAttempted = true;
            //Establish the orb
            GenericDamageOrb genericDamageOrb = new HuntressBombArrowOrb()
            {
                spp_orbDamageMult = spp_damageMult,
                spp_orbBombletBonus = spp_bombletBonus
            };
            //Set the damage to the player damage
            genericDamageOrb.damageValue = base.characterBody.damage;
            genericDamageOrb.isCrit = base.RollCrit();
            //Set the team of the orb to the player team
            genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
            //Set the attacker to the player
            genericDamageOrb.attacker = base.gameObject;
            //If the target still exists by the time we fire
            if (target)
            {
                wasArrowFired = true;
                //Apply the muzzle flash effect
                EffectManager.SimpleMuzzleFlash(assetRef.muzzleflashEffectPrefab, base.gameObject, assetRef.muzzleString, false);
                //Set the origin to the muzzle position
                genericDamageOrb.origin = muzzle.position;
                //Set the target of the orb to the target we found
                genericDamageOrb.target = target;
                //Add the orb (Fire it)
                OrbManager.instance.AddOrb(genericDamageOrb);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Idk what is really checked here, but it applies at the end of the animation so it works
            if (!wasArrowAttempted && animator.GetFloat("FireSeekingShot.fire") > 0f) Fire();
            //Set the next state
            if(base.fixedAge >= duration && base.isAuthority) outer.SetNextStateToMain();
            return;
        }

        //I don't understand networking shit very well, but I assume these help make sure the target isn't broken by weird lag shit
        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(target));
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            target = reader.ReadHurtBoxReference().ResolveHurtBox();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //Can be interrupted by any more-important skills
            return InterruptPriority.PrioritySkill;
        }
    }
}
