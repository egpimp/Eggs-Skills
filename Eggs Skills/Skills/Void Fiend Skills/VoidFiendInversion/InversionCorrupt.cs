using UnityEngine.AddressableAssets;
using UnityEngine;

namespace EggsSkills.EntityStates
{
    internal class InversionCorrupt : InversionBase
    {
        public override void OnEnter()
        {
            bodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorMegaBlasterExplosion.prefab").WaitForCompletion();
            muzzleFlash = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorCrushCorruptionMuzzleflash.prefab").WaitForCompletion();
            damageCoefficient = 0f;
            corruptionChange = -25f;
            forceMultiplier = 1f;
            base.OnEnter();
        }
    }
}
