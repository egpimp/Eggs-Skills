using EggsSkills.Orbs;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EggsSkills.Skills.Engi_Skills.MicroMissiles
{
    internal class MicroMissileEntity : BaseSkillState, SteppedSkillDef.IStepSetter
    {
        //Skills++ mod
        public static float spp_damageMult = 1f;

        //Pre-attack damage coefficient
        internal static readonly float damageCoef = 0.2f;
        //baseduration
        private static readonly float baseDuration = 0.45f;
        //recoil
        private static readonly float baseRecoil = 0.75f;
        //duration with attack speed
        private float duration;

        //For alternating cannon
        private int step;

        private Transform modelTransform;

        void SteppedSkillDef.IStepSetter.SetStep(int i)
        {
            //Handles determining step
            step = i;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            base.StartAimMode();
            duration = baseDuration / base.attackSpeedStat;
            string muzzle;
            //Get model transform
            modelTransform = GetModelTransform();
            //Get the alternating muzzle
            if (step % 2 == 0)
            {
                muzzle = "MuzzleLeft";
                base.PlayCrossfade("Gesture Left Cannon, Additive", "FireGrenadeLeft", 0.2f);
            }
            else
            {
                muzzle = "MuzzleRight";
                base.PlayCrossfade("Gesture Right Cannon, Additive", "FireGrenadeRight", 0.2f);
            }
            FireTracker(muzzle);
            Ray aimRay = base.GetAimRay();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.fixedAge > duration)
            {
                outer.SetNextStateToMain();
            }
        }
        
        private void FireTracker(string muzzle)
        {
            //Get ray
            Ray aimRay = GetAimRay();
            //Use childlocator to get the muzzle transform, and use that to find tracker origin position
            if(modelTransform)
            {
                ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                if(component)
                {
                    Transform transform = component.FindChild(muzzle);
                    if (transform) aimRay.origin = transform.position;
                }
            }
            //Fire projectile
            ProjectileManager.instance.FireProjectile(Resources.Projectiles.micromissileMarkerPrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, base.damageStat * damageCoef * spp_damageMult, 50f, base.RollCrit(), damageType: DamageTypeCombo.GenericPrimary);
            base.AddRecoil(-baseRecoil, baseRecoil, -baseRecoil, baseRecoil);
            base.characterBody.AddSpreadBloom(baseRecoil * 2f);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
