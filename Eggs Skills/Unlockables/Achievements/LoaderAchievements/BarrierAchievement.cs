using RoR2;
using EggsSkills.Config;
using RoR2.Achievements;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    internal class BarrierAchievement : BaseAchievement
    {
        internal const string ACHNAME = "LoaderHighBarrier";
        internal const string REWARDNAME = "EggsSkills.ShieldSplosion";
        internal const uint TOKENS = 10;

        //What % barrier required
        private static readonly float barrierReq = 0.95f;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.loaderRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onUpdate += BarrierClearCheck;
            if (Configuration.UnlockAll.Value) base.Grant();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onUpdate -= BarrierClearCheck;
        }

        public void BarrierClearCheck()
        {
            if (base.localUser != null && base.localUser.cachedBody != null && base.isUserAlive && base.meetsBodyRequirement && base.localUser.cachedBody.healthComponent.barrier > 0f)
            {
                float healthFraction = barrierReq * base.localUser.cachedBody.healthComponent.fullCombinedHealth;
                if (base.localUser.cachedBody.healthComponent.barrier > healthFraction) base.Grant();
            }
        }
    }
}
