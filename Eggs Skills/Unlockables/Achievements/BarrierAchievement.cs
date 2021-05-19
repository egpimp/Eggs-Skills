using System;
using RoR2;
using UnityEngine;
using EggsSkills.Resources;

namespace EggsSkills.Achievements
{
    class BarrierAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "LOADER_BARRIERUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "LOADER_BARRIERUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "LOADER_BARRIERUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "LOADER_BARRIERUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "LOADER_BARRIERUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.shieldsplosionIconS;

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("LOADER_BARRIERUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("LOADER_BARRIERUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("LOADER_BARRIERUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("LOADER_BARRIERUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.loaderRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onUpdate += BarrierClearCheck;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onUpdate -= BarrierClearCheck;
        }

        public void BarrierClearCheck()
        {
            if (base.localUser != null && base.localUser.cachedBody != null && base.isUserAlive && base.meetsBodyRequirement)
            {
                float healthFraction = 0.95f * base.localUser.cachedBody.healthComponent.fullCombinedHealth;
                if (base.localUser.cachedBody.healthComponent.barrier >= healthFraction)
                {
                    base.Grant();
                }
            }
        }
    }
}
