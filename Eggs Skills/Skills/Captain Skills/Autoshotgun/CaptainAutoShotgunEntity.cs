using EggsSkills.SkillDefs;
using EntityStates;
using UnityEngine.AddressableAssets;
using UnityEngine;
using EggsSkills.Config;
using EggsUtils.Helpers;
using RoR2;
using EntityStates.Captain.Weapon;

namespace EggsSkills.EntityStates
{
    class CaptainAutoShotgunEntity : BaseSkillState, CaptainAutoshotgunSkilldef.IRampSetter
    {
        //Skills++ modifiers
        public static float spp_procMod = 0f;
        public static uint spp_bulletMod = 0;

        //Cast time pre-attack speed
        private static readonly float baseDuration = 1f;
        //Force
        private static readonly float baseForce = 10f;
        //Recoil
        private static readonly float baseRecoil = 0.05f;
        //Damage coefficient
        private static readonly float damageCoefficient = 0.9f;
        //Cast time post-attack speed
        private float duration;
        //Max firing range
        private static readonly float maxDist = 200f;
        //Proc coefficient
        private static readonly float procCoefficient = 0.7f + spp_procMod;

        //For calculating attack speed multiplier based on ramp up
        private static readonly float minMultiplier = 1f;
        private static readonly float maxMultiplier = 2f;
        private float multiplier;

        //muzzle
        private static readonly string muzzleName = ChargeCaptainShotgun.muzzleName;

        //Hit fx
        private GameObject hitEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Captain/HitsparkCaptainShotgun.prefab").WaitForCompletion();
        //Muzzle fx
        private GameObject muzzleEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/Muzzleflash1.prefab").WaitForCompletion();
        //Tracer fx
        private GameObject tracerEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ClayBruiser/TracerClayBruiserMinigun.prefab").WaitForCompletion();

        //Bullet count
        private uint bulletCount = Configuration.GetConfigValue(Configuration.CaptainAutoshotgunPellets) + spp_bulletMod;

        void CaptainAutoshotgunSkilldef.IRampSetter.SetRamp(float ramp)
        {
            multiplier = Math.ConvertToRange(0f, 1f, minMultiplier, maxMultiplier, ramp);
        }

        public override void OnEnter()
        {
            //Standard enter procedure
            base.OnEnter();
            //Determine the cast-time
            duration = baseDuration / (base.attackSpeedStat * multiplier);
            base.PlayCrossfade("Gesture, Override", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", duration, 0.1f);
            base.PlayCrossfade("Gesture, Additive", "ChargeCaptainShotgun", "ChargeCaptainShotgun.playbackRate", duration, 0.1f);
            //Fire bullets
            BulletFire();
        }

        //Bullet firing
        private void BulletFire()
        {
            //Get aimray
            Ray aimRay = base.GetAimRay();
            //Set aim mode
            base.StartAimMode(aimRay, duration * 2f, false);
            //Network check
            if (base.isAuthority)
            {
                //Establish and fire bullet
                new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0.4f * multiplier,
                    maxSpread = 1.6f * multiplier,
                    bulletCount = bulletCount,
                    procCoefficient = procCoefficient,
                    damage = base.characterBody.damage * damageCoefficient,
                    force = baseForce,
                    muzzleName = muzzleName,
                    falloffModel = default,
                    tracerEffectPrefab = tracerEffectPrefab,
                    hitEffectPrefab = hitEffectPrefab,
                    isCrit = base.RollCrit(),
                    HitEffectNormal = false,
                    smartCollision = true,
                    maxDistance = maxDist,
                    damageType = DamageTypeCombo.GenericPrimary,
                }.Fire();
            }
            //Execute muzzle flash
            EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, base.gameObject, muzzleName, false);
            //Play the sound
            Util.PlaySound("Play_captain_m1_shootWide", base.gameObject);
            //Apply recoil, shifted towards side the bullet shot from
            base.AddRecoil(-baseRecoil * multiplier, baseRecoil * multiplier, -baseRecoil * multiplier, baseRecoil * multiplier);
            base.characterBody.AddSpreadBloom(multiplier);
        }
        public override void FixedUpdate()
        {
            //Standard update procedure
            base.FixedUpdate();
            //If cast time over, and network check
            if (base.fixedAge >= duration && base.isAuthority)
            {
                if(!base.IsKeyDownAuthority())
                {
                    base.PlayAnimation("Gesture, Additive", "FireCaptainShotgun");
                    base.PlayAnimation("Gesture, Override", "FireCaptainShotgun");
                }
                outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //Interrupted by anything
            return InterruptPriority.Skill;
        }
    }
}
