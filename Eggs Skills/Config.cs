using BepInEx;
using BepInEx.Configuration;
using EggsUtils.Helpers;
using RoR2;
using System.Linq;
using UnityEngine;
using static EggsUtils.Config.Config;

namespace EggsSkills.Config
{
    internal static class Configuration
    {
        //File
        private static ConfigFile configFile;
        //Code for config file sharing
        private static string configCode;
        //Main config options
        //Unlocks it all
        internal static ConfigEntry<bool> UnlockAll { get; private set; }
        //Needed to affect other config values, so people don't complain when they fuck they own shit up
        internal static ConfigEntry<bool> ConfigEditingAgreement { get; private set; }
        //Artificer skills active?
        internal static ConfigEntry<bool> EnableMageSkills { get; private set; }
        //Commando skills active?
        internal static ConfigEntry<bool> EnableCommandoSkills { get; private set; }
        //Mercenary skills active?
        internal static ConfigEntry<bool> EnableMercSkills { get; private set; }
        //Engineer skills active? 
        internal static ConfigEntry<bool> EnableEngiSkills { get; private set; }
        //MUL-T skills active?
        internal static ConfigEntry<bool> EnableToolbotSkills { get; private set; }
        //REX skills active?
        internal static ConfigEntry<bool> EnableTreebotSkills { get; private set; }
        //Captain skills active?
        internal static ConfigEntry<bool> EnableCaptainSkills { get; private set; }
        //Huntress skills active?
        internal static ConfigEntry<bool> EnableHuntressSkills { get; private set; }
        //Loader skills active?
        internal static ConfigEntry<bool> EnableLoaderSkills { get; private set; }
        //Acrid skills active?
        internal static ConfigEntry<bool> EnableCrocoSkills { get; private set; }
        //Bandit skills active?
        internal static ConfigEntry<bool> EnableBanditSkills { get; private set; }
        //Railgunner skills active?
        internal static ConfigEntry<bool> EnableRailgunnerSkills { get; private set; }
        //Void Fiend skills active?
        internal static ConfigEntry<bool> EnableVoidfiendSkills { get; private set; }

        //Fun value configs

        //How many bomblets
        internal static ConfigEntry<int> HuntressArrowBomblets { get; private set; }
        internal const int defaultHuntressArrowBomblets = 8;
        
        //Flechette bullet count
        internal static ConfigEntry<uint> CommandoShotgunPellets { get; private set; }
        internal const uint defaultCommandoShotgunPellets = 6u;

        //How far are enemies stunned by the slash
        internal static ConfigEntry<float> MercSlashStunrange { get; private set; }
        internal const float defaultMercSlashStunrange = 24f;

        //Pull distance
        internal static ConfigEntry<float> TreebotPullRange { get; private set; }
        internal const float defaultTreebotPullRange = 30f;

        //Should the pull be stopped by mortal limits
        internal static ConfigEntry<bool> TreebotPullSpeedcap { get; private set; }
        internal const bool defaultTreebotPullSpeedcap = true;

        //How far should loader explode
        internal static ConfigEntry<float> LoaderShieldsplodeBaseradius { get; private set; }
        internal const float defaultLoaderShieldsplodeBaseradius = 10f;
        
        //How big purge explode
        internal static ConfigEntry<float> CrocoPurgeBaseradius { get; private set; }
        internal const float defaultCrocoPurgeBaseradius = 16f;

        //Explosion radius zapport
        internal static ConfigEntry<float> MageZapportBaseradius { get; private set; }
        internal const float defaultMageZapportBaseradius = 4f;

        //Capnade radius
        internal static ConfigEntry<float> CaptainDebuffnadeRadius { get; private set; }
        internal const float defaultCaptainDebuffnadeRadius = 20f;

        //Nanobot count for MULT    
        internal static ConfigEntry<int> ToolbotNanobotCountperenemy { get; private set; }
        internal const int defaultToolbotNanobotCountperenemy = 3;

