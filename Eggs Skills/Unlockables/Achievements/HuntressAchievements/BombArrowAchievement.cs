using RoR2;
using EggsSkills.Config;
using RoR2.Achievements;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, null)]
    class BombArrowAchievement : BaseAchievement
    {
        internal const string ACHNAME = "HuntressPrimaryUtilityOnly";
        internal const string REWARDNAME = "EggsSkills.ClusterArrow";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.huntressRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            TeleporterInteraction.onTeleporterBeginChargingGlobal += GiveComponent;
            TeleporterInteraction.onTeleporterChargedGlobal += CheckComponent;
            RoR2Application.onUpdate += CheckInputs;
            if (Configuration.UnlockAll.Value) base.Grant();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            TeleporterInteraction.onTeleporterBeginChargingGlobal -= GiveComponent;
            TeleporterInteraction.onTeleporterChargedGlobal -= CheckComponent;
            RoR2Application.onUpdate -= CheckInputs;
        }

        //Checks if allowed to get achievement
        bool isValid = false;

        private void GiveComponent(TeleporterInteraction interaction)
        {
            if (base.meetsBodyRequirement && base.isUserAlive)
            {
                isValid = true;
            }
        }

        private void CheckComponent(TeleporterInteraction interaction)
        {
            if (base.meetsBodyRequirement && base.isUserAlive)
            {
                if (isValid) base.Grant();
            }
        }

        private void CheckInputs()
        {
            if (base.isUserAlive && base.meetsBodyRequirement)
            {
                InputBankTest input = base.localUser.cachedBody.inputBank;
                if (input.skill2.down || input.skill4.down) isValid = false;
            }
        }
    }
}