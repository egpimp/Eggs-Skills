using System;
using RoR2;
using UnityEngine;
using EggsSkills.Resources;
using EntityStates.Captain.Weapon;
using RoR2.Stats;
using EggsSkills.Config;
using R2API;

namespace EggsSkills.Achievements
{
    class DebuffnadeAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "CAPTAIN_SUPPORTUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "CAPTAIN_SUPPORTUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "CAPTAIN_SUPPORTUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "CAPTAIN_SUPPORTUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "CAPTAIN_SUPPORTUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.debuffNadeIconS;

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("CAPTAIN_SUPPORTUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("CAPTAIN_SUPPORTUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("CAPTAIN_SUPPORTUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("CAPTAIN_SUPPORTUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.captainRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            Stage.onStageStartGlobal += ResetCaptainComponent;
            TeleporterInteraction.onTeleporterChargedGlobal += CheckCaptainComponent;
            On.EntityStates.Captain.Weapon.CallSupplyDropBase.OnEnter += InvalidateAchievement;
            if (Configuration.UnlockAll.Value)
            {
                base.Grant();
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            Stage.onStageStartGlobal -= ResetCaptainComponent;
            TeleporterInteraction.onTeleporterChargedGlobal -= CheckCaptainComponent;
            On.EntityStates.Captain.Weapon.CallSupplyDropBase.OnEnter -= InvalidateAchievement;
        }

        private void ResetCaptainComponent(Stage stage)
        {
            if(base.meetsBodyRequirement)
            {
                SupportAchievementHandler component = base.localUser.cachedMasterController.master.gameObject.GetComponent<SupportAchievementHandler>();
                if(!component)
                {
                    component = base.localUser.cachedMasterController.master.gameObject.AddComponent<SupportAchievementHandler>();
                }
                component.Revalidate();
            }
        }

        private void CheckCaptainComponent(TeleporterInteraction interaction)
        {
            if(base.isUserAlive && base.meetsBodyRequirement)
            {
                SupportAchievementHandler component = base.localUser.cachedMasterController.master.gameObject.GetComponent<SupportAchievementHandler>();
                if(!component)
                {
                    component = base.localUser.cachedMasterController.master.gameObject.AddComponent<SupportAchievementHandler>();
                }
                if(component.IsValid())
                {
                    PlayerStatsComponent statsComponent = base.localUser.cachedStatsComponent;
                    if(statsComponent && statsComponent.currentStats.GetStatValueULong(StatDef.highestStagesCompleted) >= 2UL)
                    {
                        base.Grant();
                    }
                }
            }
        }

        private void InvalidateAchievement(On.EntityStates.Captain.Weapon.CallSupplyDropBase.orig_OnEnter orig, CallSupplyDropBase self)
        {
            orig(self);
            SupportAchievementHandler component = self.characterBody.master.gameObject.GetComponent<SupportAchievementHandler>();
            if (component)
            {
                component.Invalidate();
            }
        }
    }
}