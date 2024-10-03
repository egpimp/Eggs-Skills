using EggsSkills.Config;
using RoR2;
using RoR2.Achievements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    class MicromissileAchievement : BaseAchievement
    {
        internal const string ACHNAME = "EngiKillVagrantMine";
        internal const string REWARDNAME = "EggsSkills.Micromissiles";
        internal const uint TOKENS = 10;

        //Valid mines for the achievement
        internal static readonly string[] validMines = new string[] {"Engi", "Spider", "Tesla" };

        //Indexes for identifying correct prjoectile (Mines)
        internal static int[] validProjectilesIndexes;
        internal static BodyIndex vagrantBossBodyIndex = BodyCatalog.FindBodyIndex("VagrantBody");

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.engineerRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            validProjectilesIndexes = new int[validMines.Length];
            for(int i = 0; i < validMines.Length; i++) validProjectilesIndexes[i] = ProjectileCatalog.FindProjectileIndex($"{validMines[i]}Mine");
            GlobalEventManager.onCharacterDeathGlobal += CheckKill;
            if (Configuration.UnlockAll.Value) base.Grant();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            GlobalEventManager.onCharacterDeathGlobal -= CheckKill;
        }

        private void CheckKill(DamageReport report)
        {
            //If they are not engi return
            if (!base.meetsBodyRequirement) return;
            //If victim body doesn't exist return
            if (!report.victimBody) return;
            //If they are not vagrant return
            if (report.victimBodyIndex != vagrantBossBodyIndex) return;
            //If attacker doesn't exist return
            if (!report.damageInfo.attacker) return;
            //If killer netid does not match this net id return
            if (report.attacker.GetComponent<CharacterBody>().master.netId == null || report.attacker.GetComponent<CharacterBody>().master.netId != base.localUser.cachedMasterController.master.netId) return;
            //If not engi mine return
            if (!validProjectilesIndexes.Contains(ProjectileCatalog.GetProjectileIndex(report.damageInfo.inflictor))) return;
            //Finally if the above all fits, grant the achivement
            base.Grant();
        }
    }
}
