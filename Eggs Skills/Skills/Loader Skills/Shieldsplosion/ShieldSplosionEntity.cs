using RoR2;
using EntityStates;
using UnityEngine;
using EntityStates.JellyfishMonster;
using EggsSkills.Utility;

namespace EggsSkills.EntityStates
{
    class ShieldSplosionEntity : BaseSkillState
    {
        private float baseForce = 50f;
        private float baseRadius = 10f;
        private float damageCoefficient = 20f;
        private float procCoefficient = 1f;

        private GameObject bodyPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/effects/JellyfishNova");

        public HealthComponent component;
        public override void OnEnter()
        {
            this.component = base.healthComponent;
            base.OnEnter();
        }
        public override void OnExit()
        {
            float damageMod = (this.component.barrier / this.component.fullCombinedHealth) * this.damageCoefficient;
            float force = this.baseForce * (damageMod / 2);
            float radius = this.baseRadius * Utilities.ConvertToRange(2f, 20f, 1f, 2f, damageMod);
            
            if (base.isAuthority)
            {
                this.component.AddBarrier(-component.barrier);
                new BlastAttack
                {
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    baseDamage = base.damageStat * damageMod * 3,
                    position = base.characterBody.corePosition,
                    radius = radius,
                    baseForce = force,
                    crit = base.RollCrit(),
                    teamIndex = base.teamComponent.teamIndex,
                    procChainMask = default,
                    procCoefficient = this.procCoefficient,
                    falloffModel = BlastAttack.FalloffModel.None,
                    damageColorIndex = default,
                    damageType = DamageType.Generic,
                    attackerFiltering = default
                }.Fire();
                Util.PlaySound(JellyNova.novaSoundString, base.gameObject);
                EffectData effectData = new EffectData
                {
                    origin = base.characterBody.corePosition,
                    color = Color.yellow,
                    scale = radius
                };
                EffectManager.SpawnEffect(this.bodyPrefab, effectData, true);
                this.characterBody.AddTimedBuff(RoR2Content.Buffs.CloakSpeed, 3f);
            }
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= 0.1f && base.isAuthority)
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
