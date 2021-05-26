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
        private float damageCoefficient = 2f;
        private float baseDelay = 0.4f;
        private float delay;
        public override void OnEnter()
        {
            base.OnEnter();
            this.delay = this.baseDelay / base.attackSpeedStat;
            Ray aimRay = GetAimRay();
            StartAimMode(aimRay);
            Util.PlaySound(FireMines.throwMineSoundString, base.gameObject);
            if(GetModelAnimator())
            {
                base.PlayCrossfade("Esture, Additive","FireMineRight","FireMine.playbackRate", this.delay , 0.05f);
            }
            if(base.isAuthority)
            {
                ProjectileManager.instance.FireProjectile(Projectiles.teslaMinePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction) , base.gameObject, base.damageStat * this.damageCoefficient, 0f, base.RollCrit());
            };
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.delay && base.isAuthority)
            {
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
