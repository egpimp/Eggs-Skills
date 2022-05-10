using EggsSkills.Config;
using RoR2;
using EggsSkills.Achievements;
using EggsUtils;
using R2API;

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
        internal static UnlockableDef banditMagicBulletUnlockDef;
        internal static UnlockableDef commandoDashUnlockDef;

        internal static void RegisterUnlockables()
        {
            if (Configuration.GetConfigValue(Configuration.EnableEngiSkills))
            {
                EngiUnlockables();
            }
            if (Configuration.GetConfigValue(Configuration.EnableMageSkills))
            {
                ArtificerUnlockables();
            }
            if (Configuration.GetConfigValue(Configuration.EnableLoaderSkills))
            {
                LoaderUnlockables();
            }
            if (Configuration.GetConfigValue(Configuration.EnableCommandoSkills))
            {
                CommandoUnlockables();
            }
            if (Configuration.GetConfigValue(Configuration.EnableHuntressSkills))
            {
                HuntressUnlockables();
            }
            if (Configuration.GetConfigValue(Configuration.EnableBanditSkills))
            {
                BanditUnlockables();
            }
            if (Configuration.GetConfigValue(Configuration.EnableToolbotSkills))
            {
                MulTUnlockables();
            }
            if (Configuration.GetConfigValue(Configuration.EnableMercSkills))
            {
                MercUnlockables();
            }
            if (Configuration.GetConfigValue(Configuration.EnableTreebotSkills))
            {
                RexUnlockables();
            }
            if (Configuration.GetConfigValue(Configuration.EnableCrocoSkills))
            {
                AcridUnlockables();
            }
            if (Configuration.GetConfigValue(Configuration.EnableCaptainSkills))
            {
                CaptainUnlockables();
            }
            EggsUtils.EggsUtils.LogToConsole("Achievements registered");
        }

        internal static void EngiUnlockables()
        {
            engiTeslaUnlockDef = UnlockableAPI.AddUnlockable<TeslaMineAchievement>();
        }
        
        internal static void ArtificerUnlockables()
        {
            artificerZapportUnlockDef = UnlockableAPI.AddUnlockable<ZapportAchievement>();
        }

        internal static void LoaderUnlockables()
        {
            loaderShieldsplosionUnlockDef = UnlockableAPI.AddUnlockable<BarrierAchievement>();
        }

        internal static void CommandoUnlockables()
        {
            commandoShotgunUnlockDef = UnlockableAPI.AddUnlockable<ShotgunAchievement>();
            commandoDashUnlockDef = UnlockableAPI.AddUnlockable<DashAchievement>();
        }

        internal static void HuntressUnlockables()
        {
            huntressClusterarrowUnlockDef = UnlockableAPI.AddUnlockable<BombArrowAchievement>();
        }

        internal static void BanditUnlockables()
        {
            banditInvisSprintUnlockDef = UnlockableAPI.AddUnlockable<InvisSprintAchievement>();
            banditMagicBulletUnlockDef = UnlockableAPI.AddUnlockable<MagicBulletAchievement>();
        }

        internal static void MulTUnlockables()
        {
            multNanobeaconUnlockDef = UnlockableAPI.AddUnlockable<NanoBotAchievement>();
        }

        internal static void MercUnlockables()
        {
            mercSlashportUnlockDef = UnlockableAPI.AddUnlockable<SlashportAchievement>();
        }

        internal static void AcridUnlockables()
        {
            acridExpungeUnlockDef = UnlockableAPI.AddUnlockable<ExpungeAchievement>();
        }

        internal static void CaptainUnlockables()
        {
            captainDebuffnadeUnlockDef = UnlockableAPI.AddUnlockable<DebuffnadeAchievement>();
        }

        internal static void RexUnlockables()
        {
            rexRootUnlockDef = UnlockableAPI.AddUnlockable<RootAchievement>();
        }
    }
}