        //Bandit buff duration
        internal static ConfigEntry<float> BanditInvissprintBuffduration { get; private set; }
        internal const float defaultBanditInvissprintBuffduration = 3f;
        
        //How many times the mine pulses
        internal static ConfigEntry<int> EngiTeslaminePulses { get; private set; }
        internal const int defaultEngiTeslaminePulses = 5;

        //How many 'bounces'
        internal static ConfigEntry<int> BanditMagicbulletRicochets { get; private set; }
        internal const int defaultMagicBulletRicochets = 1;

        //Commando buff duration
        internal static ConfigEntry<float> CommandoDashBuffTimer { get; private set; }
        internal const float defaultCommandoDashBuffTimer = 1f;

        //Railgunner wall pen
        internal static ConfigEntry<bool> RailgunnerLanceWallPen { get; private set; }
        internal const bool defaultRailgunnerLanceWallPen = false;

        //Captain shotgun pellets
        internal static ConfigEntry<uint> CaptainAutoshotgunPellets { get; private set; }
        internal const uint defaultCaptainAutoshotgunPellets = 8;

        //Engi micromissile salvo count
        internal static ConfigEntry<int> EngiMicromissileSalvocount { get; private set; }
        internal const int defaultEngiMicromissileSalvocount = 0;

        //Poison Breath Angle
        internal static ConfigEntry<float> CrocoPoisonbreathAngle { get; private set; }
        internal const float defaultCrocoPoisonbreathAngle = 20f;

        //Void fiend explosion radius
        internal static ConfigEntry<float> VoidfiendInversionRadius { get; private set; }
        internal const float defaultVoidfiendInversionRadius = 28f;

