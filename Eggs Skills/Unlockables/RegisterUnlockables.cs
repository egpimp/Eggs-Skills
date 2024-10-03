using EggsSkills.Config;
using RoR2;
using EggsSkills.Achievements;
using R2API;
using UnityEngine;
using static EggsSkills.Resources.Sprites;
using System.Collections.Generic;
using EggsSkills.Stats;

namespace EggsSkills.Unlocks
{
    internal static class UnlocksRegistering
    {
        private static List<UnlockableDef> unlockList = new List<UnlockableDef>();

        internal static UnlockableDef acridPurgeUnlockDef;
        internal static UnlockableDef acridPoisonbreathUnlockDef;
        internal static UnlockableDef artificerZapportUnlockDef;
        internal static UnlockableDef banditInvisSprintUnlockDef;
        internal static UnlockableDef banditMagicBulletUnlockDef;
        internal static UnlockableDef captainDebuffnadeUnlockDef;
        internal static UnlockableDef captainAutoshotgunUnlockDef;
        internal static UnlockableDef commandoDashUnlockDef;
        internal static UnlockableDef commandoShotgunUnlockDef;
        internal static UnlockableDef engiTeslaUnlockDef;
        internal static UnlockableDef engiMicromissileUnlockDef;
        internal static UnlockableDef huntressClusterarrowUnlockDef;
        internal static UnlockableDef loaderShieldsplosionUnlockDef;
        internal static UnlockableDef mercSlashportUnlockDef;
        internal static UnlockableDef multNanobeaconUnlockDef;
        internal static UnlockableDef railgunnerLanceroundsUnlockDef;
        internal static UnlockableDef rexRootUnlockDef;
        internal static UnlockableDef voidsurvivorInversionUnlockDef;

        internal static void RegisterUnlockables()
        {
            Statdefs.InitStatHooks();
            //Acrid
            try { if (Configuration.GetConfigValue(Configuration.EnableCrocoSkills)) AcridUnlockables(); } catch { Log.LogError("Failed to load Acrid achivements"); }
            //Artificer
            try { if (Configuration.GetConfigValue(Configuration.EnableMageSkills)) ArtificerUnlockables(); } catch { Log.LogError("Failed to load Artificer achievements"); }
            //Bandit
            try { if (Configuration.GetConfigValue(Configuration.EnableBanditSkills)) BanditUnlockables(); } catch { Log.LogError("Failed to load Bandit achievements"); }
            //Captain
            try { if (Configuration.GetConfigValue(Configuration.EnableCaptainSkills)) CaptainUnlockables(); } catch { Log.LogError("Failed to load Captain achivements"); }
            //Commando
            try { if (Configuration.GetConfigValue(Configuration.EnableCommandoSkills)) CommandoUnlockables(); } catch { Log.LogError("Failed to load Commando achievements"); }
            //Engineer
            try { if (Configuration.GetConfigValue(Configuration.EnableEngiSkills)) EngiUnlockables(); } catch { Log.LogError("Failed to load Engineer achievements"); }
            //Huntress
            try { if (Configuration.GetConfigValue(Configuration.EnableHuntressSkills)) HuntressUnlockables(); } catch { Log.LogError("Failed to load Huntress achievements"); }
            //Loader
            try { if (Configuration.GetConfigValue(Configuration.EnableLoaderSkills)) LoaderUnlockables(); } catch { Log.LogError("Failed to load Loader achievements"); }
            //Mercenary
            try { if (Configuration.GetConfigValue(Configuration.EnableMercSkills)) MercUnlockables(); } catch { Log.LogError("Failed to load Mercenary achievements"); }
            //Mult
            try { if (Configuration.GetConfigValue(Configuration.EnableToolbotSkills)) MulTUnlockables(); } catch { Log.LogError("Failed to load MUL-T achievements"); }
            //Railgunner
            try { if (Configuration.GetConfigValue(Configuration.EnableRailgunnerSkills)) RailgunnerUnlockables(); } catch { Log.LogError("Failed to load Railgunner achievements"); }
            //Rex
            try { if (Configuration.GetConfigValue(Configuration.EnableTreebotSkills)) RexUnlockables(); } catch { Log.LogError("Failed to load REX achievements"); }
            //Voidfiend
            try { if (Configuration.GetConfigValue(Configuration.EnableVoidfiendSkills)) VoidsurvivorUnlockables(); } catch { Log.LogError("Failed to load Void Fiend achievements"); }
            //Again, do this in case things change saves pain and agony
            foreach(UnlockableDef def in unlockList)
            {
                ContentAddition.AddUnlockableDef(def);
                Log.LogMessage("Unlockdef: " + def.cachedName + " registered!");
            }

            Log.LogMessage("Achievements registered");
        }

         

