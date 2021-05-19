using System;
using RoR2;
using UnityEngine;
using EggsSkills.Resources;
using UnityEngine.SocialPlatforms;

namespace EggsSkills.Achievements
{
    class ExpungeAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "ACRID_CUREUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "ACRID_CUREUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "ACRID_CUREUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "ACRID_CUREUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "ACRID_CUREUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.acridpurgeIconS;

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("ACRID_CUREUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("ACRID_CUREUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("ACRID_CUREUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("ACRID_CUREUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.acridRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onUpdate += AcridPurgeAchievementTracker;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onUpdate -= AcridPurgeAchievementTracker;
        }

        private void AcridPurgeAchievementTracker()
        {
            if(base.isUserAlive && base.meetsBodyRequirement)
            {
                if (localUser != null && localUser.cachedBody != null)
                {
                    AcridPurgeTracker component = localUser.cachedBody.gameObject.GetComponent<AcridPurgeTracker>();
                    if(!component)
                    {
                        component = localUser.cachedBody.gameObject.AddComponent<AcridPurgeTracker>();
                    }
                    if(component.GetPoisonedCount() >= 20)
                    {
                        base.Grant();
                    }
                }
            }
        }
    }
}