        //This is where we load up all of the config values
        internal static void LoadConfig()
        {
            #region Pain
            //Important configs
            configFile = new ConfigFile(Paths.ConfigPath + "/EggsSkills.cfg", true);
            UnlockAll = configFile.Bind("!!Achievements!!", "UnlockAll", false, "Set to true to unlock all EggsSkills' achievements automatically.  Does not require ConfigEditingAgreement to be true to function");
            ConfigEditingAgreement = configFile.Bind("!!!Read this!!!", "ConfigEditingAgreement", false, "By setting this to true, you as the EggsSkills user agrees to not complain about bugs that may stem from mismatched configs.  Before submitting a bug report, delete the config file (It will automatically be regenerated at default values), restart the game, and attempt to recreate the bug first.  Config values will automatically be applied as default unless this is set to true");
            EnableMageSkills = configFile.Bind("!EnabledSkills!", "EnableArtificerSkills", true, "Set to false to prevent EggsSkills' Artificer skills from listing");
            EnableBanditSkills = configFile.Bind("!EnabledSkills!", "EnableBanditSkills", true, "Set to false to prevent EggsSkills' Bandit skills from listing");
            EnableMercSkills = configFile.Bind("!EnabledSkills!", "EnableMercSkills", true, "Set to false to prevent EggsSkills' Mercenary skills from listing");
            EnableEngiSkills = configFile.Bind("!EnabledSkills!", "EnableEngiSkills", true, "Set to false to prevent EggsSkills' Engineer skills from listing");
            EnableLoaderSkills = configFile.Bind("!EnabledSkills!", "EnableLoaderSkills", true, "Set to false to prevent EggsSkills' Loader skills from listing");
            EnableCommandoSkills = configFile.Bind("!EnabledSkills!", "EnableCommandoSkills", true, "Set to false to prevent EggsSkills' Commando skills from listing");
            EnableCrocoSkills = configFile.Bind("!EnabledSkills!", "EnableCrocoSkills", true, "Set to false to prevent EggsSkills' Acrid skills from listing");
            EnableHuntressSkills = configFile.Bind("!EnabledSkills!", "EnableHuntressSkills", true, "Set to false to prevent EggsSkills' Huntress skills from listing");
            EnableToolbotSkills = configFile.Bind("!EnabledSkills!", "EnableMULTSkills", true, "Set to false to prevent EggsSkills' MUL-T skills from listing");
            EnableTreebotSkills = configFile.Bind("!EnabledSkills!", "EnableREXSkills", true, "Set to false to prevent EggsSkills' REX skills from listing");
            EnableCaptainSkills = configFile.Bind("!EnabledSkills!", "EnableCaptainSkills", true, "Set to false to prevent EggsSkills' Captain skills from listing");
            EnableRailgunnerSkills = configFile.Bind("!EnabledSkills!", "EnableRailgunnerSkills", true, "Set to false to prevent EggsSkills' Railgunner skills from listing");
            EnableVoidfiendSkills = configFile.Bind("!EnabledSkills!", "EnableVoidFiendSkills", true, "Set to false to prevent EggsSkills' Void Fiend skills from listing");
            //Fun configs
            HuntressArrowBomblets = configFile.Bind("HuntressConfigs", "ArrowBomblets", defaultHuntressArrowBomblets, "How many bomblets are released from Huntress' Bomb Arrow.  Crits always release 1.5x this amount");
            CommandoShotgunPellets = configFile.Bind("CommandoConfigs", "ShotgunPellets", defaultCommandoShotgunPellets, "How many bullets are fired per shot from Commando's Flechette Rounds");
            BanditInvissprintBuffduration = configFile.Bind("BanditConfigs", "InvisSprintBuffDuration", defaultBanditInvissprintBuffduration, "How long the damage / speed buff from Bandit's Kinetic Refractor lasts");
            MageZapportBaseradius = configFile.Bind("ArtificerConfigs", "ZapportBaseRadius", defaultMageZapportBaseradius, "Radius of a minimum charge cast of Artificer's Quantum Transposition");
            CrocoPurgeBaseradius = configFile.Bind("AcridConfigs", "PurgeRadius", defaultCrocoPurgeBaseradius, "Radius that Acrid's Expunge will affect upon detonation");
            LoaderShieldsplodeBaseradius = configFile.Bind("LoaderConfigs", "ShieldsplodeRadius", defaultLoaderShieldsplodeBaseradius, "Radius of a minimum (10%) barrier cast of Loader's Barrier Buster");
            EngiTeslaminePulses = configFile.Bind("EngiConfigs", "TeslaminePulseCount", defaultEngiTeslaminePulses, "How many times should Engineer's Shock Mines deal damage before dissapearing");
            CaptainDebuffnadeRadius = configFile.Bind("CaptainConfigs", "DebuffnadeRadius", defaultCaptainDebuffnadeRadius, "Radius of Captain's Tracking Grenade explosion");
            MercSlashStunrange = configFile.Bind("MercConfigs", "SlashportHealthFraction", defaultMercSlashStunrange, "What is the range on merc stunning enemies with Execute");
            TreebotPullRange = configFile.Bind("REXConfigs", "PullRange", defaultTreebotPullRange, "Radius at which enemies should be affected by REX's DIRECTIVE: Respire");
            TreebotPullSpeedcap = configFile.Bind("REXConfigs", "PullSpeedCap", defaultTreebotPullSpeedcap, "Should REX's DIRECTIVE: Respire be capped in how fast it can pulse");
            ToolbotNanobotCountperenemy = configFile.Bind("MULTConfigs", "NanobotCount", defaultToolbotNanobotCountperenemy, "How many nanobot swarms should MUL-T's Nanobot Swarm ability fire out per enemy");
            BanditMagicbulletRicochets = configFile.Bind("BanditConfigs", "MagicBulletRicochet", defaultMagicBulletRicochets, "What is the max amount of times Bandit's Bounce should ricochet to new targets");
            CommandoDashBuffTimer = configFile.Bind("CommandoConfigs", "DashBuffTimer", defaultCommandoDashBuffTimer, "How long should the Commando's Tactical Pursuit post-dash invulnerability last");
            RailgunnerLanceWallPen = configFile.Bind("RailgunnerConfigs", "LanceWallPenetration", defaultRailgunnerLanceWallPen, "Should Railgunner's Lancer Rounds pierce everything");
            CaptainAutoshotgunPellets = configFile.Bind("CaptainConfigs", "AutoshotgunPellets", defaultCaptainAutoshotgunPellets, "How many pellets per shot from Captain's Mercury Repeater");
            EngiMicromissileSalvocount = configFile.Bind("EngiConfigs", "BonusMicromissileSalvoCount", defaultEngiMicromissileSalvocount, "How many extra micromissiles should be fired per turret per salvo from Engineer's Guided Salvo ability?");
            CrocoPoisonbreathAngle = configFile.Bind("AcridConfigs", "PoisonBreathAngle", defaultCrocoPoisonbreathAngle, "What should the angle of the cone for hitting enemies for Acrid's Caustic Spray be?");
            VoidfiendInversionRadius = configFile.Bind("VoidFiendConfigs", "ExplosionRadius", defaultVoidfiendInversionRadius, "Radius of Void Fiend's ");
            #endregion

            //If they fugged with the file note it
            if (ConfigEditingAgreement.Value) Log.LogMessage("Config file has been edited");
            //Also if they didn't
            else Log.LogMessage("Config not changed, no values applied");

            //Rev up their code
            configCode = PrepareConfigCode(configFile);
            
            //Tell them they code and how to get it if they want it
            Log.LogMessage("Config code is : " + configCode + ".  Type 'es_getconfig' in console to copy it to clipboard.");
        }

