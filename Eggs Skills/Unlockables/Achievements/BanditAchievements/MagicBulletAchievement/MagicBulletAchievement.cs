using R2API;
using UnityEngine;
using System;
using RoR2;
using EggsSkills.Resources;
using EggsSkills.Config;

namespace EggsSkills.Achievements
{
    class MagicBulletAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "BANDIT_HATUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "BANDIT_HATUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "BANDIT_HATUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "BANDIT_HATUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "BANDIT_HATUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.magicBulletIconS;

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("BANDIT_HATUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("BANDIT_HATUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("BANDIT_HATUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("BANDIT_HATUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.banditRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            On.RoR2.GlobalEventManager.OnCharacterDeath += HatTrickAchievementComponentHandler;
            RoR2Application.onUpdate += AddHatTrickAchievementComponent;
            if (Configuration.UnlockAll.Value)
            {
                base.Grant();
            }
        }
        public override void OnUninstall()
        {
            base.OnUninstall();
            On.RoR2.GlobalEventManager.OnCharacterDeath -= HatTrickAchievementComponentHandler;
            RoR2Application.onUpdate -= AddHatTrickAchievementComponent;

        }
        private void HatTrickAchievementComponentHandler(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            orig(self, damageReport);
            if (base.meetsBodyRequirement && base.isUserAlive)
            {
                if (damageReport.attacker)
                {
                    if (damageReport.attacker.GetComponent<CharacterBody>().master.netId != null && damageReport.attacker.GetComponent<CharacterBody>().master.netId == base.localUser.cachedMasterController.master.netId)
                    {
                        HatTrickAchievementHandler component = damageReport.attacker.GetComponent<HatTrickAchievementHandler>();
                        if (!component)
                        {
                            component = damageReport.attacker.AddComponent<HatTrickAchievementHandler>();
                        }
                        component.AddKill();
                        if (component.IsReqMet())
                        {
                            base.Grant();
                        }
                    }
                }
            }
        }
        private void AddHatTrickAchievementComponent()
        {
            if (base.meetsBodyRequirement && base.isUserAlive)
            {
                if (base.localUser != null && base.localUser.cachedBody != null)
                {
                    HatTrickAchievementHandler bingus = base.localUser.cachedBody.gameObject.GetComponent<HatTrickAchievementHandler>();
                    if (!bingus)
                    {
                        bingus = base.localUser.cachedBody.gameObject.AddComponent<HatTrickAchievementHandler>();
                    }
                }
            }
        }
    }
}