        internal static void AcridUnlockables()
        {
            //Expunge unlockdef
            acridPurgeUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            acridPurgeUnlockDef.achievementIcon = acridpurgeIconS;

            acridPurgeUnlockDef.nameToken = "ES_CROCO_SPECIAL_PURGE_NAME";
            acridPurgeUnlockDef.cachedName = PurgeAchievement.REWARDNAME;

            acridPurgeUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + PurgeAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + PurgeAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            acridPurgeUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + PurgeAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + PurgeAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            acridPurgeUnlockDef.sortScore = 200;

            unlockList.Add(acridPurgeUnlockDef);

            //Poison breath unlockdef
            acridPoisonbreathUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            acridPoisonbreathUnlockDef.achievementIcon = acridpoisonbreathIconS;

            acridPoisonbreathUnlockDef.nameToken = "ES_CROCO_PRIMARY_POISONBREATH_NAME";
            acridPoisonbreathUnlockDef.cachedName = PoisonbreathAchievement.REWARDNAME;

            acridPoisonbreathUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + PoisonbreathAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + PoisonbreathAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            acridPoisonbreathUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + PoisonbreathAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + PoisonbreathAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            acridPoisonbreathUnlockDef.sortScore = 200;

            unlockList.Add(acridPoisonbreathUnlockDef);
        }

        internal static void ArtificerUnlockables()
        {
            //Zapport unlockdef
            artificerZapportUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            artificerZapportUnlockDef.achievementIcon = zapportIconS;

            artificerZapportUnlockDef.nameToken = "ES_MAGE_UTILITY_ZAPPORT_NAME";
            artificerZapportUnlockDef.cachedName = ZapportAchievement.REWARDNAME;

            artificerZapportUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + ZapportAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + ZapportAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            artificerZapportUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + ZapportAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + ZapportAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            artificerZapportUnlockDef.sortScore = 200;

            unlockList.Add(artificerZapportUnlockDef);
        }

        internal static void BanditUnlockables()
        {
            //Invis Sprint unlockdef
            banditInvisSprintUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            banditInvisSprintUnlockDef.achievementIcon = invisSprintIconS;

            banditInvisSprintUnlockDef.nameToken = "ES_BANDIT2_UTILITY_INVISSPRINT_NAME";
            banditInvisSprintUnlockDef.cachedName = InvisSprintAchievement.REWARDNAME;

            banditInvisSprintUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + InvisSprintAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + InvisSprintAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            banditInvisSprintUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + InvisSprintAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + InvisSprintAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            banditInvisSprintUnlockDef.sortScore = 200;

            unlockList.Add(banditInvisSprintUnlockDef);

            //Magic Bullet unlockdef
            banditMagicBulletUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            banditMagicBulletUnlockDef.achievementIcon = magicBulletIconS;

            banditMagicBulletUnlockDef.nameToken = "ES_BANDIT2_PRIMARY_MAGICBULLET_NAME";
            banditMagicBulletUnlockDef.cachedName = MagicBulletAchievement.REWARDNAME;

            banditMagicBulletUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + MagicBulletAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + MagicBulletAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            banditMagicBulletUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + MagicBulletAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + MagicBulletAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            banditMagicBulletUnlockDef.sortScore = 200;

            unlockList.Add(banditMagicBulletUnlockDef);
        }

