using BepInEx.Configuration;
using BepInEx;
using UnityEngine;
using System;
using System.Linq;
using RoR2;
using System.Collections.Generic;
using EggsUtils.Helpers;

namespace EggsSkills.Config
{
    internal static class Configuration
    {
        private static ConfigFile configFile;
        private static string configCode;
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
        internal static ConfigEntry<int> BanditMagicbulletLuckmod { get; private set; }
        internal const int defaultMagicBulletLuckMod = 1;
        internal static ConfigEntry<int> BanditMagicbulletRicochets { get; private set; }
        internal const int defaultMagicBulletRicochets = 1;
        internal static ConfigEntry<float> CommandoDashBuffTimer { get; private set; }
        internal const float defaultCommandoDashBuffTimer = 1f;
        internal static void LoadConfig()
        {
            //Important configs
            configFile = new ConfigFile(Paths.ConfigPath + "/EggsSkills.cfg", true);
            UnlockAll = configFile.Bind("Achievements", "UnlockAll", false, "Set to true to unlock all EggsSkills' achievements automatically.  Does not require ConfigEditingAgreement to be true to function");
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
            ToolbotNanobotCountperenemy = configFile.Bind("MULTConfigs", "NanobotCount", defaultToolbotNanobotCountperenemy, "How many nanobot swarms should MUL-T's Nanobot Swarm ability fire out per enemy");
            BanditMagicbulletLuckmod = configFile.Bind("BanditConfigs", "MagicBulletLuck", defaultMagicBulletLuckMod, "How much luck should be applied on critical strike from Bandit's Magic Bullet");
            BanditMagicbulletRicochets = configFile.Bind("BanditConfigs", "MagicBulletRicochet", defaultMagicBulletRicochets, "What is the max amount of times Bandit's Magic Bullet should ricochet to new targets");
            CommandoDashBuffTimer = configFile.Bind("CommandoConfigs", "DashBuffTimer", defaultCommandoDashBuffTimer, "How long should the Commando's Tactical Pursuit post-dash invulnerability last");
            if (ConfigEditingAgreement.Value)
            {
                EggsUtils.EggsUtils.LogToConsole("Config file has been edited");
            }
            else
            {
                EggsUtils.EggsUtils.LogToConsole("Config not changed, no values applied");
            };
            PrepareConfigCode();
            EggsUtils.EggsUtils.LogToConsole("Config code is : " + configCode + ".  Type 'es_getconfig' in console to copy it to clipboard.");
        }
        internal static T GetConfigValue<T>(ConfigEntry<T> config)
        {
            return (T) (ConfigEditingAgreement.Value ? config.Value : config.DefaultValue);
        }
        private static void PrepareConfigCode()
        {
            configCode = "";
            int defaultCounter = 0;
            foreach (KeyValuePair<ConfigDefinition, ConfigEntryBase> config in configFile)
            {
                ConfigEntryBase value = config.Value;
                string tempValue = "";
                string tempType;
                if (value.BoxedValue.Equals(value.DefaultValue))
                {
                    defaultCounter += 1;
                    if(configFile.Last().Equals(config))
                    {
                        tempType = defaultCounter.ToString();
                    }
                    else
                    {
                        tempType = "";
                    }
                }
                else
                {
                    if (defaultCounter > 0)
                    {
                        configCode += defaultCounter.ToString();
                        defaultCounter = 0;
                    }
                    tempType = value.BoxedValue.GetType().Name[0].ToString().ToLower();
                    if (tempType == "b")
                    {
                        tempValue = (bool)value.BoxedValue ? "1" : "0";
                    }
                    else if (tempType == "s")
                    {
                        float floatValue = (float)value.BoxedValue;
                        if (floatValue > 3843)
                        {
                            Debug.LogError("Value of config field : " + value.Definition.Key + " too high, resetting to default)");
                            value.BoxedValue = value.DefaultValue;
                        }
                        else if (floatValue < 0)
                        {
                            Debug.LogError("Value of config field : " + value.Definition.Key + " below zero, resetting to default)");
                            value.BoxedValue = value.DefaultValue;
                        }
                        else
                        {
                            
                            int whole = Convert.ToInt32(System.Math.Floor(floatValue));
                            tempValue = Conversions.ToBase62(whole);
                            int dec = Convert.ToInt32((floatValue - whole) * 100f);
                            value.BoxedValue = (float)whole + Convert.ToSingle(System.Math.Round(dec / 100f, 2));
                            tempValue += Conversions.ToBase62(dec);
                        }
                    }
                    else if (tempType == "i")
                    {
                        int intValue = (int)value.BoxedValue;
                        if (intValue > 3843)
                        {
                            Debug.LogError("Value of config field : " + value.Definition.Key + " too high, resetting to default)");
                            value.BoxedValue = value.DefaultValue;
                        }
                        else if (intValue < 0)
                        {
                            Debug.LogError("Value of config field : " + value.Definition.Key + " below zero, resetting to default)");
                            value.BoxedValue = value.DefaultValue;
                        }
                        else
                        {
                            tempValue = Conversions.ToBase62(intValue);
                        }
                    }
                    else if (tempType == "u")
                    {
                        uint uintValue = (uint)value.BoxedValue;
                        if (uintValue > 3843)
                        {
                            Debug.LogError("Value of config field : " + value.Definition.Key + " too high, resetting to default)");
                            value.BoxedValue = value.DefaultValue;
                        }
                        else if (uintValue < 0)
                        {
                            Debug.LogError("Value of config field : " + value.Definition.Key + " below zero, resetting to default)");
                            value.BoxedValue = value.DefaultValue;
                        }
                        else
                        {
                            tempValue = Conversions.ToBase62(Convert.ToInt32(uintValue));
                        }
                    }
                }
                configCode += tempType + tempValue;
            }
            configFile.Reload();
        }

