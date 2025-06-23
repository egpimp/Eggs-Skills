using RoR2;
using EntityStates;
using UnityEngine;
using System.Linq;
using EntityStates.Treebot.Weapon;
using EggsUtils.Buffs;
using EggsSkills.Config;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;

namespace EggsSkills.EntityStates
{
    class DirectiveRootUpgraded : BaseSkillState
    {
        //Skills++
        internal static float spp_radiusBonus = 0f;
        internal static float spp_healMult = 1f;

        //Is the attack speed capped
        private readonly bool cappedAttackspeed = Configuration.GetConfigValue(Configuration.TreebotPullSpeedcap);
        //Did I crit
        private bool isCrit;
        //Handles first press of the skill
        private bool isFirstPress;

        //What % barrier per enemy
        private readonly float barrierCoefficient = 0.03f * spp_healMult;
        //How long between pulls normally
        private readonly float basePullTimer = 1f;
        //Standard radius of the skill
        private readonly float baseRadius = Configuration.GetConfigValue(Configuration.TreebotPullRange) + spp_radiusBonus;
        //Standard damage coefficient of the skill
        private readonly float damageCoefficient = 2.5f;
        //Damage coef per pulse hit
        private readonly float damageCoefficientPerHit = 0.75f;
        //Hitcount for redetonate
        private float hitCount;
        //Max attackspeed if enabled
        private readonly float maxAttackSpeedMod = 4f;
        //Max duration of skill
        private readonly float maxDuration = 8f;
        //Proc coef per pulse
        private readonly float procCoef = 0.7f;
        //Modifier of the pulse speed
        private float pullTimerModifier;
        //Pulse speed handler
        private float pullTimer;
        //Speed u r set to during
        private readonly float speedFraction = 0.8f;

        //Skill fx
        private GameObject bodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Junk/Treebot/TreebotPounderExplosion.prefab").WaitForCompletion();
        private GameObject body2Prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Treebot/OmniExplosionVFXTreebot.prefab").WaitForCompletion();