        internal static void CaptainUnlockables()
        {
            //Debuffnade unlockdef
            captainDebuffnadeUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            captainDebuffnadeUnlockDef.achievementIcon = debuffNadeIconS;

            captainDebuffnadeUnlockDef.nameToken = "ES_CAPTAIN_SECONDARY_DEBUFFNADE_NAME";
            captainDebuffnadeUnlockDef.cachedName = DebuffnadeAchievement.REWARDNAME;

            captainDebuffnadeUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + DebuffnadeAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + DebuffnadeAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            captainDebuffnadeUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + DebuffnadeAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + DebuffnadeAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            captainDebuffnadeUnlockDef.sortScore = 200;

            unlockList.Add(captainDebuffnadeUnlockDef);

            //Autoshotgun unlockdef
            captainAutoshotgunUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            captainAutoshotgunUnlockDef.achievementIcon = autoshotgunIconS;

            captainAutoshotgunUnlockDef.nameToken = "ES_CAPTAIN_PRIMARY_AUTOSHOTGUN_NAME";
            captainAutoshotgunUnlockDef.cachedName = AutoshotgunAchievement.REWARDNAME;

            captainAutoshotgunUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
{
                Language.GetString("ACHIEVEMENT_" + AutoshotgunAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + AutoshotgunAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
}));

            captainAutoshotgunUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + AutoshotgunAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + AutoshotgunAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            captainAutoshotgunUnlockDef.sortScore = 200;

            unlockList.Add(captainAutoshotgunUnlockDef);
        }

        internal static void CommandoUnlockables()
        {
            //Combat Shotgun unlockdef
            commandoShotgunUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            commandoShotgunUnlockDef.achievementIcon = shotgunIconS;

            commandoShotgunUnlockDef.nameToken = "ES_COMMANDO_PRIMARY_COMBATSHOTGUN_NAME";
            commandoShotgunUnlockDef.cachedName = ShotgunAchievement.REWARDNAME;

            commandoShotgunUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + ShotgunAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + ShotgunAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            commandoShotgunUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + ShotgunAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + ShotgunAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            commandoShotgunUnlockDef.sortScore = 200;

            unlockList.Add(commandoShotgunUnlockDef);

            //Dash unlockdef
            commandoDashUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            commandoDashUnlockDef.achievementIcon = dashIconS;

            commandoDashUnlockDef.nameToken = "ES_COMMANDO_UTILITY_DASH_NAME";
            commandoDashUnlockDef.cachedName = DashAchievement.REWARDNAME;

            commandoDashUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + DashAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + DashAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            commandoDashUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + DashAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + DashAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            commandoDashUnlockDef.sortScore = 200;

            unlockList.Add(commandoDashUnlockDef);
        }

        internal static void EngiUnlockables()
        {
            //Teslamine unlockdef
            engiTeslaUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            engiTeslaUnlockDef.achievementIcon = teslaMineIconS;

            engiTeslaUnlockDef.nameToken = "ES_ENGI_SECONDARY_TESLAMINE_NAME";
            engiTeslaUnlockDef.cachedName = TeslaMineAchievement.REWARDNAME;

            engiTeslaUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + TeslaMineAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + TeslaMineAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            engiTeslaUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + TeslaMineAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + TeslaMineAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            engiTeslaUnlockDef.sortScore = 200;

            unlockList.Add(engiTeslaUnlockDef);

            //Micromissile unlockdef
            engiMicromissileUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            engiMicromissileUnlockDef.achievementIcon = micromissileIconS;

            engiMicromissileUnlockDef.nameToken = "ES_ENGI_PRIMARY_MICROMISSILE_NAME";
            engiMicromissileUnlockDef.cachedName = MicromissileAchievement.REWARDNAME;

            engiMicromissileUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + MicromissileAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + MicromissileAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            engiMicromissileUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + MicromissileAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + MicromissileAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            engiMicromissileUnlockDef.sortScore = 200;

            unlockList.Add(engiMicromissileUnlockDef);
        }

        internal static void HuntressUnlockables()
        {
            huntressClusterarrowUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            huntressClusterarrowUnlockDef.achievementIcon = clusterarrowIconS;

            huntressClusterarrowUnlockDef.nameToken = "ES_HUNTRESS_SECONDARY_CLUSTERARROW_NAME";
            huntressClusterarrowUnlockDef.cachedName = BombArrowAchievement.REWARDNAME;

            huntressClusterarrowUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + BombArrowAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + BombArrowAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            huntressClusterarrowUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + BombArrowAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + BombArrowAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            huntressClusterarrowUnlockDef.sortScore = 200;

            unlockList.Add(huntressClusterarrowUnlockDef);
        }

