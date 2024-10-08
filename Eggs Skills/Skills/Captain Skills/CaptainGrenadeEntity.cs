﻿using EntityStates;
using RoR2;
using RoR2.Projectile;
using EggsSkills.Resources;

namespace EggsSkills.EntityStates
{
    class DebuffGrenadeEntity : BaseState
    {
        //Skills++
        public static float spp_damageMult = 1f;

        //Standard cast time
        private static readonly float baseDelay = 0.6f;
        //Damage coefficient
        private static readonly float damageCoefficient = 2.5f * spp_damageMult;
        //Post-attackspeed factoring cast time
        private float delay;

        private static readonly string soundString = "Play_treeBot_R_shoot";

        public override void OnEnter()
        {
            //Engage enter protocol
            base.OnEnter();
            //Set delay with the attack speed
            delay = baseDelay / base.attackSpeedStat;
            //Grab aimray
            var aimRay = GetAimRay();
            //Play the firing sound
            Util.PlaySound(soundString, base.gameObject);
            //Play animations 
            base.PlayAnimation("Gesture, Additive", "FireCaptainShotgun");
            base.PlayAnimation("Gesture, Override", "FireCaptainShotgun");
            //Network check, then fire projectile
            if (base.isAuthority) ProjectileManager.instance.FireProjectile(Projectiles.debuffGrenadePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, base.damageStat * damageCoefficient, 0f, base.RollCrit());
            
        }
        public override void FixedUpdate()
        {
            //Standard update procedure
            base.FixedUpdate();
            //If cast time up, and network check
            if(base.fixedAge >= delay && base.isAuthority)
            {
                //Set next state
                outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //Interrupted by any more important skills
            return InterruptPriority.PrioritySkill;
        }
    }
}
