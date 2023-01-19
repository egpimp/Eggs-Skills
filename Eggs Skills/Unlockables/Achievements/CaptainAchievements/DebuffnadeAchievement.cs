using RoR2;
using UnityEngine;
using EntityStates.Captain.Weapon;
using RoR2.Stats;
using EggsSkills.Config;
using RoR2.Achievements;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, null)]
    class DebuffnadeAchievement : BaseAchievement
    {
        internal const string ACHNAME = "CaptainStageNoBeacon";
        internal const string REWARDNAME = "EggsSkills.DebuffNade";

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
            if (Configuration.UnlockAll.Value) base.Grant();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            Stage.onStageStartGlobal -= ResetCaptainComponent;
            TeleporterInteraction.onTeleporterChargedGlobal -= CheckCaptainComponent;
            On.EntityStates.Captain.Weapon.CallSupplyDropBase.OnEnter -= InvalidateAchievement;
        }

        //Keeps track of whether or not achievement unlocking is valid for this stage
        private bool isValid = false;

        private void ResetCaptainComponent(Stage stage)
        {
            if(base.meetsBodyRequirement)
            {
                //Should always be valid at stage start
                isValid = true;
            }
        }

        private void CheckCaptainComponent(TeleporterInteraction interaction)
        {
            if(base.isUserAlive && base.meetsBodyRequirement && isValid)
            {
                PlayerStatsComponent statsComponent = base.localUser.cachedStatsComponent;
                if(statsComponent && statsComponent.currentStats.GetStatValueULong(StatDef.highestStagesCompleted) >= 2UL)
                {
                    //Grant when valid and stage 3+
                    base.Grant();
                }
            }
        }

        private void InvalidateAchievement(On.EntityStates.Captain.Weapon.CallSupplyDropBase.orig_OnEnter orig, CallSupplyDropBase self)
        {
            orig(self);
            //Invalidate when player uses supply drop
            if (self.characterBody.master == base.localUser.cachedMaster) isValid = false;
        }
    }
}