using RoR2;
using EntityStates;
using UnityEngine;
using System.Linq;
using EggsUtils.Buffs;
using EggsSkills.Config;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using EntityStates.VoidSurvivor.Weapon;
using HG;

namespace EggsSkills.EntityStates
{
    class InversionBase : BaseSkillState
    {
        //Skills++
        internal static float spp_radiusBonus = 0f;
        internal static float spp_damageMult = 1f;

        //Standard radius of the skill
        private static readonly float baseRadius = Configuration.GetConfigValue(Configuration.VoidfiendInversionRadius) + spp_radiusBonus;
        //Standard damage coefficient of the skill
        protected float damageCoefficient = 10f;
        //Max duration of skill
        private static readonly float baseDuration = 0.25f;
        private float duration;
        //Proc coef
        private static readonly float procCoef = 1f;
        private static readonly float healFraction = 0.25f;
        protected float corruptionChange;
        private static readonly float baseForce = 3800f;
        protected float forceMultiplier;

        //Sound string
        private static readonly string soundString = "Play_voidman_m2_explode";

        //Skill fx
        protected GameObject bodyPrefab;
        //Muzzle flash
        protected GameObject muzzleFlash;

        private string animation = "CrushCorruption";
        private string playbackRate = "CrushCorruption.playbackRate";

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / base.attackSpeedStat;
            base.StartAimMode(duration + 2f, false);
            base.PlayAnimation("LeftArm, Override", animation, playbackRate, duration);
            Util.PlaySound(soundString, base.gameObject);
            base.AddRecoil(-1f, 1f, -0.25f, 0.25f);
            base.characterBody.AddSpreadBloom(2f);
            if (muzzleFlash) EffectManager.SimpleMuzzleFlash(muzzleFlash, base.gameObject, "MuzzleHandBeam", false);
            if (bodyPrefab)
            {
                EffectData endEffectData = new EffectData
                {
                    scale = baseRadius,
                    origin = characterBody.corePosition
                };
                //Spawn the fx
                EffectManager.SpawnEffect(bodyPrefab, endEffectData, true);
            }
            if (NetworkServer.active)
            {
                ProcChainMask mask = default;
                mask.AddProc(ProcType.VoidSurvivorCrush);
                if (damageCoefficient < 5f) base.healthComponent.HealFraction(healFraction, mask);
                else
                {
                    DamageInfo selfDamageInfo = new DamageInfo();
                    selfDamageInfo.damage = base.healthComponent.fullCombinedHealth * healFraction;
                    selfDamageInfo.position = base.characterBody.corePosition;
                    selfDamageInfo.force = Vector3.zero;
                    selfDamageInfo.damageColorIndex = DamageColorIndex.Default;
                    selfDamageInfo.crit = false;
                    selfDamageInfo.attacker = null;
                    selfDamageInfo.inflictor = null;
                    selfDamageInfo.damageType = (DamageType.NonLethal | DamageType.BypassArmor);
                    selfDamageInfo.procCoefficient = 0f;
                    selfDamageInfo.procChainMask = mask;
                    base.healthComponent.TakeDamage(selfDamageInfo);
                }
                VoidSurvivorController controller = base.GetComponent<VoidSurvivorController>();
                if (controller) controller.AddCorruption(corruptionChange);

                BlastAttack attack = new BlastAttack();
                attack.baseDamage = base.damageStat * damageCoefficient * spp_damageMult;
                attack.position = base.characterBody.corePosition;
                attack.crit = base.RollCrit();
                attack.radius = baseRadius;
                attack.baseForce = baseForce * forceMultiplier;
                attack.inflictor = base.gameObject;
                attack.attacker = base.gameObject;
                attack.damageType = DamageType.AOE;
                attack.falloffModel = BlastAttack.FalloffModel.Linear;
                attack.losType = BlastAttack.LoSType.None;
                attack.procCoefficient = procCoef;
                attack.teamIndex = base.characterBody.teamComponent.teamIndex;
                attack.Fire();
            };
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(isAuthority && fixedAge >= duration)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
