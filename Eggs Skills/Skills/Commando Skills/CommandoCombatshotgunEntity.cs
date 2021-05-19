using RoR2;
using EntityStates;
using UnityEngine;
using EntityStates.Commando.CommandoWeapon;
using EggsBuffs;

namespace EggsSkills.EntityStates
{

    public class CombatShotgunEntity : BaseState
    {

        private float critMod;
        private float baseDuration = 0.4f;
        private float duration;
        private GameObject hitEffectPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark1");
        private GameObject tracerEffectPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/effects/tracers/TracerCommandoDefault");
        private string muzzleString;
        private GameObject muzzleEffectPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/effects/muzzleflashes/Muzzleflash1");
        private int stock;
        public override void OnEnter()
        {
            base.OnEnter();
            stock = base.skillLocator.primary.stock+4;
            this.duration = this.baseDuration / base.attackSpeedStat;
            if (stock % 2 == 1)
            {
                muzzleString = "MuzzleRight";
                base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
            }
            else
            {
                muzzleString = "MuzzleLeft";
                base.PlayAnimation("Gesture Additive, Left", "FirePistol, Left");
            }
            Util.PlaySound(FireShotgun.attackSoundString, base.gameObject);
            EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, base.gameObject, muzzleString, false);
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            if (base.isAuthority)
            {
                if (base.RollCrit())
                {
                    critMod = 1;
                }
                else
                {
                    critMod = 0;
                }
                BulletAttack bulletAttack = new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 1f - critMod,
                    maxSpread = 5f - (critMod * 2f),
                    bulletCount = 6u,
                    procCoefficient = 0.4f,
                    damage = base.characterBody.damage * 0.6f,
                    force = 3,
                    muzzleName = muzzleString,
                    falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                    tracerEffectPrefab = this.tracerEffectPrefab,
                    hitEffectPrefab = this.hitEffectPrefab,
                    isCrit = base.RollCrit(),
                    HitEffectNormal = false,
                    stopperMask = LayerIndex.world.mask,
                    smartCollision = true,
                    maxDistance = 200f,
                    damageType = DamageType.Generic
                };
                bulletAttack.Fire();
            }
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
