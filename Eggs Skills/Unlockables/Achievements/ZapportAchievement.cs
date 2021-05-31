using RoR2;
using EggsSkills.Resources;
using System;
using UnityEngine;
using EggsSkills.Config;

namespace EggsSkills.Achievements
{
    internal class ZapportAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "ARTIFICER_FTLUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "ARTIFICER_FTLUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "ARTIFICER_FTLUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "ARTIFICER_FTLUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "ARTIFICER_FTLUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.zapportIconS;
        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("ARTIFICER_FTLUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("ARTIFICER_FTLUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("ARTIFICER_FTLUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("ARTIFICER_FTLUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.artificerRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onUpdate += ClearCheck;
            if (Configuration.UnlockAll.Value)
            {
                base.Grant();
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onUpdate += ClearCheck;
        }

        public void ClearCheck()
        {
            if (base.isUserAlive && base.meetsBodyRequirement)
            {
                if (base.localUser != null && base.localUser.cachedBody && base.localUser.cachedBody.moveSpeed / base.localUser.cachedBody.baseMoveSpeed >= 5f)
                {
                    base.Grant();
                }
            }
        }
    }
}
