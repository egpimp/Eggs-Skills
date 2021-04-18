using RoR2;
using EntityStates;
using UnityEngine;
using EntityStates.JellyfishMonster;
using EggsSkills;

namespace EggsSkills.EntityStates
{
    class ShieldSplosionEntity : BaseSkillState
    {
        public HealthComponent component;
        private GameObject bodyPrefab = Resources.Load<GameObject>("prefabs/effects/JellyfishNova");
        public override void OnEnter()
        {
            component = base.healthComponent;
            base.OnEnter();
        }
        public override void OnExit()
        {
            float damageMod = (component.barrier / component.fullCombinedHealth) * 60;
            float radius = 20f * ((damageMod + 16f) / 18f);
            component.Networkbarrier = 0;
            new BlastAttack
            {
                attacker = base.gameObject,
                inflictor = base.gameObject,
                baseDamage = base.damageStat * damageMod,
                position = base.characterBody.corePosition,
                radius = radius,
                baseForce = 200f,
                crit = base.RollCrit(),
                teamIndex = base.teamComponent.teamIndex,
                procChainMask = default,
                procCoefficient = 1,
                bonusForce = new Vector3(0,0,0),
                falloffModel = BlastAttack.FalloffModel.None,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Generic,
                attackerFiltering = AttackerFiltering.Default
            }.Fire();
            Util.PlaySound(JellyNova.novaSoundString,base.gameObject);
            EffectData effectData = new EffectData
            {
                origin = base.characterBody.corePosition,
                color = Color.yellow,
                scale = radius
            };
            EffectManager.SpawnEffect(bodyPrefab, effectData, true);
            characterBody.AddTimedBuff(RoR2Content.Buffs.CloakSpeed,3f);
            characterBody.RecalculateStats();
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            if(base.fixedAge >= 0.1f)
            {
                this.outer.SetNextStateToMain();
            }
            base.FixedUpdate();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
