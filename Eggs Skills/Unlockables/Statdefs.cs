using RoR2;
using RoR2.Stats;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EggsSkills.Stats
{
    internal class Statdefs
    {
        internal static void InitStatHooks()
        {
            GlobalEventManager.onServerDamageDealt += OnDamageDealt;
        }

        internal static readonly StatDef totalCrocoDotDamage = StatDef.Register("totalCrocoDotDamage", StatRecordType.Sum, StatDataType.ULong, 0.01, null);
    
        private static void OnDamageDealt(DamageReport report)
        {
            if (!report.attacker) return;
            StatSheet attackerSheet = PlayerStatsComponent.FindMasterStatSheet(report.attackerMaster);
            if(attackerSheet != null)
            {
                if (report.attackerBodyIndex == StatManager.crocoBodyIndex && (report.damageInfo.damageType & DamageType.DoT) == DamageType.DoT) attackerSheet.PushStatValue(totalCrocoDotDamage, (ulong)report.damageDealt);
            }
        }
    }
}
