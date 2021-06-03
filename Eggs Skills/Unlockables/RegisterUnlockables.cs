using EggsSkills.Config;
using RoR2;
using EggsSkills.Achievements;
using EggsSkills.Utility;

namespace EggsSkills.Unlocks
{
    internal static class UnlocksRegistering
    {
        internal static UnlockableDef engiTeslaUnlockDef;
        internal static UnlockableDef huntressClusterarrowUnlockDef;
        internal static UnlockableDef artificerZapportUnlockDef;
        internal static UnlockableDef loaderShieldsplosionUnlockDef;
        internal static UnlockableDef commandoShotgunUnlockDef;
        internal static UnlockableDef banditInvisSprintUnlockDef;
        internal static UnlockableDef multNanobeaconUnlockDef;
        internal static UnlockableDef acridExpungeUnlockDef;
        internal static UnlockableDef mercSlashportUnlockDef;
        internal static UnlockableDef captainDebuffnadeUnlockDef;
        internal static UnlockableDef rexRootUnlockDef;

        internal static void RegisterUnlockables()
        {
            if (Configuration.GetConfigValue<bool>(Configuration.EnableEngiSkills))
            {
                EngiUnlockables();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableMageSkills))
            {
                ArtificerUnlockables();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableLoaderSkills))
            {
                LoaderUnlockables();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableCommandoSkills))
            {
                CommandoUnlockables();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableHuntressSkills))
            {
                HuntressUnlockables();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableBanditSkills))
            {
                BanditUnlockables();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableToolbotSkills))
            {
                MulTUnlockables();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableMercSkills))
            {
                MercUnlockables();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableTreebotSkills))
            {
                RexUnlockables();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableCrocoSkills))
            {
                AcridUnlockables();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableCaptainSkills))
            {
                CaptainUnlockables();
            }
            Utilities.LogToConsole("Achievements registered");
        }

        internal static void EngiUnlockables()
        {
            engiTeslaUnlockDef = Unlockables.AddUnlockable<TeslaMineAchievement>(true);
        }
        
        internal static void ArtificerUnlockables()
        {
            artificerZapportUnlockDef = Unlockables.AddUnlockable<ZapportAchievement>(true);
        }

        internal static void LoaderUnlockables()
        {
            loaderShieldsplosionUnlockDef = Unlockables.AddUnlockable<BarrierAchievement>(true);
        }

        internal static void CommandoUnlockables()
        {
            commandoShotgunUnlockDef = Unlockables.AddUnlockable<ShotgunAchievement>(true);
        }

        internal static void HuntressUnlockables()
        {
            huntressClusterarrowUnlockDef = Unlockables.AddUnlockable<BombArrowAchievement>(true);
        }

        internal static void BanditUnlockables()
        {
            banditInvisSprintUnlockDef = Unlockables.AddUnlockable<InvisSprintAchievement>(true);
        }

        internal static void MulTUnlockables()
        {
            multNanobeaconUnlockDef = Unlockables.AddUnlockable<NanoBotAchievement>(true);
        }

        internal static void MercUnlockables()
        {
            mercSlashportUnlockDef = Unlockables.AddUnlockable<SlashportAchievement>(true);
        }

        internal static void AcridUnlockables()
        {
            acridExpungeUnlockDef = Unlockables.AddUnlockable<ExpungeAchievement>(true);
        }

        internal static void CaptainUnlockables()
        {
            captainDebuffnadeUnlockDef = Unlockables.AddUnlockable<DebuffnadeAchievement>(true);
        }

        internal static void RexUnlockables()
        {
            rexRootUnlockDef = Unlockables.AddUnlockable<RootAchievement>(true);
        }
    }
}
