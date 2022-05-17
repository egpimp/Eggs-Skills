using RoR2;
using EggsSkills.Config;
using RoR2.Achievements;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, null)]
    internal class TeslaMineAchievement : BaseAchievement
    {
        internal const string ACHNAME = "EngiMultipleElectricItems";
        internal const string REWARDNAME = "EggsSkills.TeslaMine";


        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.engineerRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            On.RoR2.CharacterMaster.OnInventoryChanged += ClearCheck;
            if (Configuration.UnlockAll.Value)
            {
                base.Grant();
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            On.RoR2.CharacterMaster.OnInventoryChanged -= ClearCheck;
        }

        public void ClearCheck(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            orig(self);
            if (base.isUserAlive && base.meetsBodyRequirement)
            {
                if (self.netId != null && self.netId == base.localUser.cachedMasterController.master.netId)
                {
                    int itemCount = 0;

                    Inventory inventory = self.inventory;

                    if (inventory.GetItemCount(RoR2Content.Items.ChainLightning) >= 1 || inventory.GetItemCount(DLC1Content.Items.ChainLightningVoid) >= 1) itemCount += 1;

                    if (inventory.GetItemCount(RoR2Content.Items.ShockNearby) >= 1) itemCount += 1;

                    if (inventory.GetItemCount(RoR2Content.Items.NovaOnLowHealth) >= 1) itemCount += 1;

                    if (inventory.GetItemCount(RoR2Content.Items.LightningStrikeOnHit) >= 1) itemCount += 1;

                    if (inventory.GetEquipmentIndex() == RoR2Content.Equipment.Lightning.equipmentIndex || inventory.GetEquipmentIndex() == RoR2Content.Equipment.AffixBlue.equipmentIndex || inventory.GetEquipmentIndex() == RoR2Content.Equipment.BFG.equipmentIndex || inventory.GetEquipmentIndex() == RoR2Content.Equipment.QuestVolatileBattery.equipmentIndex) itemCount += 1;
                    
                    if (itemCount >= 4)
                    {
                        base.Grant();
                    }
                }
            }
        }
    }
}
