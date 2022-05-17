using RoR2;
using EggsSkills.Config;
using RoR2.Achievements;
using UnityEngine;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, null)]
    class InvisSprintAchievement : BaseAchievement
    {
        internal const string ACHNAME = "Bandit2LongInvis";
        internal const string REWARDNAME = "EggsSkills.InvisSprint";

        //Countdown for achievement
        private float countDown = 180f;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.banditRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onFixedUpdate += InvisTracker;
            if (Configuration.UnlockAll.Value)
            {
                base.Grant();
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onFixedUpdate += InvisTracker;
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
                        Debug.Log("Tick: " + countDown);
                    }
                }
            }
        }
    }
}