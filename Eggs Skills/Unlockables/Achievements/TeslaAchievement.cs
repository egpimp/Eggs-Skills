using EggsSkills.Resources;
using RoR2;
using System;
using UnityEngine;

namespace EggsSkills.Achievements
{
    internal class TeslaMineAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "ENGI_ELECTRICUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "ENGI_ELECTRICUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "ENGI_ELECTRICUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "ENGI_ELECTRICUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "ENGI_ELECTRICUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.teslaMineIconS;

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("ENGI_ELECTRICUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("ENGI_ELECTRICUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("ENGI_ELECTRICUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("ENGI_ELECTRICUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.engineerRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            On.RoR2.CharacterMaster.OnInventoryChanged += ClearCheck;
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
                    bool flag1 = false;
                    bool flag2 = false;
                    bool flag3 = false;
                    bool flag4 = false;
                    Inventory inventory = self.inventory;
                    if (inventory.GetItemCount(RoR2Content.Items.ChainLightning) >= 1)
                    {
                        flag1 = true;
                    }
                    if (inventory.GetItemCount(RoR2Content.Items.ShockNearby) >= 1)
                    {
                        flag2 = true;
                    }
                    if (inventory.GetEquipmentIndex() == RoR2Content.Equipment.Lightning.equipmentIndex || inventory.GetEquipmentIndex() == RoR2Content.Equipment.AffixBlue.equipmentIndex)
                    {
                        flag3 = true;
                    }
                    if (inventory.GetItemCount(RoR2Content.Items.LightningStrikeOnHit) >= 1)
                    {
                        flag4 = true;
                    }
                    if (flag1 && flag2 && flag3 && flag4)
                    {
                        base.Grant();
                    }
                }
            }
        }
    }
}
