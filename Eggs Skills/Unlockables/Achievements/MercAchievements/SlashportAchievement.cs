using RoR2;
using EggsSkills.Config;
using RoR2.Achievements;
using System.Collections.Generic;
using UnityEngine;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    internal class SlashportAchievement : BaseAchievement
    {
        internal const string ACHNAME = "MercExposeEnemies";
        internal const string REWARDNAME = "EggsSkills.Slashport";
        internal const uint TOKENS = 10;

        //How many exposed enemies hit required
        private static readonly int exposeReq = 10;
        //Time between hits
        private static readonly float baseTimer = 5f;
        private float timer;


        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.mercenaryRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onFixedUpdate += Timer;
            On.RoR2.HealthComponent.TakeDamage += DamageChecker;
            if (Configuration.UnlockAll.Value) base.Grant();
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onFixedUpdate -= Timer;
            On.RoR2.HealthComponent.TakeDamage -= DamageChecker;
        }

        private void Timer()
        {
            if (timer > 0f) timer -= Time.fixedDeltaTime;
            else enemyList.Clear();
        }

        private List<CharacterBody> enemyList = new List<CharacterBody>();

        private void DamageChecker(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (damageInfo.attacker && damageInfo.attacker.GetComponent<CharacterBody>())
            {
                if (base.isUserAlive && base.meetsBodyRequirement)
                {
                    //Make sure attacker is player
                    if (damageInfo.attacker.GetComponent<CharacterBody>().master && damageInfo.attacker.GetComponent<CharacterBody>().master.netId != null && damageInfo.attacker.GetComponent<CharacterBody>().master.netId == base.localUser.cachedMasterController.master.netId)
                    {
                        if (self && self.body)
                        {
                            //Make sure enemy has buff and isn't in the list
                            if (self.body.HasBuff(RoR2Content.Buffs.MercExpose) && !enemyList.Contains(self.body))
                            {
                                //Add them to the list
                                enemyList.Add(self.body);
                                //Reset timer
                                timer = baseTimer;
                                //If we have enough in list grant achievement
                                if (enemyList.Count >= exposeReq) base.Grant();
                            }
                        }
                    }
                }
            }
            orig(self, damageInfo);
        }
    }
}