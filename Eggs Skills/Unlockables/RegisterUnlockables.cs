using EggsSkills.Config;
using RoR2;
using EggsSkills.Achievements;
using R2API;
using UnityEngine;
using static EggsSkills.Resources.Sprites;
using static EggsSkills.Resources.LanguageTokens;
using static EggsSkills.SkillsLoader;
using static EggsUtils.EggsUtils;
using System.Collections.Generic;

namespace EggsSkills.Unlocks
{
    internal static class UnlocksRegistering
    {
        private static List<UnlockableDef> unlockList = new List<UnlockableDef>();

        internal static UnlockableDef acridPurgeUnlockDef;
        internal static UnlockableDef artificerZapportUnlockDef;
        internal static UnlockableDef banditInvisSprintUnlockDef;
        internal static UnlockableDef banditMagicBulletUnlockDef;
        internal static UnlockableDef captainDebuffnadeUnlockDef;
        internal static UnlockableDef commandoDashUnlockDef;
        internal static UnlockableDef commandoShotgunUnlockDef;
        internal static UnlockableDef engiTeslaUnlockDef;
        internal static UnlockableDef huntressClusterarrowUnlockDef;
        internal static UnlockableDef loaderShieldsplosionUnlockDef;
        internal static UnlockableDef mercSlashportUnlockDef;
        internal static UnlockableDef multNanobeaconUnlockDef;
        internal static UnlockableDef rexRootUnlockDef;

        internal static void RegisterUnlockables()
        {
            //Acrid
            if (Configuration.GetConfigValue(Configuration.EnableCrocoSkills)) AcridUnlockables();
            //Artificer
            if (Configuration.GetConfigValue(Configuration.EnableMageSkills)) ArtificerUnlockables();
            //Bandit
            if (Configuration.GetConfigValue(Configuration.EnableBanditSkills)) BanditUnlockables();
            //Captain
            if (Configuration.GetConfigValue(Configuration.EnableCaptainSkills)) CaptainUnlockables();
            //Commando
            if (Configuration.GetConfigValue(Configuration.EnableCommandoSkills)) CommandoUnlockables();
            //Engineer
            if (Configuration.GetConfigValue(Configuration.EnableEngiSkills)) EngiUnlockables();
            //Huntress
            if (Configuration.GetConfigValue(Configuration.EnableHuntressSkills)) HuntressUnlockables();
            //Loader
            if (Configuration.GetConfigValue(Configuration.EnableLoaderSkills)) LoaderUnlockables();
            //Mercenary
            if (Configuration.GetConfigValue(Configuration.EnableMercSkills)) MercUnlockables();
            //Mult
            if (Configuration.GetConfigValue(Configuration.EnableToolbotSkills)) MulTUnlockables();
            //Rex
            if (Configuration.GetConfigValue(Configuration.EnableTreebotSkills)) RexUnlockables();

            //Again, do this in case things change saves pain and agony
            foreach(UnlockableDef def in unlockList)
            {
                ContentAddition.AddUnlockableDef(def);
                LogToConsole("Unlockdef: " + def.cachedName + " registered!");
            }

            LogToConsole("Achievements registered");
        }



        internal static void AcridUnlockables()
        {
            //Expunge unlockdef
            acridPurgeUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            acridPurgeUnlockDef.achievementIcon = acridpurgeIconS;

            acridPurgeUnlockDef.nameToken = prefix + acridName.ToUpper() + "_" + "SPECIAL_PURGE" + nSuffix;
            acridPurgeUnlockDef.cachedName = PurgeAchievement.REWARDNAME;

            acridPurgeUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + PurgeAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + PurgeAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            acridPurgeUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + PurgeAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + PurgeAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            acridPurgeUnlockDef.sortScore = 200;

            unlockList.Add(acridPurgeUnlockDef);
        }

