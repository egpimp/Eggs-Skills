using RoR2;
using EntityStates;
using UnityEngine;
using System.Linq;
using EntityStates.Treebot.Weapon;
using EggsUtils.Buffs;
using EggsSkills.Config;
using UnityEngine.Networking;

namespace EggsSkills.EntityStates
{
    class DirectiveRoot : BaseSkillState
    {
        //Is the attack speed capped
        private readonly bool cappedAttackspeed = Configuration.GetConfigValue<bool>(Configuration.TreebotPullSpeedcap);
        //Did I crit
        private bool isCrit;
        //Handles first press of the skill
        private bool isFirstPress;

        //What % barrier per enemy
        private readonly float barrierCoefficient = 0.03f;
        //How long between pulls normally
        private readonly float basePullTimer = 1f;
        //Standard radius of the skill
        private readonly float baseRadius = Configuration.GetConfigValue(Configuration.TreebotPullRange);
        //Standard damage coefficient of the skill
        private readonly float damageCoefficient = 2.5f;
        //Max attackspeed if enabled
        private readonly float maxAttackSpeedMod = 4f;
        //Max duration of skill
        private readonly float maxDuration = 8f;
        //Modifier of the pulse speed
        private float pullTimerModifier;
        //Pulse speed handler
        private float pullTimer;
        //Speed u r set to during
        private readonly float speedFraction = 0.7f;

        //Skill fx
        private GameObject bodyPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/TreebotPounderExplosion");
        
        public override void OnEnter()
        {
            base.OnEnter();
            //Min between attack speed and max amount allowed to be applied to pulse frequency
            float[] getMin = new float[] {this.maxAttackSpeedMod, base.attackSpeedStat};
            //Get the attack speed modifier based on attack speed and whether it's capped or not
            this.pullTimerModifier = this.cappedAttackspeed ? getMin.Min() : base.attackSpeedStat;
            //Set speed to the speed fraction
            base.characterMotor.walkSpeedPenaltyCoefficient = this.speedFraction;
            //Check network and give buff
            if(NetworkServer.active) base.characterBody.AddBuff(BuffsLoading.buffDefAdaptive);
            //This is the first press of the button
            this.isFirstPress = true;
            //Set pull timer based the modifier
            this.pullTimer = this.basePullTimer / this.pullTimerModifier;
        }
        public override void OnExit()
        {
            //Remove the buff
            if(NetworkServer.active) base.characterBody.RemoveBuff(BuffsLoading.buffDefAdaptive);
            //Fix speed
            base.characterMotor.walkSpeedPenaltyCoefficient = 1f;
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //If key is no longer down and first press is still active, disable the first press flag
            if (!base.IsKeyDownAuthority() && this.isFirstPress) this.isFirstPress = false;

            //All exit cases here, required network checks
            //Exit if key is pushed and it is not the first press
            if (base.isAuthority && base.IsKeyDownAuthority() && !this.isFirstPress)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            //Exit if the character is not grounded anymore
            else if(base.isAuthority && !base.characterMotor.isGrounded)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            //And exit if the duration is up
            else if(base.isAuthority && base.fixedAge >= this.maxDuration)
            {
                this.outer.SetNextStateToMain();
                return;
            }

            //If timer is over 0, count down
            if (this.pullTimer > 0) this.pullTimer -= Time.fixedDeltaTime;
            //Otherwise
            else
            {
                //Reset timer and execute the pull effect
                this.pullTimer = this.basePullTimer / this.pullTimerModifier;
                this.Pull();
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        { 
            //Interruptible by any
            return InterruptPriority.Skill;
        }
        public void Pull()
        {
            //Check if this pulse crits
            this.isCrit = base.RollCrit();
            //Spheresearch
            foreach (HurtBox hurtBox in new SphereSearch
            {
                origin = base.characterBody.corePosition,
                radius = this.baseRadius,
                mask = LayerIndex.entityPrecise.mask
            }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
            {
                //force calc
                //Get body of the enemy
                CharacterBody body = hurtBox.healthComponent.body;
                //Calc for finding direction
                Vector3 a = hurtBox.transform.position - base.characterBody.corePosition;
                //Determine distance
                float magnitude = a.magnitude;
                //Grab direction
                Vector3 direction = a.normalized;
                //Get mass of enemy
                float mass = body.GetComponent<Rigidbody>().mass;
                //Hold this
                float massEval;
                //If not flying, we can pull harder
                if (!body.isFlying) massEval = mass * -20f - 400f;
                //If flying, pull softer
                else massEval = (mass * -20f - 400f) / 2;
                //Cap the mass evaluation so big creatures don't fly
                float[] maxMass = new float[] { massEval, -6000 };
                //Determine applied force with mass, direction, and magnitude scaling with distance
                Vector3 appliedForce = maxMass.Max() * direction * ((magnitude + 15) / (this.baseRadius * 2));

                //damage
                //Network check
                if (NetworkServer.active)
                {
                    //Prep damage
                    DamageInfo damageInfo = new DamageInfo
                    {
                        attacker = base.gameObject,
                        inflictor = base.gameObject,
                        crit = this.isCrit,
                        damage = base.damageStat * this.damageCoefficient,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Stun1s,
                        force = appliedForce,
                        procCoefficient = 0.4f,
                        procChainMask = default,
                        position = hurtBox.transform.position
                    };
                    //Apply damage
                    hurtBox.healthComponent.TakeDamage(damageInfo);
                    //Apply onhits
                    GlobalEventManager.instance.OnHitEnemy(damageInfo, body.gameObject);
                    //Give barrier to player
                    base.healthComponent.AddBarrier(base.healthComponent.fullCombinedHealth * this.barrierCoefficient);
                }
            }
            //Play animation
            base.PlayAnimation("Gesture", "LightImpact");
            //Setup fx data
            EffectData bodyEffectData = new EffectData
            {
                origin = base.characterBody.footPosition,
                color = Color.green,
                scale = this.baseRadius
            };
            //Play the fx
            if(base.isAuthority) EffectManager.SpawnEffect(bodyPrefab, bodyEffectData, true);
            //Play sound
            Util.PlaySound(FireMortar.fireSoundString, base.gameObject);
        }
    }
}
