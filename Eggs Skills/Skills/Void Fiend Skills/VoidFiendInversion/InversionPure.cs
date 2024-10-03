using UnityEngine.AddressableAssets;
using UnityEngine;

namespace EggsSkills.EntityStates
{
    internal class InversionPure : InversionBase
    {
        public override void OnEnter()
        {
            bodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/ElementalRingVoid/ElementalRingVoidImplodeEffect.prefab").WaitForCompletion();
            muzzleFlash = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorCrushHealthMuzzleflash.prefab").WaitForCompletion();
            damageCoefficient = 10f;
            corruptionChange = 25f;
            forceMultiplier = -1f;
            base.OnEnter();
        }
    }
}
