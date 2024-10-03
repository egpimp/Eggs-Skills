using RoR2;
using EggsSkills.Config;
using RoR2.Achievements;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    internal class TeslaMineAchievement : BaseAchievement
    {
        internal const string ACHNAME = "EngiMultipleElectricItems";
        internal const string REWARDNAME = "EggsSkills.TeslaMine";
        internal const uint TOKENS = 10;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.engineerRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            On.RoR2.CharacterMaster.OnInventoryChanged += ClearCheck;
            if (Configuration.UnlockAll.Value) base.Grant();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            On.RoR2.CharacterMaster.OnInventoryChanged -= ClearCheck;
        }

        public void ClearCheck(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
        {
            orig(self);
            //If they are dead or are not engi return
            if (!base.isUserAlive || !base.meetsBodyRequirement) return;
            //Verify that person picking up item is same as the person 'getting' the achievement, otherwise return
            if (self.netId == null || self.netId != base.localUser.cachedMasterController.master.netId) return;    
            //Logic for counting starts here
            int itemCount = 0;
            Inventory inventory = self.inventory;
            //Check ukes or polylutes
            if (inventory.GetItemCount(RoR2Content.Items.ChainLightning) >= 1 || inventory.GetItemCount(DLC1Content.Items.ChainLightningVoid) >= 1) itemCount += 1;
            //Check Electric boomerang
            if(inventory.GetItemCount(DLC2Content.Items.StunAndPierce) >= 1) itemCount += 1;
            //Check tesla coils (Of the unstable persuasion)
            if (inventory.GetItemCount(RoR2Content.Items.ShockNearby) >= 1) itemCount += 1;
            //Check vagrant tentacle
            if (inventory.GetItemCount(RoR2Content.Items.NovaOnLowHealth) >= 1) itemCount += 1;
            //Check electric worm tooth
            if (inventory.GetItemCount(RoR2Content.Items.LightningStrikeOnHit) >= 1) itemCount += 1;
            //Check one of a few electric equipment (Royal capacitor, Preon, Volatile battery, Electric elite thing)
            if (inventory.GetEquipmentIndex() == RoR2Content.Equipment.Lightning.equipmentIndex || inventory.GetEquipmentIndex() == RoR2Content.Equipment.AffixBlue.equipmentIndex || inventory.GetEquipmentIndex() == RoR2Content.Equipment.BFG.equipmentIndex || inventory.GetEquipmentIndex() == RoR2Content.Equipment.QuestVolatileBattery.equipmentIndex) itemCount += 1;
            //Finally if enough items, grant
            if (itemCount >= 4) base.Grant();
        }
    }
}
