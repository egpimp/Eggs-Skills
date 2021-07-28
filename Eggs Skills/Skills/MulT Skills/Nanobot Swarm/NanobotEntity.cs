using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates.Toolbot;
using EntityStates.GolemMonster;
using EggsUtils.Helpers;

namespace EggsSkills.EntityStates
{
    class NanobotEntity : BaseSkillState
    {
        private ChildLocator muzzleLocator;

        private float baseDuration = 0.5f;
        private float duration;
        private float maxDist = 1000f;

        private GameObject lineEffect;

        private LineRenderer lineComponent;

        private Ray aimRay;

        private string muzzleString = "MuzzleNailgun";

        private Transform muzzleTransform;
        public override void OnEnter()
        {
            base.OnEnter();
            this.aimRay = base.GetAimRay();
            this.muzzleLocator = base.modelLocator.modelTransform.GetComponent<ChildLocator>();
            this.muzzleTransform = this.muzzleLocator.FindChild(this.muzzleString);
            this.lineEffect = Object.Instantiate<GameObject>(ChargeLaser.laserPrefab);
            this.lineEffect.SetActive(true);
            this.lineComponent = this.lineEffect.GetComponent<LineRenderer>();
            this.aimRay.origin = this.muzzleTransform.position;
            this.lineComponent.startColor = Color.white;
            this.lineComponent.endColor = Color.white;
            GradientColorKey[] colorKey = new GradientColorKey[1];
            colorKey[0].color = Color.white;
            this.lineComponent.colorGradient.colorKeys = colorKey;
            this.lineComponent.startWidth = 0.1f;
            this.lineComponent.endWidth = 0.1f;
            this.duration = this.baseDuration / base.attackSpeedStat;
            base.PlayAnimation("Gesture, Additive", "PrepBomb", "PrepBomb.playbackRate", this.duration);
            base.PlayAnimation("Stance, Override", "PutAwayGun");
            Util.PlaySound(AimStunDrone.enterSoundString, base.gameObject);
        }
        public override void OnExit()
        {
            base.OnExit();
            if(this.lineEffect)
            {
                this.lineEffect.SetActive(false);
                Destroy(this.lineEffect);
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!base.IsKeyDownAuthority() && base.fixedAge >= this.duration && base.isAuthority)
            {
                this.Fire();
                this.outer.SetNextStateToMain();
            }
            this.aimRay = base.GetAimRay();
            RaycastHit hit;
            Vector3 origHitPos = base.inputBank.GetAimRaycast(this.maxDist, out hit) ? hit.point : this.aimRay.GetPoint(this.maxDist);
            this.aimRay.origin = this.muzzleTransform.position;
            this.lineEffect.transform.parent = this.muzzleTransform;
            this.aimRay.direction = Math.GetDirection(this.aimRay.origin, origHitPos);
            Vector3 newHitPos = Physics.Raycast(this.aimRay.origin, this.aimRay.direction, out hit, this.maxDist) ? hit.point : this.aimRay.GetPoint(this.maxDist);
            this.lineComponent.SetPositions(new Vector3[] { this.aimRay.origin, newHitPos});
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
        private void Fire()
        {
            if (base.isAuthority)
            {
                ProjectileManager.instance.FireProjectile(Resources.Projectiles.nanoBeaconPrefab, this.aimRay.origin, Util.QuaternionSafeLookRotation(this.aimRay.direction), base.gameObject, base.damageStat, 50f, base.RollCrit());
            }
            base.PlayAnimation("Gesture, Additive", "FireBomb", "FireBomb.playbackRate", duration);
            base.PlayCrossfade("Stance, Override", "Empty", 0.1f);
            Util.PlaySound(AimStunDrone.exitSoundString, base.gameObject);
        }
    }
}
