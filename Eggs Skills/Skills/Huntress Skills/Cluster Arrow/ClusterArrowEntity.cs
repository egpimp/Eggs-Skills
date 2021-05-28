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
        private Animator animator;

        private ChildLocator childLocator;

        private FireSeekingArrow assetRef = new FireSeekingArrow();

        private float baseDuration = 1f;
        private float duration;

        private GenericDamageOrb genericDamageOrb;

        private HuntressTracker huntressTracker;
        private HurtBox target;

        private Transform muzzle;
        public override void OnEnter()
        {
            base.OnEnter();
            this.huntressTracker = GetComponent<HuntressTracker>();
            this.genericDamageOrb = new HuntressBombArrowOrb();
            this.target = huntressTracker.GetTrackingTarget();
            this.duration = baseDuration / attackSpeedStat;
            Transform modelTransform = GetModelTransform();
            if(modelTransform)
            {
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                this.muzzle = childLocator.FindChild(assetRef.muzzleString);
                this.animator = modelTransform.GetComponent<Animator>();
            }
            Util.PlayAttackSpeedSound(assetRef.attackSoundString, gameObject, attackSpeedStat);
            if(base.characterBody)
            {
                base.characterBody.SetAimTimer(duration + 1f);
            }
            base.PlayCrossfade("Gesture, Override", "FireSeekingShot", "FireSeekingShot.playbackRate", this.duration, this.duration * 0.2f / this.attackSpeedStat);
            base.PlayCrossfade("Gesture, Additive", "FireSeekingShot", "FireSeekingShot.playbackRate", this.duration, this.duration * 0.2f / this.attackSpeedStat);
        }
        private void Fire()
        {
            this.genericDamageOrb.damageValue = base.characterBody.damage;
            this.genericDamageOrb.isCrit = RollCrit();
            this.genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(gameObject);
            this.genericDamageOrb.attacker = base.gameObject;
            if (this.target)
            {
                EffectManager.SimpleMuzzleFlash(this.assetRef.muzzleflashEffectPrefab, base.gameObject, this.assetRef.muzzleString, false);
                this.genericDamageOrb.origin = this.muzzle.position;
                this.genericDamageOrb.target = this.target;
                if (base.isAuthority)
                {
                    OrbManager.instance.AddOrb(this.genericDamageOrb);
                }
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.animator.GetFloat("FireSeekingShot.fire") > 0f && base.isAuthority)
            {
                Fire();
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
