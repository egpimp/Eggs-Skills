using EntityStates;
using EntityStates.Croco;
using RoR2;
using UnityEngine;

namespace EggsSkills.EntityStates
{
    class AcridPurgeEntity : BaseState
    {
        public float maxTrackingDistance = 5000f;
        public new TeamComponent teamComponent;
        private readonly GameObject bodyPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/effects/impacteffects/CrocoDiseaseImpactEffect");
        public override void OnEnter()
        {
            base.OnEnter();
            if(base.isAuthority)
            {
                this.teamComponent = base.teamComponent;
                base.PlayAnimation("Gesture, Mouth", "FireSpit", "FireSpit.playbackRate", 1f);
                foreach (HurtBox hurtBox in new SphereSearch
                {
                    origin = base.characterBody.footPosition,
                    radius = this.maxTrackingDistance,
                    mask = LayerIndex.entityPrecise.mask
                }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(this.teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
                {
                    CharacterBody body = hurtBox.healthComponent.body;
                    HealthComponent component = hurtBox.healthComponent;
                    if(body.HasBuff(RoR2Content.Buffs.Poisoned))
                    {
                        new BlastAttack
                        {
                            position = body.corePosition,
                            baseDamage = component.fullHealth * 0.1f + base.damageStat * 2f,
                            baseForce = 0,
                            radius = 16f,
                            attacker = base.gameObject,
                            inflictor = base.gameObject,
                            teamIndex = base.teamComponent.teamIndex,
                            crit = base.RollCrit(),
                            procChainMask = default,
                            procCoefficient = 1,
                            bonusForce = new Vector3(0, 0, 0),
                            falloffModel = BlastAttack.FalloffModel.None,
                            damageColorIndex = DamageColorIndex.Default,
                            damageType = DamageType.Generic,
                            attackerFiltering = AttackerFiltering.Default
                        }.Fire();
                        EffectManager.SimpleSoundEffect(BaseLeap.landingSound.index, body.footPosition, true);
                        EffectData bodyEffectData = new EffectData
                        {
                            origin = body.corePosition,
                            color = Color.green,
                            scale = 16
                        };
                        EffectManager.SpawnEffect(bodyPrefab, bodyEffectData, true);
                    }
                    else if(body.HasBuff(RoR2Content.Buffs.Blight))
                    {
                        new BlastAttack
                        {
                            position = body.corePosition,
                            baseDamage = base.damageStat * (3f * body.GetBuffCount(RoR2Content.Buffs.Blight)),
                            baseForce = 0,
                            radius = 16f,
                            attacker = base.gameObject,
                            inflictor = base.gameObject,
                            teamIndex = base.teamComponent.teamIndex,
                            crit = base.RollCrit(),
                            procChainMask = default,
                            procCoefficient = 1,
                            bonusForce = new Vector3(0, 0, 0),
                            falloffModel = BlastAttack.FalloffModel.None,
                            damageColorIndex = DamageColorIndex.Default,
                            damageType = DamageType.Generic,
                            attackerFiltering = AttackerFiltering.Default
                        }.Fire();
                        EffectManager.SimpleSoundEffect(BaseLeap.landingSound.index, body.footPosition, true);
                        EffectData bodyEffectData = new EffectData
                        {
                            origin = body.corePosition,
                            color = Color.yellow,
                            scale = 16
                        };
                        EffectManager.SpawnEffect(bodyPrefab, bodyEffectData, true);
                    }
                }
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.fixedAge >= 0.1f)
            {
                this.outer.SetNextStateToMain();                
            };
        }
    }
}