        internal static void GetConfigCode()
        {
            PrepareConfigCode();
            GUIUtility.systemCopyBuffer = configCode;
            EggsUtils.EggsUtils.LogToConsole(configCode + " Copied to clipboard");
        }
        internal static void LoadConfigCode(string code)
        {
            int length = code.Length;
            int pointer = 0;
            int section = 0;
            int numDefaults = 0;
            foreach (KeyValuePair<ConfigDefinition, ConfigEntryBase> config in configFile)
            {
                ConfigEntryBase configEntry = config.Value;
                char pointed = code[pointer];
                int num;
                bool isLast = false;
                if (numDefaults == 0)
                {
                    while (int.TryParse(code.Substring(pointer, section + 1), out num))
                    {
                        section += 1;
                        numDefaults = num;
                        if(pointer + section >= code.Length)
                        {
                            isLast = true;
                            break;
                        }
                    }
                    if(section > 0)
                    {
                        pointer += section - (isLast ? 1 : 0);
                        section = 0;
                    }
                }
                if (numDefaults > 0)
                {
                    numDefaults -= 1;
                    configEntry.BoxedValue = configEntry.DefaultValue;
                    continue;
                }
                pointer += 1;
                if (pointed == Convert.ToChar("b"))
                {
                    section = 1;
                    configEntry.BoxedValue = code.Substring(pointer, section) == "1";
                }
                else if (pointed == Convert.ToChar("s"))
                {
                    section = 2;
                    int whole = Conversions.FromBase62(code.Substring(pointer, section));
                    int dec = Conversions.FromBase62(code.Substring(pointer + section, section));
                    float convertedValue = whole + (dec / 100f);
                    configEntry.BoxedValue = convertedValue;
                    section += 2;
                }
                else if (pointed == Convert.ToChar("i"))
                {
                    section = 2;
                    configEntry.BoxedValue = Conversions.FromBase62(code.Substring(pointer, section));
                }
                else if (pointed == Convert.ToChar("u"))
                {
                    section = 2;
                    uint tempVal = (uint) Conversions.FromBase62(code.Substring(pointer, section));
                    configEntry.BoxedValue = tempVal;
                }
                else
                {
                    Debug.LogError("Invalid code section, process aborted");
                    return;
                }
                pointer += section;
                section = 0;
            }
            EggsUtils.EggsUtils.LogToConsole("Config code loaded, restart game for it to take effect");
            configFile.Save();
            configFile.Reload();
        }


        [ConCommand(commandName = "es_getconfig", flags = ConVarFlags.None, helpText = "Get your EggsSkills config setting as a shareable string.")]
        private static void CCGetEggSkillsConfig(ConCommandArgs args)
        {
            GetConfigCode();
        }
        [ConCommand(commandName = "es_setconfig", flags = ConVarFlags.None, helpText ="Sets all of your EggsSkills config values to the one indicated by the config code.  If none specified, will take value from clipboard.  Case sensitive.  Must restart game to take effect.")]
        private static void CCSetEggSkillsConfig(ConCommandArgs args)
        {
            string code = "";
            if (args.Count > 1)
            {
                Debug.LogError("Invald args");
            }
            else if(args.Count == 1)
            {
                code = args.TryGetArgString(0);
            }
            else
            {
                code = GUIUtility.systemCopyBuffer;
            }
            code.Trim();
            foreach (char _ in code)
            {
                if (!Conversions.chars.Contains(_))
                {
                    Debug.LogError("Invalid character : " + _);
                    return;
                }
            }
            LoadConfigCode(code);
        }
    }
}