        internal static void ArtificerUnlockables()
        {
            //Zapport unlockdef
            artificerZapportUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            artificerZapportUnlockDef.achievementIcon = zapportIconS;

            artificerZapportUnlockDef.nameToken = prefix + artificerName.ToUpper() + "_" + "UTILITY_ZAPPORT" + nSuffix;
            artificerZapportUnlockDef.cachedName = ZapportAchievement.REWARDNAME;

            artificerZapportUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + ZapportAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + ZapportAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            artificerZapportUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + ZapportAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + ZapportAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            artificerZapportUnlockDef.sortScore = 200;

            unlockList.Add(artificerZapportUnlockDef);
        }

        internal static void BanditUnlockables()
        {
            //Invis Sprint unlockdef
            banditInvisSprintUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            banditInvisSprintUnlockDef.achievementIcon = invisSprintIconS;

            banditInvisSprintUnlockDef.nameToken = prefix + banditName.ToUpper() + "_" + "UTILITY_INVISSPRINT" + nSuffix;
            banditInvisSprintUnlockDef.cachedName = InvisSprintAchievement.REWARDNAME;

            banditInvisSprintUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + InvisSprintAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + InvisSprintAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            banditInvisSprintUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + InvisSprintAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + InvisSprintAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            banditInvisSprintUnlockDef.sortScore = 200;

            unlockList.Add(banditInvisSprintUnlockDef);

            //Magic Bullet unlockdef
            banditMagicBulletUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            banditMagicBulletUnlockDef.achievementIcon = magicBulletIconS;

            banditMagicBulletUnlockDef.nameToken = prefix + banditName.ToUpper() + "_" + "PRIMARY_MAGICBULLET" + nSuffix;
            banditMagicBulletUnlockDef.cachedName = MagicBulletAchievement.REWARDNAME;

            banditMagicBulletUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + MagicBulletAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + MagicBulletAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            banditMagicBulletUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + MagicBulletAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + MagicBulletAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            banditMagicBulletUnlockDef.sortScore = 200;

            unlockList.Add(banditMagicBulletUnlockDef);
        }

        internal static void CaptainUnlockables()
        {
            //Debuffnade unlockdef
            captainDebuffnadeUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            captainDebuffnadeUnlockDef.achievementIcon = debuffNadeIconS;

            captainDebuffnadeUnlockDef.nameToken = prefix + captainName.ToUpper() + "_" + "SECONDARY_DEBUFFNADE" + nSuffix;
            captainDebuffnadeUnlockDef.cachedName = DebuffnadeAchievement.REWARDNAME;

            captainDebuffnadeUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + DebuffnadeAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + DebuffnadeAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            captainDebuffnadeUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + DebuffnadeAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + DebuffnadeAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            captainDebuffnadeUnlockDef.sortScore = 200;

            unlockList.Add(captainDebuffnadeUnlockDef);
        }

        internal static void CommandoUnlockables()
        {
            //Combat Shotgun unlockdef
            commandoShotgunUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            commandoShotgunUnlockDef.achievementIcon = shotgunIconS;

            commandoShotgunUnlockDef.nameToken = prefix + commandoName.ToUpper() + "_" + "PRIMARY_COMBATSHOTGUN" + nSuffix;
            commandoShotgunUnlockDef.cachedName = ShotgunAchievement.REWARDNAME;

            commandoShotgunUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + ShotgunAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + ShotgunAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            commandoShotgunUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + ShotgunAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + ShotgunAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            commandoShotgunUnlockDef.sortScore = 200;

            unlockList.Add(commandoShotgunUnlockDef);

            //Dash unlockdef
            commandoDashUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            commandoDashUnlockDef.achievementIcon = dashIconS;

            commandoDashUnlockDef.nameToken = prefix + commandoName.ToUpper() + "_" + "UTILITY_DASH" + nSuffix;
            commandoDashUnlockDef.cachedName = DashAchievement.REWARDNAME;

            commandoDashUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + DashAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + DashAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            commandoDashUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + DashAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + DashAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            commandoDashUnlockDef.sortScore = 200;

            unlockList.Add(commandoDashUnlockDef);
        }