        public override void OnEnter()
        {
            base.OnEnter();
            //Min between attack speed and max amount allowed to be applied to pulse frequency
            float[] getMin = new float[] {maxAttackSpeedMod, attackSpeedStat};
            //Get the attack speed modifier based on attack speed and whether it's capped or not
            pullTimerModifier = cappedAttackspeed ? getMin.Min() : attackSpeedStat;
            //Set speed to the speed fraction
            characterMotor.walkSpeedPenaltyCoefficient = speedFraction;
            //Give buff on server only
            if(NetworkServer.active) characterBody.AddBuff(BuffsLoading.buffDefAdaptive);
            //This is the first press of the button
            isFirstPress = true;
            //Set pull timer based the modifier
            pullTimer = basePullTimer / pullTimerModifier;
            //Start at 0 hits
            hitCount = 0f;
        }
        public override void OnExit()
        {
            //Remove the buff server only
            if(NetworkServer.active) characterBody.RemoveBuff(BuffsLoading.buffDefAdaptive);
            //Fix speed
            characterMotor.walkSpeedPenaltyCoefficient = 1f;
            //Deal end-pulse
            if (hitCount > 0f)
            {
                if (base.isAuthority)
                {
                    //Setup fx data
                    EffectData bodyEffectData = new EffectData
                    {
                        origin = characterBody.footPosition,
                        color = Color.magenta,
                        scale = baseRadius
                    };
                    EffectManager.SpawnEffect(body2Prefab, bodyEffectData, true);
                    new BlastAttack()
                    {
                        attacker = gameObject,
                        inflictor = gameObject,
                        falloffModel = BlastAttack.FalloffModel.None,
                        position = transform.position,
                        radius = baseRadius,
                        baseDamage = damageCoefficientPerHit * hitCount * base.damageStat,
                        damageType = DamageType.AOE | DamageTypeCombo.GenericSpecial,
                        crit = base.RollCrit(),
                        procCoefficient = procCoef,
                        baseForce = 1000f,
                        teamIndex = teamComponent.teamIndex
                    }.Fire();
                }
                //Play animation
                PlayAnimation("Gesture", "LightImpact");
                //Play sound
                Util.PlaySound(CreatePounder.fireSoundString, gameObject);
            }
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //If key is no longer down and first press is still active, disable the first press flag
            if (!IsKeyDownAuthority() && isFirstPress) isFirstPress = false;

            //All exit cases here, required network checks
            //Exit if key is pushed and it is not the first press
            if (isAuthority && IsKeyDownAuthority() && !isFirstPress)
            {
                outer.SetNextStateToMain();
                return;
            }
            //Exit if the character is not grounded anymore
            else if(isAuthority && !characterMotor.isGrounded)
            {
                outer.SetNextStateToMain();
                return;
            }
            //And exit if the duration is up
            else if(isAuthority && fixedAge >= maxDuration)
            {
                outer.SetNextStateToMain();
                return;
            }

            //If timer is over 0, count down
            if (pullTimer > 0) pullTimer -= Time.fixedDeltaTime;
            //Otherwise
            else
            {
                //Reset timer and execute the pull effect
                pullTimer = basePullTimer / pullTimerModifier;
                Pull();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        { 
            //Interruptible by any other skill
            return InterruptPriority.Skill;
        }

        public void Pull()
        {
            //For whether anything was hit
            bool hit = false;
            //Check if this pulse crits
            isCrit = RollCrit();
            //Spheresearch
            foreach (HurtBox hurtBox in new SphereSearch
            {
                origin = characterBody.corePosition,
                radius = baseRadius,
                mask = LayerIndex.entityPrecise.mask
            }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
            {
            //force calc
                //Get body of the enemy
                CharacterBody body = hurtBox.healthComponent.body;
                //Calc for finding vector (Pos2 - Pos1)
                Vector3 a = hurtBox.transform.position - base.characterBody.corePosition;
                //Determine distance (Magnitude gives raw dist)
                float magnitude = a.magnitude;
                //Grab direction (Normalized gives raw dir)
                Vector3 direction = a.normalized;
                //Get mass of enemy (Used to determine pull strength)
                float mass = body.GetComponent<Rigidbody>().mass;
                //The base force
                float baseForce = mass * -20f - 400f;
                //Half force for flying enemies
                if (body.isFlying) baseForce /= 2;
                //Cap the force to 6000 (Note its pulling backwards, so we use negative force)
                float maxBaseForce = new float[] { baseForce, -6000 }.Max();
                //This gives us a factor of the distance compared to the given radius of the skill
                float distFactor = (magnitude + 15) / (baseRadius * 2);
                //Apply force, multiplied by distance factor, in given direction
                Vector3 appliedForce = maxBaseForce * direction * distFactor;

            //damage
                //Deal damage / barrier on server only
                if (NetworkServer.active)
                {
                    //Prep damage
                    DamageInfo damageInfo = new DamageInfo
                    {
                        attacker = gameObject,
                        inflictor = gameObject,
                        crit = isCrit,
                        damage = damageStat * damageCoefficient,
                        damageType = DamageType.Stun1s,
                        force = appliedForce,
                        procCoefficient = procCoef,
                        position = hurtBox.transform.position
                    };
                    //Apply damage
                    hurtBox.healthComponent.TakeDamage(damageInfo);
                    //Apply onhits
                    GlobalEventManager.instance.OnHitEnemy(damageInfo, body.gameObject);
                    GlobalEventManager.instance.OnHitAll(damageInfo, body.gameObject);
                    //Give barrier to player as % of hp
                    healthComponent.AddBarrier(healthComponent.fullCombinedHealth * barrierCoefficient);
                    hit = true;
                }
            }
            //Play animation
            PlayAnimation("Gesture", "LightImpact");
            //Setup fx data
            EffectData bodyEffectData = new EffectData
            {
                origin = characterBody.footPosition,
                color = Color.green,
                scale = baseRadius
            };
            //Play the fx on client only
            if(isAuthority) EffectManager.SpawnEffect(bodyPrefab, bodyEffectData, true);
            //Play sound
            Util.PlaySound(FireMortar.fireSoundString, gameObject);
            //Increment hitcount if hit
            if (hit) hitCount++;
        }
    }
}
