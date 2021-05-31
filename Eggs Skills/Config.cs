using BepInEx.Configuration;
using BepInEx;
using HG;

namespace EggsSkills.Config
{
    internal static class Configuration
    {
        private static ConfigFile configFile;

        //Main config options
        internal static ConfigEntry<bool> UnlockAll { get; private set; }
        internal static ConfigEntry<bool> ConfigEditingAgreement { get; private set; }
        internal static ConfigEntry<bool> EnableMageSkills { get; private set; }
        internal static ConfigEntry<bool> EnableCommandoSkills { get; private set; }
        internal static ConfigEntry<bool> EnableMercSkills { get; private set; }
        internal static ConfigEntry<bool> EnableEngiSkills { get; private set; }
        internal static ConfigEntry<bool> EnableToolbotSkills { get; private set; }
        internal static ConfigEntry<bool> EnableTreebotSkills { get; private set; }
        internal static ConfigEntry<bool> EnableCaptainSkills { get; private set; }
        internal static ConfigEntry<bool> EnableHuntressSkills { get; private set; }
        internal static ConfigEntry<bool> EnableLoaderSkills { get; private set; }
        internal static ConfigEntry<bool> EnableCrocoSkills { get; private set; }
        internal static ConfigEntry<bool> EnableBanditSkills { get; private set; }

