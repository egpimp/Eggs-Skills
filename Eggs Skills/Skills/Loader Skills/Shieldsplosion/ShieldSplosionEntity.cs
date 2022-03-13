using RoR2;
using EntityStates;
using UnityEngine;
using EntityStates.JellyfishMonster;
using EggsSkills.Config;
using EggsUtils.Helpers;

namespace EggsSkills.EntityStates
{
    class ShieldSplosionEntity : BaseSkillState
    {
        //Should the barrier be removed on use?
        private readonly bool shouldRemoveBarrier = Configuration.GetConfigValue(Configuration.LoaderShieldsplodeRemovebarrieronuse);

        //Force of the blast
        private readonly float baseForce = 50f;
        //Min radius at min barrier
        private readonly float baseRadius = Configuration.GetConfigValue(Configuration.LoaderShieldsplodeBaseradius);
        //Damage coefficient
        private readonly float damageCoefficient = 20f;
        //Max radius multiplier
        private readonly float maxRadiusMult = 3f;
        //Proc coefficient
        private readonly float procCoefficient = 1f;

        //FX prefab
        private GameObject bodyPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/JellyfishNova");

        //Player health component
        public HealthComponent component;

        public override void OnEnter()
        {
            //Grab the health component
            this.component = base.healthComponent;
            base.OnEnter();
        }
        public override void OnExit()
        {
            //Take damage coeff and multiply it by % barrier to translate amount to damage
            float damageMod = (this.component.barrier / this.component.fullCombinedHealth) * this.damageCoefficient;
            //Get the force based on base force and half the damage multiplier
            float force = this.baseForce * (damageMod / 2);
            //Radius is based on damage, tripled at max barrier
            float radius = this.baseRadius * Math.ConvertToRange(2f, this.damageCoefficient, 1f, this.maxRadiusMult, damageMod);
            //Network check
            if (base.isAuthority)
            {
                //If barrier should be removed, remove it
                if (this.shouldRemoveBarrier) this.component.AddBarrier(-component.barrier);
                //Perform the balst attack
                new BlastAttack
                {
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    //Triple damage at this part to match thing
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
            }
            //Play the sound
            Util.PlaySound(JellyNova.novaSoundString, base.gameObject);
            //Setup fx data
            EffectData effectData = new EffectData
            {
                origin = base.characterBody.corePosition,
                color = Color.yellow,
                scale = radius
            };
            //Spawn the fx
            EffectManager.SpawnEffect(this.bodyPrefab, effectData, true);
            //Apply the speed-buff from 'shedding' barrier
            this.characterBody.AddTimedBuff(RoR2Content.Buffs.CloakSpeed, 3f);
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Check network, and the very short cast time
            if (base.fixedAge >= 0.1f && base.isAuthority)
            {
                //Then set next state
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //Interruptible by anything
            return InterruptPriority.Skill;
        }
    }
}
