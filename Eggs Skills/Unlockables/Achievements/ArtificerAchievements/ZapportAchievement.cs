using RoR2;
using EggsSkills.Config;
using RoR2.Achievements;
using UnityEngine;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    internal class ZapportAchievement : BaseAchievement
    {
        internal const string ACHNAME = "MageFastMoveSpeed";
        internal const string REWARDNAME = "EggsSkills.Zapport";
        internal const uint TOKENS = 10;

        //500% normal ms required
        private static readonly float moveSpeedReq = 5f;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.artificerRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onUpdate += ClearCheck;
            if (Configuration.UnlockAll.Value) base.Grant();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onUpdate += ClearCheck;
        }

        public void ClearCheck()
        {
            //Make sure player is alive, and make sure they are arti
            if (base.isUserAlive && base.meetsBodyRequirement)
            {
                //Make sure local user exists, then make sure their body exists, then make sure their move speed meets the req
                if (base.localUser != null && base.localUser.cachedBody && base.localUser.cachedBody.moveSpeed / base.localUser.cachedBody.baseMoveSpeed >= moveSpeedReq)
                {
                    base.Grant();
                }
            }
        }
    }
}
