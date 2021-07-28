using System;
using R2API;
using RoR2;
using UnityEngine;
using EggsSkills.Resources;
using EggsSkills.Config;

namespace EggsSkills.Achievements
{
    class DashAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "COMMANDO_PERSEVEREUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "COMMANDO_PERSEVEREUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "COMMANDO_PERSEVEREUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "COMMANDO_PERSEVEREUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "COMMANDO_PERSEVEREUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.dashIconS;

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("COMMANDO_PERSEVEREUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("COMMANDO_PERSEVEREUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("COMMANDO_PERSEVEREUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("COMMANDO_PERSEVEREUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.commandoRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            TeleporterInteraction.onTeleporterChargedGlobal += CheckAchievement;
            if (Configuration.UnlockAll.Value)
            {
                base.Grant();
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            TeleporterInteraction.onTeleporterChargedGlobal -= CheckAchievement;
        }

        private void CheckAchievement(TeleporterInteraction interaction)
        {
            if(base.isUserAlive && base.meetsBodyRequirement)
            {
                if (base.localUser.cachedBody.healthComponent.health <= base.localUser.cachedBody.healthComponent.fullHealth / 5)
                {
                    base.Grant();
                }
            }
        }
    }
}
