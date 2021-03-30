using RoR2;
using EntityStates;
using UnityEngine;
using System.Linq;
using EntityStates.Treebot.Weapon;

namespace EggsSkills.EntityStates
{
    class DirectiveRoot : BaseSkillState
    {
        public bool isFirstPress;
        public float pullTimer;
        public bool isCrit;
        private GameObject bodyPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/TreebotPounderExplosion");
        public override void OnEnter()
        {
            if (base.isAuthority)
            {
                base.OnEnter();
                base.characterMotor.walkSpeedPenaltyCoefficient = 0.6f;
                base.characterBody.AddBuff(RoR2Content.Buffs.ArmorBoost);
                this.isFirstPress = true;
                this.pullTimer = 0.5f;
            }
        }
        public override void OnExit()
        {
            base.characterBody.RemoveBuff(RoR2Content.Buffs.ArmorBoost);
            base.characterMotor.walkSpeedPenaltyCoefficient = 1;
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            if (base.isAuthority && !base.IsKeyDownAuthority() && isFirstPress)
            {
                this.isFirstPress = false;
            }
            if (base.isAuthority && base.IsKeyDownAuthority() && !isFirstPress)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            else if(base.isAuthority && !base.characterMotor.isGrounded)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            else if(base.isAuthority && base.fixedAge >= 8)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            if (this.pullTimer > 0)
            {
                this.pullTimer -= Time.fixedDeltaTime;
            }
            else
            {
                this.pullTimer = 0.5f;
                this.Pull();
            }
            base.FixedUpdate();
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
        public void Pull()
        {
            isCrit = base.RollCrit();
            foreach (HurtBox hurtBox in new SphereSearch
            {
                origin = base.characterBody.corePosition,
                radius = 30,
                mask = LayerIndex.entityPrecise.mask
            }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
            {
                //force calc
                CharacterBody body = hurtBox.healthComponent.body;
                Vector3 a = hurtBox.transform.position - base.characterBody.corePosition;
                float magnitude = a.magnitude;
                Vector3 direction = a.normalized;
                float mass = body.GetComponent<Rigidbody>().mass;
                float massEval;
                if (!body.isFlying)
                {
                    massEval = mass * -20f - 400f;
                }
                else
                {
                    massEval = (mass * -20f - 400f) / 2;
                }
                var maxMass = new float[] { massEval, -6000 };
                Vector3 appliedForce = maxMass.Max() * direction * ((magnitude + 15) / 60);
                //damage
                DamageInfo damageInfo = new DamageInfo
                {
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    crit = isCrit,
                    damage = base.damageStat * 1.25f,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Stun1s,
                    force = appliedForce,
                    procCoefficient = 0.4f,
                    procChainMask = default(ProcChainMask),
                    position = hurtBox.transform.position
                };
                hurtBox.healthComponent.TakeDamage(damageInfo);
                GlobalEventManager.instance.OnHitEnemy(damageInfo,body.gameObject);
                base.healthComponent.AddBarrier(base.healthComponent.fullCombinedHealth * .02f);
                
            }
                base.PlayAnimation("Gesture", "LightImpact");
                EffectData bodyEffectData = new EffectData
                {
                    origin = base.characterBody.footPosition,
                    color = Color.green,
                    scale = 30
                };
                EffectManager.SpawnEffect(bodyPrefab, bodyEffectData, true);
                Util.PlaySound(FireMortar.fireSoundString, base.gameObject);

        }
    }
}
