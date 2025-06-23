using RoR2;
using EntityStates;
using UnityEngine;
using EggsSkills.Config;
using UnityEngine.AddressableAssets;

namespace EggsSkills.EntityStates
{

    public class LancerRoundsEntity : BaseState
    {
        //Skills++ modifiers
        public static float spp_procMod = 0f;
        public static float spp_damageMult = 1f;

        //Cast time pre-attack speed
        private readonly float baseDuration = 0.6f;
        //Force
        private readonly float baseForce = 50f;
        //Damage Coefficient pre-bonus
        private readonly float baseDamageCoefficient = 3f;
        //Recoil
        private readonly float baseRecoil = 0.4f;
        //Damage Coefficient after bonus
        private float damageCoefficient;
        //Cast time with attack speed
        private float duration;
        //Max range
        private float maxDist = 1000f;
        //procCoef
        private readonly float procCoefficient = 1f;
        //recoil after damage
        private float recoil;
        //For the fun config option
        private bool wallPen = Configuration.GetConfigValue(Configuration.RailgunnerLanceWallPen);

        //Muzzle string
        private string muzzleName = "MuzzlePistol";

        //fx
        private GameObject tracer = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/TracerRailgunLight.prefab").WaitForCompletion();
        private GameObject muzzleFlash = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/MuzzleflashSmokeRing.prefab").WaitForCompletion();
        private GameObject hitspark = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/HitsparkBandit.prefab").WaitForCompletion();

        public override void OnEnter()
        {
            //Run OnEnter
            base.OnEnter();
            //Get altered attack speed stat
            float alteredAttackSpeedMod = ((base.attackSpeedStat - 1) / 2) + 1;
            //Find cast time
            duration = baseDuration / alteredAttackSpeedMod;
            //Find modified damage
            damageCoefficient = baseDamageCoefficient * alteredAttackSpeedMod;
            //Calc recoil
            recoil = baseRecoil * alteredAttackSpeedMod;
            //Fire
            Fire();
            base.PlayAnimation("Gesture, Override", "FirePistol", "FirePistol.playbackrate", duration);
            Util.PlaySound("Play_railgunner_m2_alt_fire", base.gameObject);
        }

        private void Fire()
        {
            //Aimray
            Ray aimRay = base.GetAimRay();
            //Aim mode
            base.StartAimMode(aimRay, duration * 1.5f, false);
            //Network check
            if (base.isAuthority)
            {
                //Setup and firing bulletattack
                new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    maxSpread = 0.05f,
                    bulletCount = 1u,
                    procCoefficient = procCoefficient + spp_procMod,
                    damage = base.characterBody.damage * damageCoefficient * spp_damageMult,
                    force = baseForce * damageCoefficient,
                    smartCollision = true,
                    maxDistance = maxDist,
                    muzzleName = muzzleName,
                    tracerEffectPrefab = tracer,
                    hitEffectPrefab = hitspark,
                    falloffModel = BulletAttack.FalloffModel.None,
                    stopperMask = wallPen ? LayerIndex.noCollision.mask : LayerIndex.world.mask,
                    radius = damageCoefficient / 5f,
                    damageType = DamageTypeCombo.GenericPrimary
                }.Fire();
            }
            EffectManager.SimpleMuzzleFlash(muzzleFlash, base.gameObject, muzzleName, true);
            base.AddRecoil(-recoil, recoil, -recoil, recoil);
            base.characterBody.AddSpreadBloom(0.5f + recoil);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(base.isAuthority & base.fixedAge > duration)
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