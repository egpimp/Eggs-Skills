using System;
using System.Collections.Generic;
using System.Text;
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
        private HuntressTracker huntressTracker;
        private GenericDamageOrb genericDamageOrb;
        private ChildLocator childLocator;
        private Animator animator;
        private HurtBox target;
        private float baseDuration = 1f;
        private float duration;
        private Transform muzzle;
        private FireSeekingArrow assetRef = new FireSeekingArrow();
        public override void OnEnter()
        {
            base.OnEnter();
            huntressTracker = GetComponent<HuntressTracker>();
            genericDamageOrb = new HuntressBombArrowOrb();
            target = huntressTracker.GetTrackingTarget();
            duration = baseDuration / attackSpeedStat;
            Transform modelTransform = GetModelTransform();
            if(modelTransform)
            {
                childLocator = modelTransform.GetComponent<ChildLocator>();
                muzzle = childLocator.FindChild(assetRef.muzzleString);
                animator = modelTransform.GetComponent<Animator>();
            }
            Util.PlayAttackSpeedSound(assetRef.attackSoundString, gameObject, attackSpeedStat);
            if(characterBody)
            {
                characterBody.SetAimTimer(duration + 1f);
            }
            base.PlayCrossfade("Gesture, Override", "FireSeekingShot", "FireSeekingShot.playbackRate", this.duration, this.duration * 0.2f / this.attackSpeedStat);
            base.PlayCrossfade("Gesture, Additive", "FireSeekingShot", "FireSeekingShot.playbackRate", this.duration, this.duration * 0.2f / this.attackSpeedStat);
        }
        private void Fire()
        {
            if (base.isAuthority)
            {
                genericDamageOrb.damageValue = characterBody.damage;
                genericDamageOrb.isCrit = RollCrit();
                genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(gameObject);
                genericDamageOrb.attacker = gameObject;
                if (target)
                {
                    EffectManager.SimpleMuzzleFlash(assetRef.muzzleflashEffectPrefab, gameObject, assetRef.muzzleString, false);
                    genericDamageOrb.origin = muzzle.position;
                    genericDamageOrb.target = target;
                    OrbManager.instance.AddOrb(genericDamageOrb);
                }
            }
            outer.SetNextStateToMain();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.animator.GetFloat("FireSeekingShot.fire") > 0f)
            {
                Fire();
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