        internal static void LoaderUnlockables()
        {
            loaderShieldsplosionUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            loaderShieldsplosionUnlockDef.achievementIcon = shieldsplosionIconS;

            loaderShieldsplosionUnlockDef.nameToken = "ES_LOADER_SPECIAL_SHIELDSPLOSION_NAME";
            loaderShieldsplosionUnlockDef.cachedName = BarrierAchievement.REWARDNAME;

            loaderShieldsplosionUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + BarrierAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + BarrierAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            loaderShieldsplosionUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + BarrierAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + BarrierAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            loaderShieldsplosionUnlockDef.sortScore = 200;

            unlockList.Add(loaderShieldsplosionUnlockDef);
        }

        internal static void MercUnlockables()
        {
            mercSlashportUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            mercSlashportUnlockDef.achievementIcon = slashportIconS;

            mercSlashportUnlockDef.nameToken = "ES_MERC_SPECIAL_SLASHPORT_NAME";
            mercSlashportUnlockDef.cachedName = SlashportAchievement.REWARDNAME;

            mercSlashportUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + SlashportAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + SlashportAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            mercSlashportUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + SlashportAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + SlashportAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            mercSlashportUnlockDef.sortScore = 200;

            unlockList.Add(mercSlashportUnlockDef);
        }

        internal static void MulTUnlockables()
        {
            multNanobeaconUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            multNanobeaconUnlockDef.achievementIcon = nanoBotsIconS;

            multNanobeaconUnlockDef.nameToken = "ES_TOOLBOT_SECONDARY_NANOBOT_NAME";
            multNanobeaconUnlockDef.cachedName = NanoBotAchievement.REWARDNAME;

            multNanobeaconUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + NanoBotAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + NanoBotAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            multNanobeaconUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + NanoBotAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + NanoBotAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            multNanobeaconUnlockDef.sortScore = 200;

            unlockList.Add(multNanobeaconUnlockDef);
        }
        
        internal static void RailgunnerUnlockables()
        {
            railgunnerLanceroundsUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            railgunnerLanceroundsUnlockDef.achievementIcon = lanceroundsIconS;

            railgunnerLanceroundsUnlockDef.nameToken = "ES_RAILGUNNER_PRIMARY_LANCEROUNDS_NAME";
            railgunnerLanceroundsUnlockDef.cachedName = LanceRoundsAchievement.REWARDNAME;

            railgunnerLanceroundsUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + LanceRoundsAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + LanceRoundsAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            railgunnerLanceroundsUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + LanceRoundsAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + LanceRoundsAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            railgunnerLanceroundsUnlockDef.sortScore = 200;

            unlockList.Add(railgunnerLanceroundsUnlockDef);
        }

        internal static void RexUnlockables()
        {
            rexRootUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            rexRootUnlockDef.achievementIcon = rexrootIconS;

            rexRootUnlockDef.nameToken = "ES_TREEBOT_SPECIAL_ROOT_NAME";
            rexRootUnlockDef.cachedName = RootAchievement.REWARDNAME;

            rexRootUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + RootAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + RootAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            rexRootUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + RootAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + RootAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            rexRootUnlockDef.sortScore = 200;

            unlockList.Add(rexRootUnlockDef);
        }

        internal static void VoidsurvivorUnlockables()
        {
            voidsurvivorInversionUnlockDef = ScriptableObject.CreateInstance<UnlockableDef>();

            voidsurvivorInversionUnlockDef.achievementIcon = inversionIconS;

            voidsurvivorInversionUnlockDef.nameToken = "ES_VOIDSURVIVOR_SPECIAL_INVERSION_NAME";
            voidsurvivorInversionUnlockDef.cachedName = InversionAchievement.REWARDNAME;

            voidsurvivorInversionUnlockDef.getHowToUnlockString = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + InversionAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + InversionAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            voidsurvivorInversionUnlockDef.getUnlockedString = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
            {
                Language.GetString("ACHIEVEMENT_" + InversionAchievement.ACHNAME.ToUpper() + "_NAME"),
                Language.GetString("ACHIEVEMENT_" + InversionAchievement.ACHNAME.ToUpper() + "_DESCRIPTION")
            }));

            voidsurvivorInversionUnlockDef.sortScore = 200;

            unlockList.Add(voidsurvivorInversionUnlockDef);
        }
    }
}
