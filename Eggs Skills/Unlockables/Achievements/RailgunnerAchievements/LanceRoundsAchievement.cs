using RoR2.Achievements;
using RoR2;
using EggsSkills.Config;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    internal class LanceRoundsAchievement : BaseAchievement
    {
        internal const string ACHNAME = "RailgunnerHighCrit";
        internal const string REWARDNAME = "EggsSkills.LanceRounds";
        internal const uint TOKENS = 10;

        //How much crit damage to unlock
        //Note: 2x mult is normal crit, 3x mult is 10 glasses (For Railgunner), 4x mult is what we want
        private static readonly float critReq = 4f;
 
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.railgunnerRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onUpdate += ClearCheck;
            if (Configuration.UnlockAll.Value)
            {
                base.Grant();
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onUpdate -= ClearCheck;
        }

        public void ClearCheck()
        {
            //Make sure player is alive, and make sure they are rg
            if (base.isUserAlive && base.meetsBodyRequirement)
            {
                //Make sure local user exists, then make sure their body exists, then make sure their move speed meets the req
                if (base.localUser != null && base.localUser.cachedBody && base.localUser.cachedBody.critMultiplier >= critReq)
                {
                    base.Grant();
                }
            }
        }
    }
}
