using EggsSkills.Config;
using EggsSkills.ModCompats;
using EntityStates;
using RoR2;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace EggsSkills.EntityStates
{ 
    internal class PoisonBreath : BaseSkillState
    {
        //Skills++
        internal static float spp_sizeMultiplier = 1f;
        internal static float spp_damageMult = 1f;

        private static readonly float baseDuration = 0.5f;
        private static readonly float baseDistance = 25f;
        private static readonly float baseAngle = Configuration.GetConfigValue(Configuration.CrocoPoisonbreathAngle);
        private static readonly float baseDamageCoefficient = 0.25f;
        private static readonly float procCoefficient = 0.7f;
        private float duration;

        private static readonly string soundString = "Play_minimushroom_spore_explode";

        //Effect to be played on use
        private GameObject bodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/MiniMushroom/SporeGrenadeRepeatHitImpact.prefab").WaitForCompletion();

        CrocoDamageTypeController controller;

        public override void OnEnter()
        {
            base.OnEnter();
            controller = base.GetComponent<CrocoDamageTypeController>();
            duration = baseDuration / base.attackSpeedStat;
            base.StartAimMode(duration * 2f);
            Ray ray = GetAimRay();
            base.PlayAnimation("Gesture, Mouth", "FireSpit", "FireSpit.playbackRate", duration + 0.1f);
            base.characterBody.AddSpreadBloom(0.5f);

            //Create fx
            PlayVFX(ray);
            //Get hit targets
            HurtBox[] hit = GetAllHit(ray);
            //Apply the damage and poison to all of those targets
            if (NetworkServer.active) ApplyDamage(hit);
            Util.PlaySound(soundString, base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.fixedAge > duration && base.isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        private HurtBox[] GetAllHit(Ray aimRay)
        {
            //Search a cone in front of the player
            BullseyeSearch search = new BullseyeSearch()
            {
                searchOrigin = aimRay.origin,
                searchDirection = aimRay.direction,
                maxAngleFilter = baseAngle,
                minAngleFilter = 0f,
                maxDistanceFilter = baseDistance * spp_sizeMultiplier,
                teamMaskFilter = TeamMask.GetUnprotectedTeams(teamComponent.teamIndex),
                filterByDistinctEntity = true,
                filterByLoS = true
            };
            search.RefreshCandidates();
            search.FilterOutGameObject(base.gameObject);
            return search.GetResults().Where(new Func<HurtBox, bool>(Util.IsValid)).Distinct(default(HurtBox.EntityEqualityComparer)).ToArray();
        }

        private void ApplyDamage(HurtBox[] targets)
        {
            foreach(HurtBox target in targets)
            {
                CharacterBody body = target.healthComponent.body;
                if (!body) continue;
                bool isPoisoned = body.HasBuff(RoR2Content.Buffs.Poisoned) || body.HasBuff(RoR2Content.Buffs.Blight) || DeeprotCompat.CheckHasDeeprot(body) || DeeprotCompat.CheckHasSoulrot(body);
                //Calc damage
                float damageCoefficient = isPoisoned ? baseDamageCoefficient * 2f : baseDamageCoefficient;
                //Establish damageinfo
                DamageInfo info = new DamageInfo()
                {
                    damage = base.damageStat * damageCoefficient,
                    force = Vector3.zero,
                    attacker = base.gameObject,
                    crit = base.RollCrit(),
                    damageType = controller.GetDamageType(),
                    inflictor = base.gameObject,
                    position = body.corePosition,
                    procCoefficient = procCoefficient,
                    
                };
                target.healthComponent.TakeDamage(info);
                GlobalEventManager.instance.OnHitEnemy(info, body.gameObject);
            }
        }

        private void PlayVFX(Ray aimRay)
        {
            EffectData bodyEffectData = new EffectData
            {
                color = Color.green,
                scale = 0.5f
            };

            for (int i = 0; i < Mathf.Ceil(baseDistance * spp_sizeMultiplier); i++)
            {
                float randDist = UnityEngine.Random.Range(0.5f, baseDistance * spp_sizeMultiplier);
                float legalRadius = randDist / Mathf.Tan(baseAngle / 2);
                float randRadius = UnityEngine.Random.Range(-legalRadius, legalRadius) / 3;
                Vector3 pos = aimRay.origin + aimRay.direction * randDist + UnityEngine.Random.insideUnitSphere * randRadius;
                bodyEffectData.origin = pos;
                EffectManager.SpawnEffect(bodyPrefab, bodyEffectData, true);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