        //Fun value configs
        internal static ConfigEntry<int> HuntressArrowBomblets { get; private set; }
        internal const int defaultHuntressArrowBomblets = 8;
        internal static ConfigEntry<uint> CommandoShotgunPellets { get; private set; }
        internal const uint defaultCommandoShotgunPellets = 6u;
        internal static ConfigEntry<float> MercSlashHealthfraction { get; private set; }
        internal const float defaultMercSlashHealthfraction = 0.2f;
        internal static ConfigEntry<float> TreebotPullRange { get; private set; }
        internal const float defaultTreebotPullRange = 30f;
        internal static ConfigEntry<bool> TreebotPullSpeedcap { get; private set; }
        internal const bool defaultTreebotPullSpeedcap = true;
        internal static ConfigEntry<float> LoaderShieldsplodeBaseradius { get; private set; }
        internal const float defaultLoaderShieldsplodeBaseradius = 10f;
        internal static ConfigEntry<bool> LoaderShieldsplodeRemovebarrieronuse { get; private set; }
        internal const bool defaultLoaderShieldsplodeRemovebarrieronuse = true;
        internal static ConfigEntry<float> CrocoPurgeBaseradius { get; private set; }
        internal const float defaultCrocoPurgeBaseradius = 16f;
        internal static ConfigEntry<float> MageZapportBaseradius { get; private set; }
        internal const float defaultMageZapportBaseradius = 3f;
        internal static ConfigEntry<float> CaptainDebuffnadeRadius { get; private set; }
        internal const float defaultCaptainDebuffnadeRadius = 20f;
        internal static ConfigEntry<int> ToolbotNanobotCountperenemy { get; private set; }
        internal const int defaultToolbotNanobotCountperenemy = 3;
        internal static ConfigEntry<float> BanditInvissprintBuffduration { get; private set; }
        internal const float defaultBanditInvissprintBuffduration = 3f;
        internal static ConfigEntry<int> EngiTeslaminePulses { get; private set; }
        internal const int defaultEngiTeslaminePulses = 5;
        internal static void LoadConfig()
        {
            //Important configs
            configFile = new ConfigFile(Paths.ConfigPath + "/EggsSkills.cfg", true);
            UnlockAll = configFile.Bind("Achievements", "UnlockAll", false, "Set to true to unlock all EggsSkills' achievements automatically.  Does not require ConfigEditingAgreement to be true to function.");
            ConfigEditingAgreement = configFile.Bind("!Read this!", "ConfigEditingAgreement", false, "By setting this to true, you as the EggsSkills user agrees to not complain about bugs that may stem from mismatched configs.  Config values will automatically be applied as default unless this is set to true");
            EnableMageSkills = configFile.Bind("EnabledSkills", "EnableArtificerSkills", true, "Set to false to prevent EggsSkills' Artificer skills from listing");
            EnableBanditSkills = configFile.Bind("EnabledSkills", "EnableBanditSkills", true, "Set to false to prevent EggsSkills' Bandit skills from listing");
            EnableMercSkills = configFile.Bind("EnabledSkills", "EnableMercSkills", true, "Set to false to prevent EggsSkills' Mercenary skills from listing");
            EnableEngiSkills = configFile.Bind("EnabledSkills", "EnableEngiSkills", true, "Set to false to prevent EggsSkills' Engineer skills from listing");
            EnableLoaderSkills = configFile.Bind("EnabledSkills", "EnableLoaderSkills", true, "Set to false to prevent EggsSkills' Loader skills from listing");
            EnableCommandoSkills = configFile.Bind("EnabledSkills", "EnableCommandoSkills", true, "Set to false to prevent EggsSkills' Commando skills from listing");
            EnableCrocoSkills = configFile.Bind("EnabledSkills", "EnableCrocoSkills", true, "Set to false to prevent EggsSkills' Acrid skills from listing");
            EnableHuntressSkills = configFile.Bind("EnabledSkills", "EnableHuntressSkills", true, "Set to false to prevent EggsSkills' Huntress skills from listing");
            EnableToolbotSkills = configFile.Bind("EnabledSkills", "EnableMULTSkills", true, "Set to false to prevent EggsSkills' MUL-T skills from listing");
            EnableTreebotSkills = configFile.Bind("EnabledSkills", "EnableREXSkills", true, "Set to false to prevent EggsSkills' REX skills from listing");
            EnableCaptainSkills = configFile.Bind("EnabledSkills", "EnableCaptainSkills", true, "Set to false to prevent EggsSkills' Captain skills from listing");
            //Fun configs
            HuntressArrowBomblets = configFile.Bind("HuntressConfigs", "ArrowBomblets", defaultHuntressArrowBomblets, "How many bomblets are released from Huntress' Bomb Arrow.  Crits always release 1.5x this amount. (Egg is not responsible for CPU / GPU melting)");
            CommandoShotgunPellets = configFile.Bind("CommandoConfigs", "ShotgunPellets", defaultCommandoShotgunPellets, "How many bullets are fired per shot from Commando's Flechette Rounds");
            BanditInvissprintBuffduration = configFile.Bind("BanditConfigs", "InvisSprintBuffDuration", defaultBanditInvissprintBuffduration, "How long the damage / speed buff from Bandit's Kinetic Refractor lasts");
            MageZapportBaseradius = configFile.Bind("ArtificerConfigs", "ZapportBaseRadius", defaultMageZapportBaseradius, "Radius of a minimum charge cast of Artificer's Quantum Transposition");
            CrocoPurgeBaseradius = configFile.Bind("AcridConfigs", "PurgeRadius", defaultCrocoPurgeBaseradius, "Radius that Acrid's Expunge will affect upon detonation");
            LoaderShieldsplodeBaseradius = configFile.Bind("LoaderConfigs", "ShieldsplodeRadius", defaultLoaderShieldsplodeBaseradius, "Radius of a minimum (10%) barrier cast of Loader's Barrier Buster");
            LoaderShieldsplodeRemovebarrieronuse = configFile.Bind("LoaderConfigs", "ShieldsplodeRemoveBarrierOnUse", defaultLoaderShieldsplodeRemovebarrieronuse, "Should Loader's Barrier Buster remove your barrier on use");
            EngiTeslaminePulses = configFile.Bind("EngiConfigs", "TeslaminePulseCount", defaultEngiTeslaminePulses, "How many times should Engineer's Shock Mines deal damage before dissapearing");
            CaptainDebuffnadeRadius = configFile.Bind("CaptainConfigs", "DebuffnadeRadius", defaultCaptainDebuffnadeRadius, "Radius of Captain's Tracking Grenade");
            MercSlashHealthfraction = configFile.Bind("MercConfigs", "SlashportHealthFraction", defaultMercSlashHealthfraction, "What percent of missing health damage should Mercenary's Fatal Assault deal");
            TreebotPullRange = configFile.Bind("REXConfigs", "PullRange", defaultTreebotPullRange, "Radius at which enemies should be affected by REX's DIRECTIVE: Respire");
            TreebotPullSpeedcap = configFile.Bind("REXConfigs", "PullSpeedCap", defaultTreebotPullSpeedcap, "Should REX's DIRECTIVE: Respire be capped in how fast it can pulse");
            ToolbotNanobotCountperenemy = configFile.Bind("MULTConfigs", "MULTNanobotCount", defaultToolbotNanobotCountperenemy, "How many nanobot swarms should MUL-T's Nanobot Swarm ability fire out per enemy");
        }
        internal static T GetConfigValue<T>(ConfigEntry<T> config)
        {
            return (T) (ConfigEditingAgreement.Value ? config.Value : config.DefaultValue);
        }
    }
}
