using EggsSkills.Config;
using EntityStates;
using EntityStates.Croco;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using EggsSkills.ModCompats;

namespace EggsSkills.EntityStates
{
    class AcridPurgeEntity : BaseState
    {
        //Skills++ modifiers
        public static float spp_damageMult = 1f;
        public static float spp_radiusMult = 1f;

        //Damage coefficient for blight effect
        private readonly float blightDamageCoefficient = 3f;
        //Deep root damage coefficient
        private readonly float drDamageCoefficient = 5f;
        //Detonation radius
        private readonly float detonationRadius = Configuration.GetConfigValue(Configuration.CrocoPurgeBaseradius) * spp_radiusMult;
        //Health fraction for poison effect
        private readonly float healthFraction = 0.1f;
        //Max distance for finding poisoned targets
        private readonly float maxTrackingDistance = 5000f;
        //Damage coefficient for poison effect
        private readonly float poisonDamageCoefficient = 2f;
        //Overall proc coefficient
        private readonly float procCoefficient = 1f;

        private static readonly string soundString = "Play_gup_attack1_shoot";

        //Effect to be played on use
        private GameObject bodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Croco/CrocoDiseaseImpactEffect.prefab").WaitForCompletion();

        public override void OnEnter()
        {
            //Do normal onenter things first
            base.OnEnter();
            //Play the animation
            base.PlayAnimation("Gesture, Mouth", "FireSpit", "FireSpit.playbackRate", 1f);
            //Spheresearch for enemies in range, starting from doggo foots
            foreach (HurtBox hurtBox in new SphereSearch
            {
                origin = base.characterBody.footPosition,
                radius = maxTrackingDistance,
                mask = LayerIndex.entityPrecise.mask
            }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
            {
                //Get the characterbody cause we need buffcount
                CharacterBody body = hurtBox.healthComponent.body;
                //Get health component for poison calc
                HealthComponent component = hurtBox.healthComponent;
                //If they have the poison debuff...
                if(body.HasBuff(RoR2Content.Buffs.Poisoned))
                {
                    //Network check
                    if (base.isAuthority)
                    {
                        //Make blast attack and fire it at pos of all enemies
                        new BlastAttack
                        {
                            position = body.corePosition,
                            baseDamage = component.fullHealth * healthFraction + base.damageStat * poisonDamageCoefficient * spp_damageMult,
                            baseForce = 0f,
                            radius = detonationRadius,
                            attacker = base.gameObject,
                            inflictor = base.gameObject,
                            teamIndex = base.teamComponent.teamIndex,
                            crit = base.RollCrit(),
                            procCoefficient = procCoefficient,
                            falloffModel = BlastAttack.FalloffModel.None,
                        }.Fire();
                    }
                    //Play sfx at enemies
                    Util.PlaySound(soundString, body.gameObject);
                    //Vfx data
                    EffectData bodyEffectData = new EffectData
                    {
                        origin = body.corePosition,
                        color = Color.green,
                        scale = detonationRadius
                    };
                    //Play vfx at enemies
                    EffectManager.SpawnEffect(bodyPrefab, bodyEffectData, true);
                }
                //If blighted...
                if(body.HasBuff(RoR2Content.Buffs.Blight))
                {
                    //Network check
                    if (base.isAuthority)
                    {
                        //Make blast attack and fire it also at pos of all enemies affected
                        new BlastAttack
                        {
                            position = body.corePosition,
                            baseDamage = base.damageStat * (blightDamageCoefficient * body.GetBuffCount(RoR2Content.Buffs.Blight)) * spp_damageMult,
                            baseForce = 0f,
                            radius = detonationRadius,
                            attacker = base.gameObject,
                            inflictor = base.gameObject,
                            teamIndex = base.teamComponent.teamIndex,
                            crit = base.RollCrit(),
                            procCoefficient = procCoefficient,
                            falloffModel = BlastAttack.FalloffModel.None,
                        }.Fire();
                    }
                    //Play sfx at enemy pos
                    Util.PlaySound(soundString, body.gameObject);
                    //Setup vfx data
                    EffectData bodyEffectData = new EffectData
                    {
                        origin = body.corePosition,
                        color = Color.yellow,
                        scale = detonationRadius
                    };
                    //Play vfx data at enemy positions
                    EffectManager.SpawnEffect(bodyPrefab, bodyEffectData, true);
                }
                //Third case for other mod passive
                if (DeeprotCompat.CheckHasDeeprot(body) || DeeprotCompat.CheckHasSoulrot(body))
                {
                    if(base.isAuthority)
                    {
                        //Make blast attack and fire it at pos of all enemies
                        new BlastAttack
                        {
                            position = body.corePosition,
                            baseDamage = base.damageStat * drDamageCoefficient * spp_damageMult,
                            baseForce = 0f,
                            radius = detonationRadius,
                            attacker = base.gameObject,
                            inflictor = base.gameObject,
                            teamIndex = base.teamComponent.teamIndex,
                            crit = base.RollCrit(),
                            procCoefficient = procCoefficient,
                            falloffModel = BlastAttack.FalloffModel.None,
                            damageType = DamageType.Stun1s
                        }.Fire();
                    }
                    //Play sfx at enemy pos
                    Util.PlaySound(soundString, body.gameObject);
                    //Setup vfx data
                    EffectData bodyEffectData = new EffectData
                    {
                        origin = body.corePosition,
                        color = Color.magenta,
                        scale = detonationRadius
                    };
                    //Play vfx data at enemy positions
                    EffectManager.SpawnEffect(bodyPrefab, bodyEffectData, true);
                }
            }
        }
        public override void FixedUpdate()
        {
            //Run base fixedupdate stuff
            base.FixedUpdate();
            //If the skill has been going on for 1/10 seconds and network check....
            if(base.fixedAge >= 0.1f && base.isAuthority)
            {
                //Set state back to main state
                outer.SetNextStateToMain();
                //Return
                return;
            };
        }
    }
}
