using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates.Toolbot;

namespace EggsSkills.EntityStates
{
    class NanobotEntity : BaseState
    {
        private float baseDuration = 0.5f;
        private float duration;
        private Ray aimRay;
        private string muzzleString = "MuzzleNailgun";
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / base.attackSpeedStat;
            base.PlayAnimation("Gesture, Additive", "PrepBomb", "PrepBomb.playbackRate", duration);
            base.PlayAnimation("Stance, Override", "PutAwayGun");
            
        }
        public override void OnExit()
        {
            base.OnExit();
            aimRay = GetAimRay();
            ChildLocator component = base.modelLocator.modelTransform.GetComponent<ChildLocator>();
            Transform transform = component.FindChild(muzzleString);
            if (base.isAuthority)
            {
                ProjectileManager.instance.FireProjectile(EggsSkills.Resources.Projectiles.nanoBeaconPrefab, transform.position, Util.QuaternionSafeLookRotation(aimRay.direction), gameObject, damageStat, 50f, RollCrit());
            }
            Util.PlaySound(AimStunDrone.enterSoundString, base.gameObject);
            base.PlayAnimation("Gesture, Additive", "FireBomb", "FireBomb.playbackRate", duration);
            base.PlayCrossfade("Stance, Override", "Empty", 0.1f);
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.fixedAge >= duration && base.isAuthority)
            {
                outer.SetNextStateToMain();
            }

        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
