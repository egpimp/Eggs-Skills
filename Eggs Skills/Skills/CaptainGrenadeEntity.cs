using EntityStates;
using EntityStates.Treebot;
using RoR2;
using RoR2.Projectile;
using EggsSkills.Properties;

namespace EggsSkills.EntityStates
{
    class DebuffGrenadeEntity : BaseState
    {
        public float baseDelay = 0.6f;
        public float delay;
        public override void OnEnter()
        {
            base.OnEnter();
            delay = baseDelay / attackSpeedStat;
            var aimRay = GetAimRay();
            if (base.isAuthority)
            {
                Util.PlaySound(FireFlower2.enterSoundString, gameObject);
                base.PlayAnimation("Gesture, Additive", "FireCaptainShotgun");
                base.PlayAnimation("Gesture, Override", "FireCaptainShotgun");
                ProjectileManager.instance.FireProjectile(Assets.debuffGrenadePrefab,aimRay.origin,RoR2.Util.QuaternionSafeLookRotation(aimRay.direction),gameObject,damageStat * 2.5f,0,RollCrit());
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.fixedAge >= delay)
            {
                this.outer.SetNextStateToMain();
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
