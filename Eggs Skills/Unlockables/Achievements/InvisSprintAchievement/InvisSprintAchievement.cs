using System;
using RoR2;
using UnityEngine;
using EggsSkills.Resources;

namespace EggsSkills.Achievements
{
    class InvisSprintAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "BANDIT_FLANKEDUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "BANDIT_FLANKEDUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "BANDIT_FLANKEDUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "BANDIT_FLANKEDUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "BANDIT_FLANKEDUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.invisSprintIconS;

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("BANDIT_FLANKEDUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("BANDIT_FLANKEDUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("BANDIT_FLANKEDUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("BANDIT_FLANKEDUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.banditRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onUpdate += InvisTracker;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onUpdate += InvisTracker;
        }

        private void InvisTracker()
        {
            if (base.isUserAlive && base.meetsBodyRequirement)
            {
                if (localUser != null)
                {
                    CharacterBody body = localUser.cachedBody;
                    if (body != null)
                    {
                        FlankedAchievmentHandler component = body.gameObject.GetComponent<FlankedAchievmentHandler>();
                        if (component)
                        {
                            if (component.MeetsRequirement())
                            {
                                base.Grant();
                            }
                        }
                        else
                        {
                            component = body.gameObject.AddComponent<FlankedAchievmentHandler>();
                        }
                    }
                }
            }
        }
    }
}