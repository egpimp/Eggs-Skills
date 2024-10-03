using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace EggsSkills.EntityStates
{
    internal class InversionChargePure : InversionChargeBase
    {
        public override void OnEnter()
        {
            muzzleFlash = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorChargeCrushHealth.prefab").WaitForCompletion();
            nextState = new InversionPure();
            base.OnEnter();
        }
    }
}
