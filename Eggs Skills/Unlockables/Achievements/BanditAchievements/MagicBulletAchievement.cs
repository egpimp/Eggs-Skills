using UnityEngine;
using RoR2;
using EggsSkills.Config;
using RoR2.Achievements;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    internal class MagicBulletAchievement : BaseAchievement
    {
        internal const string ACHNAME = "Bandit2FastKills";
        internal const string REWARDNAME = "EggsSkills.MagicBullet";
        internal const uint TOKENS = 10;

        //How many kills in 1s to get achievement
        private static readonly int reqKills = 3;
        //How many seconds to get the kills
        private static readonly float killTime = 1f;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.banditRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            On.RoR2.GlobalEventManager.OnCharacterDeath += HatTrickAchievementComponentHandler;
            RoR2Application.onFixedUpdate += RunCountDown;
            if (Configuration.UnlockAll.Value) base.Grant();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            On.RoR2.GlobalEventManager.OnCharacterDeath -= HatTrickAchievementComponentHandler;
            RoR2Application.onFixedUpdate -= RunCountDown;
        }

        //Bumps up to 1 when a kill is made while 0, counts down while above one
        private float killCountdown = 0f;
        //Counts kills during the 1s timer
        private int killCount = 0;

        private void HatTrickAchievementComponentHandler(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            orig(self, damageReport);
            if (base.meetsBodyRequirement && base.isUserAlive)
            {
                if (damageReport.attacker)
                {
                    if (damageReport.attacker.GetComponent<CharacterBody>().master.netId != null && damageReport.attacker.GetComponent<CharacterBody>().master.netId == base.localUser.cachedMasterController.master.netId)
                    {
                        //Always tick a kill up when a kill is made
                        killCount += 1;
                        //If the counter is up, however (AKA no active tracking of kills being done) reset it to 1
                        if (killCountdown <= 0) killCountdown = killTime;
                        if (killCount >= reqKills) base.Grant();
                    }
                }
            }
        }

        private void RunCountDown()
        {
            //If counter is active, tick it down
            if (killCountdown > 0f) killCountdown -= Time.fixedDeltaTime;
            //Otherwise, reset killcount
            else killCount = 0;
        }
    }
}
