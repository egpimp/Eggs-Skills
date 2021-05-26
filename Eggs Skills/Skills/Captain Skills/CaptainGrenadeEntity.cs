using EntityStates;
using RoR2;
using RoR2.Projectile;
using EggsSkills.Resources;

namespace EggsSkills.EntityStates
{
    class DebuffGrenadeEntity : BaseState
    {
        public float baseDelay = 0.6f;
        private float damageCoefficient = 2.5f;
        public float delay;
        public override void OnEnter()
        {
            base.OnEnter();
            this.delay = this.baseDelay / base.attackSpeedStat;
            var aimRay = GetAimRay();
            Util.PlaySound(FireFlower2.enterSoundString, base.gameObject);
            base.PlayAnimation("Gesture, Additive", "FireCaptainShotgun");
            base.PlayAnimation("Gesture, Override", "FireCaptainShotgun");
            if (base.isAuthority)
            {
                ProjectileManager.instance.FireProjectile(Projectiles.debuffGrenadePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, base.damageStat * this.damageCoefficient , 0f, base.RollCrit());
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.fixedAge >= this.delay && base.isAuthority)
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
