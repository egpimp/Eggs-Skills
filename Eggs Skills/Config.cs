using BepInEx.Configuration;

namespace EggsSkills.Config
{
    internal static class Configuration
    {
        private static ConfigFile configFile;

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

        internal static void LoadConfig()
        {
            configFile = new ConfigFile("BepInEx/config/EggsSkills.cfg", true);
            ConfigEditingAgreement = configFile.Bind("Read this", "ConfigEditingAgreement", false, "By setting this to true, you as the EggsSkills user agrees to not complain about bugs that may stem from mismatched configs.  You cannot change config values without this being set to true.");
            EnableMageSkills = configFile.Bind("EnabledSkills", "EnableMageSkills", true, "Set to false to prevent EggsSkills' Artificer skills from listing");
            EnableBanditSkills = configFile.Bind("EnabledSkills", "EnableBanditSkills", true, "Set to false to prevent EggsSkills' Bandit skills from listing");
            EnableMercSkills = configFile.Bind("EnabledSkills", "EnableMercSkills", true, "Set to false to prevent EggsSkills' Mercenary skills from listing");
            EnableEngiSkills = configFile.Bind("EnabledSkills", "EnableEngiSkills", true, "Set to false to prevent EggsSkills' Engineer skills from listing");
            EnableLoaderSkills = configFile.Bind("EnabledSkills", "EnableLoaderSkills", true, "Set to false to prevent EggsSkills' Loader skills from listing");
            EnableCommandoSkills = configFile.Bind("EnabledSkills", "EnableCommandoSkills", true, "Set to false to prevent EggsSkills' Commando skills from listing");
            EnableCrocoSkills = configFile.Bind("EnabledSkills", "EnableCrocoSkills", true, "Set to false to prevent EggsSkills' Acrid skills from listing");
            EnableHuntressSkills = configFile.Bind("EnabledSkills", "EnableHuntressSkills", true, "Set to false to prevent EggsSkills' Huntress skills from listing");
            EnableToolbotSkills = configFile.Bind("EnabledSkills", "EnableToolbotSkills", true, "Set to false to prevent EggsSkills' MUL-T skills from listing");
            EnableTreebotSkills = configFile.Bind("EnabledSkills", "EnableTreebotSkills", true, "Set to false to prevent EggsSkills' REX skills from listing");
            EnableCaptainSkills = configFile.Bind("EnabledSkills", "EnableCaptainSkills", true, "Set to false to prevent EggsSkills' Captain skills from listing");
        }
    }
}
