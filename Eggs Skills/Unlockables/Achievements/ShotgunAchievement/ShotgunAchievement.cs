using System;
using RoR2;
using UnityEngine;
using EggsSkills.Resources;
using UnityEngine.Networking;

namespace EggsSkills.Achievements
{
    class ShotgunAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "COMMANDO_BULLETUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "COMMANDO_BULLETUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "COMMANDO_BULLETUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "COMMANDO_BULLETUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "COMMANDO_BULLETUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.shotgunIconS;

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("COMMANDO_BULLETUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("COMMANDO_BULLETUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("COMMANDO_BULLETUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("COMMANDO_BULLETUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.commandoRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            On.RoR2.GlobalEventManager.OnCharacterDeath += BulletAchievementComponentHandler;
            RoR2Application.onUpdate += AddBulletAchievementComponent;

        }
        public override void OnUninstall()
        {
            base.OnUninstall();
            On.RoR2.GlobalEventManager.OnCharacterDeath -= BulletAchievementComponentHandler;
            RoR2Application.onUpdate -= AddBulletAchievementComponent;

        }
        private void BulletAchievementComponentHandler(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            orig(self, damageReport);
            if (base.meetsBodyRequirement && base.isUserAlive)
            {
                if (damageReport.attacker)
                {
                    if (damageReport.attacker.GetComponent<CharacterBody>().master.netId != null && damageReport.attacker.GetComponent<CharacterBody>().master.netId == base.localUser.cachedMasterController.master.netId)
                    {
                        BulletAchievementHandler component = damageReport.attacker.GetComponent<BulletAchievementHandler>();
                        if (!component)
                        {
                            component = damageReport.attacker.AddComponent<BulletAchievementHandler>();
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
        private void AddBulletAchievementComponent()
        {
            if (base.meetsBodyRequirement && base.isUserAlive)
            {
                if (base.localUser != null && base.localUser.cachedBody != null)
                {
                    BulletAchievementHandler bingus = base.localUser.cachedBody.gameObject.GetComponent<BulletAchievementHandler>();
                    if (!bingus)
                    {
                        bingus = base.localUser.cachedBody.gameObject.AddComponent<BulletAchievementHandler>();
                    }
                }
            }
        }
    }
}