        internal static void EngiUnlockables()
        {
            //Teslamine unlockdef
            engiTeslaUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            engiTeslaUnlockDef.achievementIcon = teslaMineIconS;

            engiTeslaUnlockDef.nameToken = prefix + engineerName.ToUpper() + "_" + "SECONDARY_TESLAMINE" + nSuffix;
            engiTeslaUnlockDef.cachedName = TeslaMineAchievement.REWARDNAME;

            engiTeslaUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + TeslaMineAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + TeslaMineAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            engiTeslaUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + TeslaMineAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + TeslaMineAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            engiTeslaUnlockDef.sortScore = 200;

            unlockList.Add(engiTeslaUnlockDef);
        }

        internal static void HuntressUnlockables()
        {
            huntressClusterarrowUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            huntressClusterarrowUnlockDef.achievementIcon = clusterArrowIconS;

            huntressClusterarrowUnlockDef.nameToken = prefix + huntressName.ToUpper() + "_" + "SECONDARY_CLUSTERARROW" + nSuffix;
            huntressClusterarrowUnlockDef.cachedName = BombArrowAchievement.REWARDNAME;

            huntressClusterarrowUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + BombArrowAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + BombArrowAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            huntressClusterarrowUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + BombArrowAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + BombArrowAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            huntressClusterarrowUnlockDef.sortScore = 200;

            unlockList.Add(huntressClusterarrowUnlockDef);
        }

        internal static void LoaderUnlockables()
        {
            loaderShieldsplosionUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            loaderShieldsplosionUnlockDef.achievementIcon = shieldsplosionIconS;

            loaderShieldsplosionUnlockDef.nameToken = prefix + loaderName.ToUpper() + "_" + "SPECIAL_SHIELDSPLOSION" + nSuffix;
            loaderShieldsplosionUnlockDef.cachedName = BarrierAchievement.REWARDNAME;

            loaderShieldsplosionUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + BarrierAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + BarrierAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            loaderShieldsplosionUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + BarrierAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + BarrierAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            loaderShieldsplosionUnlockDef.sortScore = 200;

            unlockList.Add(loaderShieldsplosionUnlockDef);
        }

        internal static void MercUnlockables()
        {
            mercSlashportUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            mercSlashportUnlockDef.achievementIcon = slashportIconS;

            mercSlashportUnlockDef.nameToken = prefix + mercenaryName.ToUpper() + "_" + "SPECIAL_SLASHPORT" + nSuffix;
            mercSlashportUnlockDef.cachedName = SlashportAchievement.REWARDNAME;

            mercSlashportUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + SlashportAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + SlashportAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            mercSlashportUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + SlashportAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + SlashportAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            mercSlashportUnlockDef.sortScore = 200;

            unlockList.Add(mercSlashportUnlockDef);
        }

        internal static void MulTUnlockables()
        {
            multNanobeaconUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            multNanobeaconUnlockDef.achievementIcon = nanoBotsIconS;

            multNanobeaconUnlockDef.nameToken = prefix + multName.ToUpper() + "_" + "SECONDARY_NANOBOT" + nSuffix;
            multNanobeaconUnlockDef.cachedName = NanoBotAchievement.REWARDNAME;

            multNanobeaconUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + NanoBotAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + NanoBotAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            multNanobeaconUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + NanoBotAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + NanoBotAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            multNanobeaconUnlockDef.sortScore = 200;

            unlockList.Add(multNanobeaconUnlockDef);
        }

        internal static void RexUnlockables()
        {
            rexRootUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            rexRootUnlockDef.achievementIcon = rexrootIconS;

            rexRootUnlockDef.nameToken = prefix + rexName.ToUpper() + "_" + "SPECIAL_ROOT" + nSuffix;
            rexRootUnlockDef.cachedName = RootAchievement.REWARDNAME;

            rexRootUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + RootAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + RootAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            rexRootUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString(ach_prefix + RootAchievement.ACHNAME.ToUpper() + nSuffix),
                Language.GetString(ach_prefix + RootAchievement.ACHNAME.ToUpper() + dSuffix)
            }));

            rexRootUnlockDef.sortScore = 200;

            unlockList.Add(rexRootUnlockDef);
        }
    }
}
