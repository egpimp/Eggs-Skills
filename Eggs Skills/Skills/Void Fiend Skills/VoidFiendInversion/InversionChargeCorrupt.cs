using UnityEngine.AddressableAssets;
using UnityEngine;

namespace EggsSkills.EntityStates
{
    internal class InversionChargeCorrupt : InversionChargeBase
    {
        public override void OnEnter()
        {
            muzzleFlash = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorChargeCrushCorruption.prefab").WaitForCompletion();
            nextState = new InversionCorrupt();
            base.OnEnter();
        }
    }
}
