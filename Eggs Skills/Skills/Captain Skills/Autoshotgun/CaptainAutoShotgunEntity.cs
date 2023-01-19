using EggsSkills.SkillDefs;
using EntityStates;
using UnityEngine.AddressableAssets;
using UnityEngine;
using EggsSkills.Config;
using EggsUtils.Helpers;

namespace EggsSkills.EntityStates
{
    class CaptainAutoShotgunEntity : BaseState, CaptainAutoshotgunSkilldef.IRampSetter
    {
        //Skills++ modifiers
        public static float spp_procMod = 0f;
        public static uint spp_bulletMod = 0;

        //Cast time pre-attack speed
        private static readonly float baseDuration = 0.8f;
        //Force
        private static readonly float baseForce = 10f;
        //Damage coefficient
        private static readonly float damageCoefficient = 1f;
        //Cast time post-attack speed
        private float duration;
        //Max firing range
        private static readonly float maxDist = 200f;
        //Proc coefficient
        private static readonly float procCoefficient = 0.6f + spp_procMod;

        //For calculating attack speed multiplier based on ramp up
        private static readonly float minMultiplier = 1f;
        private static readonly float maxMultiplier = 2.5f;
        private float multiplier;

        //Hit fx
        private GameObject hitEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/Hitspark1.prefab").WaitForCompletion();
        //Muzzle fx
        private GameObject muzzleEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Common/VFX/Muzzleflash1.prefab").WaitForCompletion();
        //Tracer fx
        private GameObject tracerEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/TracerCommandoDefault.prefab").WaitForCompletion();

        //Bullet count
        private uint bulletCount = Configuration.GetConfigValue(Configuration.CaptainAutoshotgunPellets) + spp_bulletMod;

        void CaptainAutoshotgunSkilldef.IRampSetter.SetRamp(float ramp)
        {
            multiplier = Math.ConvertToRange(0f, 1f, minMultiplier, maxMultiplier, ramp);
        }


    }
}
