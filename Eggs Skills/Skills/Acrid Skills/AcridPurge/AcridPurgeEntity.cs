using EggsSkills.Config;
using EntityStates;
using EntityStates.Croco;
using RoR2;
using UnityEngine;

namespace EggsSkills.EntityStates
{
    class AcridPurgeEntity : BaseState
    {
        private float blightDamageCoefficient = 3f;
        private float detonationRadius = Configuration.GetConfigValue<float>(Configuration.CrocoPurgeBaseradius);
        private float healthFraction = 0.1f;
        private float maxTrackingDistance = 5000f;
        private float poisonDamageCoefficient = 2.5f;
        private float procCoefficient = 1f;

        private GameObject bodyPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/effects/impacteffects/CrocoDiseaseImpactEffect");
        public override void OnEnter()
        {
            base.OnEnter();
            {
                base.PlayAnimation("Gesture, Mouth", "FireSpit", "FireSpit.playbackRate", 1f);
                foreach (HurtBox hurtBox in new SphereSearch
                {
                    origin = base.characterBody.footPosition,
                    radius = this.maxTrackingDistance,
                    mask = LayerIndex.entityPrecise.mask
                }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
                {
                    CharacterBody body = hurtBox.healthComponent.body;
                    HealthComponent component = hurtBox.healthComponent;
                    if(body.HasBuff(RoR2Content.Buffs.Poisoned))
                    {
                        if (base.isAuthority)
                        {
                            new BlastAttack
                            {
                                position = body.corePosition,
                                baseDamage = component.fullHealth * this.healthFraction + base.damageStat * this.poisonDamageCoefficient,
                                baseForce = 0f,
                                radius = this.detonationRadius,
                                attacker = base.gameObject,
                                inflictor = base.gameObject,
                                teamIndex = base.teamComponent.teamIndex,
                                crit = base.RollCrit(),
                                procChainMask = default,
                                procCoefficient = this.procCoefficient,
                                falloffModel = BlastAttack.FalloffModel.None,
                                damageColorIndex = default,
                                damageType = DamageType.Generic,
                                attackerFiltering = default
                            }.Fire();
                        }
                        EffectManager.SimpleSoundEffect(BaseLeap.landingSound.index, body.footPosition, true);
                        EffectData bodyEffectData = new EffectData
                        {
                            origin = body.corePosition,
                            color = Color.green,
                            scale = this.detonationRadius
                        };
                        EffectManager.SpawnEffect(bodyPrefab, bodyEffectData, true);
                    }
                    else if(body.HasBuff(RoR2Content.Buffs.Blight))
                    {
                        if (base.isAuthority)
                        {
                            new BlastAttack
                            {
                                position = body.corePosition,
                                baseDamage = base.damageStat * (this.blightDamageCoefficient * body.GetBuffCount(RoR2Content.Buffs.Blight)),
                                baseForce = 0f,
                                radius = this.detonationRadius,
                                attacker = base.gameObject,
                                inflictor = base.gameObject,
                                teamIndex = base.teamComponent.teamIndex,
                                crit = base.RollCrit(),
                                procChainMask = default,
                                procCoefficient = 1,
                                falloffModel = BlastAttack.FalloffModel.None,
                                damageColorIndex = default,
                                damageType = DamageType.Generic,
                                attackerFiltering = default
                            }.Fire();
                        }
                        EffectManager.SimpleSoundEffect(BaseLeap.landingSound.index, body.footPosition, true);
                        EffectData bodyEffectData = new EffectData
                        {
                            origin = body.corePosition,
                            color = Color.yellow,
                            scale = this.detonationRadius
                        };
                        EffectManager.SpawnEffect(bodyPrefab, bodyEffectData, true);
                    }
                }
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.fixedAge >= 0.1f && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            };
        }
    }
}
