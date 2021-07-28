using System;
using RoR2;
using UnityEngine;
using EggsSkills.Resources;
using EggsSkills.Config;
using R2API;

namespace EggsSkills.Achievements
{
    class RootAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "REX_BREATHINGUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "REX_BREATHINGUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "REX_BREATHINGUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "REX_BREATHINGUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "REX_BREATHINGUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.rexrootIconS;

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("REX_BREATHINGUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("REX_BREATHINGUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("REX_BREATHINGUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("REX_BREATHINGUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.rexRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onUpdate += RootComponentCheck;
            On.RoR2.GlobalEventManager.OnCharacterDeath += AddDeath;
            if (Configuration.UnlockAll.Value)
            {
                base.Grant();
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onUpdate -= RootComponentCheck;
            On.RoR2.GlobalEventManager.OnCharacterDeath -= AddDeath;
        }

        private void RootComponentCheck()
        {
            if(base.localUser != null && base.localUser.cachedBody != null && base.isUserAlive && base.meetsBodyRequirement)
            {
                BreathingroomAchievementHandler component = localUser.cachedBody.gameObject.GetComponent<BreathingroomAchievementHandler>();
                if(!component)
                {
                    component = localUser.cachedBody.gameObject.AddComponent<BreathingroomAchievementHandler>();
                }
                if(component.HasReqKills())
                {
                    base.Grant();
                }
            }
        }

        private void AddDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            if(base.isUserAlive && base.meetsBodyRequirement)
            {
                if(damageReport.attacker)
                {
                    if(damageReport.attacker.GetComponent<CharacterBody>().master.netId != null && damageReport.attacker.GetComponent<CharacterBody>().master.netId == base.localUser.cachedMasterController.master.netId)
                    {
                        BreathingroomAchievementHandler component = damageReport.attacker.GetComponent<BreathingroomAchievementHandler>();
                        if(component)
                        {
                            component.AddKill(damageReport.victimBody);
                        }
                    }
                }
            }
            orig(self, damageReport);
        }
    }
}