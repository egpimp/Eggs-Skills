using RoR2;
using EggsSkills.Config;
using RoR2.Achievements;
using UnityEngine;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    class InvisSprintAchievement : BaseAchievement
    {
        internal const string ACHNAME = "Bandit2LongInvis";
        internal const string REWARDNAME = "EggsSkills.InvisSprint";
        internal const uint TOKENS = 10;

        //Countdown for achievement
        private static float baseCountDown = 180f;
        private float countDown;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.banditRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onFixedUpdate += InvisTracker;
            Run.onRunStartGlobal += ResetTimer;
            if (Configuration.UnlockAll.Value) base.Grant();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onFixedUpdate -= InvisTracker;
            Run.onRunStartGlobal -= ResetTimer;
        }

        private void InvisTracker()
        {
            if (base.isUserAlive && base.meetsBodyRequirement)
            {
                if (localUser != null)
                {
                    CharacterBody body = localUser.cachedBody;
                    if (body != null)
                    {
                        //Countdown while invis
                        if (body.HasBuff(RoR2Content.Buffs.Cloak) && countDown >= 0) countDown -= Time.fixedDeltaTime;
                        //If time is up u good homie
                        if (countDown <= 0) base.Grant();
                    }
                }
            }
        }

        private void ResetTimer(Run run)
        {
            countDown = baseCountDown;
        }
    }
}