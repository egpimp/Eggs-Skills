using System;
using RoR2;
using UnityEngine;
using EggsSkills.Resources;
using EggsSkills.Config;
using R2API;
using RoR2.Achievements;
using System.Collections.Generic;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    internal class RootAchievement : BaseAchievement
    {
        internal const string ACHNAME = "TreebotManyCloseKills";
        internal const string REWARDNAME = "EggsSkills.Root";
        internal const uint TOKENS = 10;

        //How many kills for achievement
        private static readonly int reqKills = 100;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.rexRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onUpdate += GetNearbyBodies;
            On.RoR2.GlobalEventManager.OnCharacterDeath += AddDeath;
            if (Configuration.UnlockAll.Value)
            {
                base.Grant();
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onUpdate -= GetNearbyBodies;
            On.RoR2.GlobalEventManager.OnCharacterDeath -= AddDeath;
        }

        //List of nearby enemies
        private List<HurtBox> nearbyEnemies = new List<HurtBox>();
        //Counts enemies killed
        private int enemyCounter = 0;

        private void GetNearbyBodies()
        {
            nearbyEnemies = new List<HurtBox>();
            if(base.localUser != null && base.localUser.cachedBody != null && base.isUserAlive && base.meetsBodyRequirement)
            {
                foreach (HurtBox hurtBox in new SphereSearch
                {
                    origin = localUser.cachedBody.corePosition,
                    radius = 16,
                    mask = LayerIndex.entityPrecise.mask
                }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(localUser.cachedBody.teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
                {
                    nearbyEnemies.Add(hurtBox.hurtBoxGroup.mainHurtBox);
                }
            }
        }

        private void AddDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            if(base.isUserAlive && base.meetsBodyRequirement)
            {
                if(damageReport.attacker)
                {
                    if(damageReport.attacker.GetComponent<CharacterBody>().master.netId != null && damageReport.attacker.GetComponent<CharacterBody>().master.netId == base.localUser.cachedMasterController.master.netId)
                    {
                        //If killed enemy is a nearby enemy track it
                        if(nearbyEnemies.Contains(damageReport.victimBody.mainHurtBox)) enemyCounter++;
                        //If enough killed, grant
                        if (enemyCounter >= reqKills) base.Grant();
                    }
                }
            }
            orig(self, damageReport);
        }
    }
}