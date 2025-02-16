using EggsSkills.Config;
using RoR2;
using RoR2.Achievements;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    class AutoshotgunAchievement : BaseAchievement
    {
        internal const string ACHNAME = "Captain3MountainTeleporter";
        internal const string REWARDNAME = "EggsSkills.Autoshotgun";
        internal const uint TOKENS = 10;

        private static readonly int shrineCount = 3;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.captainRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            TeleporterInteraction.onTeleporterFinishGlobal += ShrineCheck;
            if (Configuration.UnlockAll.Value) base.Grant();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            TeleporterInteraction.onTeleporterFinishGlobal -= ShrineCheck;
        }

        private void ShrineCheck(TeleporterInteraction interaction)
        {
            if (!base.isUserAlive) return;
            if (!base.meetsBodyRequirement) return;
            if (interaction && interaction.shrineBonusStacks >= shrineCount) base.Grant();
        }
    }
}
