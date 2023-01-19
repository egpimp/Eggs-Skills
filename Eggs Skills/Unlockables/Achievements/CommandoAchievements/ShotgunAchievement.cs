using RoR2;
using UnityEngine;
using EggsSkills.Config;
using RoR2.Achievements;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, null)]
    class ShotgunAchievement : BaseAchievement
    {
        internal const string ACHNAME = "CommandoM1Kills";
        internal const string REWARDNAME = "EggsSkills.CombatShotgun";

        //How many kills req to meet
        private static readonly int killsReq = 20;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.commandoRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            On.RoR2.GlobalEventManager.OnCharacterDeath += BulletAchievementComponentHandler;
            RoR2Application.onUpdate += CheckInputs;
            if (Configuration.UnlockAll.Value) base.Grant();
        }
        public override void OnUninstall()
        {
            base.OnUninstall();
            On.RoR2.GlobalEventManager.OnCharacterDeath -= BulletAchievementComponentHandler;
            RoR2Application.onUpdate -= CheckInputs;

        }

        //Counts number of kills while holding m1
        private int killCounter = 0;

        private void BulletAchievementComponentHandler(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            orig(self, damageReport);
            if (base.meetsBodyRequirement && base.isUserAlive)
            {
                if (damageReport.attacker)
                {
                    if (damageReport.attacker.GetComponent<CharacterBody>().master.netId != null && damageReport.attacker.GetComponent<CharacterBody>().master.netId == base.localUser.cachedMasterController.master.netId)
                    {
                        //Kill go up when kill
                        killCounter++;
                        //If met then grant
                        if (killCounter >= killsReq) base.Grant();
                    }
                }
            }
        }

        private void CheckInputs()
        {
            if (base.isUserAlive && base.meetsBodyRequirement)
            {
                InputBankTest input = localUser.cachedBody.inputBank;
                //If m1 not held, or m2 or r pressed, reset kills
                if (!input.skill1.down || input.skill2.down || input.skill4.down) killCounter = 0;
            }
        }
    }
}