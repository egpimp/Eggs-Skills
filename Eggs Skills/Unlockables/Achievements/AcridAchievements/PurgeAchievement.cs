using RoR2;
using EggsSkills.Config;
using RoR2.Achievements;
using UnityEngine;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    internal class PurgeAchievement : BaseAchievement
    {
        internal const string ACHNAME = "CrocoManyPoisoned";
        internal const string REWARDNAME = "EggsSkills.Expunge";
        internal const uint TOKENS = 10;

        //Max distance to check for poisoned enemies
        private static readonly float maxTrackingDistance = 50000f;
        //20 poisoned enemies required
        private static readonly float poisonCountReq = 20;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.acridRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onUpdate += AcridPurgeAchievementTracker;
            if (Configuration.UnlockAll.Value) base.Grant();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onUpdate -= AcridPurgeAchievementTracker;
        }


        private void AcridPurgeAchievementTracker()
        {
            if(base.isUserAlive && base.meetsBodyRequirement)
            {
                if (localUser != null && localUser.cachedBody != null)
                {
                    //Poisoncounter to 0 cause we haven't found any poisoned creatures yet
                    int poisonCounter = 0;
                    //Spheresearch it
                    foreach (HurtBox hurtBox in new SphereSearch
                    {
                        origin = localUser.cachedBody.footPosition,
                        radius = maxTrackingDistance,
                        mask = LayerIndex.entityPrecise.mask
                    }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(localUser.cachedBody.teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
                    {
                        //Get the targets body component
                        CharacterBody body = hurtBox.healthComponent.body;
                        //Make sure they stats ain't brokeded
                        body.RecalculateStats();
                        //If they have either buff, tick up the counter
                        if (body.HasBuff(RoR2Content.Buffs.Poisoned) || body.HasBuff(RoR2Content.Buffs.Blight)) poisonCounter += 1;
                    }
                    //Check count, grant if yes
                    if (poisonCounter >= poisonCountReq) base.Grant();
                }
            }
        }
    }
}