using RoR2.Achievements;
using RoR2.Stats;
using System;
using System.Collections.Generic;
using System.Text;
using EggsSkills.Stats;
using RoR2;
using EggsSkills.Config;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    internal class PoisonbreathAchievement : BaseStatMilestoneAchievement
    {
        internal const string ACHNAME = "CrocoDotDamage";
        internal const string REWARDNAME = "EggsSkills.Poisonbreath";
        internal const uint TOKENS = 10;

        private const ulong damageRequirement = 500000;
        
        public override StatDef statDef
        {
            get
            {
                return Statdefs.totalCrocoDotDamage;
            }
        }

        public override ulong statRequirement
        {
            get
            {
                if (Configuration.UnlockAll.Value) return 0;
                else return damageRequirement;
            }
        }
    }
}
