using RoR2;
using EggsSkills.Config;
using RoR2.Achievements;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, null)]
    internal class DashAchievement : BaseAchievement
    {
        internal const string ACHNAME = "CommandoFinishTeleporterLowHealth";
        internal const string REWARDNAME = "EggsSkills.Dash";

        //What hp % for req to be met
        private static readonly float hpReq = 0.2f;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.commandoRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            TeleporterInteraction.onTeleporterChargedGlobal += CheckAchievement;
            if (Configuration.UnlockAll.Value)
            {
                base.Grant();
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            TeleporterInteraction.onTeleporterChargedGlobal -= CheckAchievement;
        }

        private void CheckAchievement(TeleporterInteraction interaction)
        {
            if(base.isUserAlive && base.meetsBodyRequirement)
            {
                //If health is under requirement it's done
                if (base.localUser.cachedBody.healthComponent.health <= base.localUser.cachedBody.healthComponent.fullHealth * hpReq)
                {
                    base.Grant();
                }
            }
        }
    }
}
