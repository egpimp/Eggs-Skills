using System;
using RoR2;
using UnityEngine;
using EggsSkills.Resources;

namespace EggsSkills.Achievements
{
    class BombArrowAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "HUNTRESS_TRADITIONALUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "HUNTRESS_TRADITIONALUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "HUNTRESS_TRADITIONALUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "HUNTRESS_TRADITIONALUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "HUNTRESS_TRADITIONALUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.clusterArrowIconS;

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("HUNTRESS_TRADITIONALUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("HUNTRESS_TRADITIONALUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("HUNTRESS_TRADITIONALUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("HUNTRESS_TRADITIONALUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.huntressRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            TeleporterInteraction.onTeleporterBeginChargingGlobal += GiveComponent;
            TeleporterInteraction.onTeleporterChargedGlobal += CheckComponent;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            TeleporterInteraction.onTeleporterBeginChargingGlobal -= GiveComponent;
            TeleporterInteraction.onTeleporterChargedGlobal -= CheckComponent;
        }

        private void GiveComponent(TeleporterInteraction interaction)
        {
            if (base.meetsBodyRequirement && base.isUserAlive)
            {
                TraditionalAchievementHandler component = localUser.cachedBody.gameObject.GetComponent<TraditionalAchievementHandler>();
                if (!component)
                {
                    component = localUser.cachedBody.gameObject.AddComponent<TraditionalAchievementHandler>();
                }
                component.ResetEligibility();
            }
        }

        private void CheckComponent(TeleporterInteraction interaction)
        {
            if (base.meetsBodyRequirement && base.isUserAlive)
            {
                TraditionalAchievementHandler component = localUser.cachedBody.gameObject.GetComponent<TraditionalAchievementHandler>();
                if (!component)
                {
                    component = localUser.cachedBody.gameObject.AddComponent<TraditionalAchievementHandler>();
                }
                if (component.CheckEligibility())
                {
                    base.Grant();
                }
                component.ResetEligibility();
            }
        }
    }
}