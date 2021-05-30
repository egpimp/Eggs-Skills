using EggsSkills.Config;
using RoR2;
using EggsSkills.Achievements;

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
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableEngiSkills.Value : true)
            {
                EngiUnlockables();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableMageSkills.Value : true)
            {
                ArtificerUnlockables();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableLoaderSkills.Value : true)
            {
                LoaderUnlockables();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableCommandoSkills.Value : true)
            {
                CommandoUnlockables();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableHuntressSkills.Value : true)
            {
                HuntressUnlockables();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableBanditSkills.Value : true)
            {
                BanditUnlockables();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableToolbotSkills.Value : true)
            {
                MulTUnlockables();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableMercSkills.Value : true)
            {
                MercUnlockables();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableTreebotSkills.Value : true)
            {
                RexUnlockables();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableCrocoSkills.Value : true)
            {
                AcridUnlockables();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableCaptainSkills.Value : true)
            {
                CaptainUnlockables();
            }
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