        //This is how we get a config value, or default if they have not marked the 'I may be messing things up' box
        internal static T GetConfigValue<T>(ConfigEntry<T> config)
        {
            return (T) (ConfigEditingAgreement.Value ? config.Value : config.DefaultValue);
        }

        //This is run when they do es_getconfig
        internal static void GetConfigCode()
        {
            //Rev up that code
            configCode = PrepareConfigCode(configFile);
            //Save it to clipboard
            GUIUtility.systemCopyBuffer = configCode;
            //Tell them it on they clipboard
            Log.LogMessage(configCode + " Copied to clipboard");
        }

        //Simple command to get your on config value
        [ConCommand(commandName = "es_getconfig", flags = ConVarFlags.None, helpText = "Get your EggsSkills config setting as a shareable string.")]
        private static void CCGetEggSkillsConfig(ConCommandArgs args)
        {
            //Run the method for doing that thing
            GetConfigCode();
        }

        //Simple command for setting your config value
        [ConCommand(commandName = "es_setconfig", flags = ConVarFlags.None, helpText ="Sets all of your EggsSkills config values to the one indicated by the config code.  If none specified, will take value from clipboard.  Case sensitive.  Must restart game to take effect.")]
        private static void CCSetEggSkillsConfig(ConCommandArgs args)
        {
            //Start with empty code
            string code = string.Empty;
            //If there are too many args, tell them
            if (args.Count > 1) Log.LogWarning("Invald args");
            //If the right amount of args, get the string they give us
            else if(args.Count == 1) code = args.TryGetArgString(0);
            //If 0 args, grab it from their clipboard
            else code = GUIUtility.systemCopyBuffer;
            //Minor polish so spaces don't implode the system
            code.Trim();
            //For every char in the code
            foreach (char _ in code)
            {
                //If it isn't supposed to be there
                if (!Conversions.CHARS.Contains(_))
                {
                    //Yell at them and gtfo
                    Log.LogWarning("Invalid character : " + _);
                    return;
                }
            }
            //If it all works fine try to set the config with the code
            LoadConfigCode(code, ref configFile);
        }
    }
}
