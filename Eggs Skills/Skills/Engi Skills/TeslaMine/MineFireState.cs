using RoR2;
using EntityStates;
using UnityEngine;
using RoR2.Projectile;
using EggsSkills.Resources;
using EntityStates.Engi.EngiWeapon;

namespace EggsSkills.EntityStates
{
    class TeslaMineFireState : BaseState
    
    {
        //Basic cast time
        private readonly float baseDelay = 0.4f;
        //Damage coefficient
        private readonly float damageCoefficient = 2f;
        //Cast time
        private float delay;

        public override void OnEnter()
        {
            base.OnEnter();
            //Set delay based on attack speed
            delay = baseDelay / base.attackSpeedStat;
            //Get aimray
            Ray aimRay = base.GetAimRay();
            //Start aim mode
            base.StartAimMode(aimRay);
            //Play sound
            Util.PlaySound(FireMines.throwMineSoundString, base.gameObject);
            //If animator exists play the animation
            if(base.GetModelAnimator()) base.PlayCrossfade("Esture, Additive","FireMineRight","FireMine.playbackRate", delay , 0.05f);
            //And if network check, fire the projectile
            if(base.isAuthority) ProjectileManager.instance.FireProjectile(Projectiles.teslaMinePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction) , base.gameObject, base.damageStat * damageCoefficient, 0f, base.RollCrit());
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //If skill finished, set to next state
            if (base.fixedAge >= delay && base.isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //Can be interrupted by more important skills
            return InterruptPriority.PrioritySkill;
        }
    }
}
