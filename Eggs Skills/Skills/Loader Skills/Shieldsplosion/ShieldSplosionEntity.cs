﻿using RoR2;
using EntityStates;
using UnityEngine;
using EntityStates.JellyfishMonster;
using EggsSkills.Config;
using EggsUtils.Helpers;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;

namespace EggsSkills.EntityStates
{
    class ShieldSplosionEntity : BaseSkillState
    {
        //Skills++
        internal static float spp_radiusMult = 1f;
        internal static float spp_refund = 0f;

        //Force of the blast
        private static readonly float baseForce = 50f;
        //Min radius at min barrier
        private static readonly float baseRadius = Configuration.GetConfigValue(Configuration.LoaderShieldsplodeBaseradius);
        //Damage coefficient
        private static readonly float damageCoefficient = 20f;
        //Max radius multiplier
        private static readonly float maxRadiusMult = 3f;
        //Proc coefficient
        private static readonly float procCoefficient = 1f;

        //FX prefab
        private GameObject bodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Jellyfish/JellyfishNova.prefab").WaitForCompletion();

        private static readonly string soundString = "Play_jellyfish_detonate";

        //Player health component
        private HealthComponent component;

        public override void OnEnter()
        {
            //Grab the health component
            component = base.healthComponent;
            base.OnEnter();
        }
        public override void OnExit()
        {
            //Take damage coeff and multiply it by % barrier to translate amount to damage
            float damageMod = (component.barrier / component.fullCombinedHealth) * damageCoefficient;
            //Get the force based on base force and half the damage multiplier
            float force = baseForce * (damageMod / 2);
            //Radius is based on damage, tripled at max barrier
            float radius = baseRadius * Math.ConvertToRange(2f, damageCoefficient, 1f, maxRadiusMult, damageMod);
            //If barrier should be removed, remove it
            if (NetworkServer.active) component.AddBarrier(-component.barrier);
            //Network check
            if (base.isAuthority)
            {
                //Perform the balst attack
                BlastAttack atk = new BlastAttack()
                {
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    //Triple damage at this part to match thing
                    baseDamage = base.damageStat * damageMod * 3,
                    position = base.characterBody.corePosition,
                    radius = radius * spp_radiusMult,
                    baseForce = force,
                    crit = base.RollCrit(),
                    teamIndex = base.teamComponent.teamIndex,
                    procCoefficient = procCoefficient,
                    falloffModel = BlastAttack.FalloffModel.None,
                    damageType = DamageType.AOE | DamageTypeCombo.GenericSpecial
                };
                atk.Fire();
            }
            //Play the sound
            Util.PlaySound(soundString, base.gameObject);
            //Refund barrier if spp enabled
            if (NetworkServer.active) component.AddBarrier(component.fullCombinedHealth * spp_refund);
            //Setup fx data
            EffectData effectData = new EffectData
            {
                origin = base.characterBody.corePosition,
                color = Color.yellow,
                scale = radius
            };
            //Spawn the fx
            EffectManager.SpawnEffect(bodyPrefab, effectData, true);
            //Apply the speed-buff from 'shedding' barrier
            characterBody.AddTimedBuff(RoR2Content.Buffs.CloakSpeed, 3f);
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Check network, and the very short cast time
            if (base.fixedAge >= 0.1f && base.isAuthority)
            {
                //Then set next state
                outer.SetNextStateToMain();
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
