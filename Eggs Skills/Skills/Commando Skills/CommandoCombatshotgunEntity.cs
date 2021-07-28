using RoR2;
using EntityStates;
using UnityEngine;
using EntityStates.Commando.CommandoWeapon;
using RoR2.Skills;
using EggsSkills.Config;

namespace EggsSkills.EntityStates
{

    public class CombatShotgunEntity : BaseState, SteppedSkillDef.IStepSetter
    {
        private float baseDuration = 0.4f;
        private float baseForce = 10f;
        private float baseRecoil = 0.6f;
        private float damageCoefficient = 0.6f;
        private float duration;
        private float maxDist = 200f;
        private float procCoefficient = 0.4f;

        private GameObject hitEffectPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark1");
        private GameObject muzzleEffectPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/effects/muzzleflashes/Muzzleflash1");
        private GameObject tracerEffectPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/effects/tracers/TracerCommandoDefault");

        private int critMod;
        private int step;

        private uint bulletCount = Configuration.GetConfigValue<uint>(Configuration.CommandoShotgunPellets);

        void SteppedSkillDef.IStepSetter.SetStep(int i)
        {
            step = i;
        }
        public override void OnEnter()
        {
            base.OnEnter();
            bool isCrit;
            this.duration = this.baseDuration / base.attackSpeedStat;
            string muzzleString;
            isCrit = base.RollCrit();
            critMod = isCrit ? 1 : 0;
            if (step % 2 == 1)
            {
                muzzleString = "MuzzleRight";
                base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
            }
            else
            {
                muzzleString = "MuzzleLeft";
                base.PlayAnimation("Gesture Additive, Left", "FirePistol, Left");
            }
            BulletFire(muzzleString, isCrit);
        }
        private void BulletFire(string muzzleName, bool isCrit)
        {
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, duration * 2f, false);
            if (base.isAuthority)
            {
                new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 1f - this.critMod,
                    maxSpread = 5f - (this.critMod * 2f),
                    bulletCount = this.bulletCount,
                    procCoefficient = this.procCoefficient,
                    damage = base.characterBody.damage * this.damageCoefficient,
                    force = this.baseForce,
                    muzzleName = muzzleName,
                    falloffModel = default,
                    tracerEffectPrefab = this.tracerEffectPrefab,
                    hitEffectPrefab = this.hitEffectPrefab,
                    isCrit = isCrit,
                    HitEffectNormal = false,
                    smartCollision = true,
                    maxDistance = maxDist,
                    damageType = DamageType.Generic,
                }.Fire();
            }
            EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, base.gameObject, muzzleName, false);
            Util.PlaySound(FireShotgun.attackSoundString, base.gameObject);
            base.AddRecoil(-this.baseRecoil, this.baseRecoil, -2 * this.baseRecoil * (1 - (step % 2)), 2 * this.baseRecoil * (step % 2));
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
