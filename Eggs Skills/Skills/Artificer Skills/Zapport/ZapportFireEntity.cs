using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using UnityEngine;
using RoR2;
using EntityStates.VagrantMonster;

namespace EggsSkills.EntityStates
{
    class ZapportFireEntity : BaseSkillState
    {
        internal float radius;
        internal float damageMult;
        internal Vector3 moveVec;
        private string rMuzzleString = "MuzzleRight";
        private string lMuzzleString = "MuzzleLeft";
        private GameObject explosionPrefab = UnityEngine.Resources.Load<GameObject>("Prefabs/effects/MageLightningBombExplosion");
        private GameObject muzzlePrefab = UnityEngine.Resources.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        public override void OnEnter()
        {
            base.OnEnter();
            characterMotor.velocity = Vector3.zero;
            base.PlayAnimation("Gesture, Additive", "FireWall");
            Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
            EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, lMuzzleString, false);
            EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, rMuzzleString, false);
            characterMotor.rootMotion += moveVec;
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public override void OnExit()
        {
            base.OnExit();
            EffectData endEffectData = new EffectData
            {
                scale = radius * 2f,
                origin = characterBody.corePosition
            };
            EffectManager.SpawnEffect(explosionPrefab, endEffectData, true);
            new BlastAttack
            {
                position = characterBody.corePosition,
                baseDamage = base.damageStat * damageMult,
                baseForce = 40 * damageMult,
                radius = radius,
                attacker = base.gameObject,
                inflictor = base.gameObject,
                teamIndex = base.teamComponent.teamIndex,
                crit = RollCrit(),
                procChainMask = default(ProcChainMask),
                procCoefficient = 1,
                bonusForce = new Vector3(0, 0, 0),
                falloffModel = BlastAttack.FalloffModel.None,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Stun1s,
                attackerFiltering = AttackerFiltering.Default
            }.Fire();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= 0.1f)
            {
                this.outer.SetNextStateToMain();
            }
        }
    }
}
