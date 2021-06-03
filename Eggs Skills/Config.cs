using BepInEx.Configuration;
using BepInEx;
using EggsSkills.Utility;
using UnityEngine;
using System;
using System.Linq;
using RoR2;
using System.Collections.Generic;

namespace EggsSkills.Config
{
    internal static class Configuration
    {
        private static string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static ConfigFile configFile;
        private static string configCode;
        private static int minCodeLength;
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
            if (ConfigEditingAgreement.Value)
            {
                Utilities.LogToConsole("Config file has been edited");
            }
            else
            {
                Utilities.LogToConsole("Config not changed, no values applied");
            };
            PrepareConfigCode();
            Utilities.LogToConsole("Config code is : " + configCode + ".  Type 'es_getconfig' in console to copy it to clipboard.");
        }
        internal static T GetConfigValue<T>(ConfigEntry<T> config)
        {
            return (T) (ConfigEditingAgreement.Value ? config.Value : config.DefaultValue);
        }
        private static void PrepareConfigCode()
        {
            minCodeLength = configFile.Count;
            configCode = "";
            foreach (KeyValuePair<ConfigDefinition, ConfigEntryBase> config in configFile)
            {
                ConfigEntryBase value = config.Value;
                string tempValue = "";
                string tempType;
                if (value.BoxedValue.Equals(value.DefaultValue))
                {
                    tempType = "d";
                }
                else
                {
                    tempType = value.BoxedValue.GetType().Name[0].ToString().ToLower();
                    if (tempType == "b")
                    {
                        tempValue = (bool)value.BoxedValue ? "01" : "00";
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
                            int whole = Convert.ToInt32(Math.Floor(floatValue));
                            tempValue = ToBase62(whole);
                            int dec = Convert.ToInt32((floatValue - whole) * 100f);
                            value.BoxedValue = (float)whole + Convert.ToSingle(Math.Round(dec / 100f, 2));
                            tempValue += ToBase62(dec);
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
                            tempValue = ToBase62(intValue);
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
                            tempValue = ToBase62(Convert.ToInt32(uintValue));
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
            Utilities.LogToConsole(configCode + " Copied to clipboard");
        }
        internal static void LoadConfigCode(string code)
        {
            
            int length = code.Length;
            int pointer = 0;
            int section = 0;
            foreach (KeyValuePair<ConfigDefinition,ConfigEntryBase> config in configFile)
            {
                ConfigEntryBase configEntry = config.Value;
                string pointed = code[pointer].ToString();
                pointer += 1;
                if (pointed == "d")
                {
                    section = 0;
                    configEntry.BoxedValue = configEntry.DefaultValue;
                }
                else if (pointed == "b")
                {
                    section = 2;
                    configEntry.BoxedValue = code.Substring(pointer, section) == "01";
                }
                else if (pointed == "s")
                {
                    section = 2;
                    int whole = FromBase62(code.Substring(pointer, section));
                    int dec = FromBase62(code.Substring(pointer + section, section));
                    float convertedValue = whole + (dec / 100f);
                    configEntry.BoxedValue = convertedValue;
                    section += 2;
                }
                else if (pointed == "i")
                {
                    section = 2;
                    configEntry.BoxedValue = FromBase62(code.Substring(pointer, section));
                }
                else if (pointed == "u")
                {
                    section = 2;
                    uint tempVal = (uint)FromBase62(code.Substring(pointer, section));
                    configEntry.BoxedValue = tempVal;
                }
                else
                {
                    Debug.LogError("Invalid code section, process aborted");
                    return;
                }
                pointer += section;
            }
            Utilities.LogToConsole("Config code loaded, restart game for it to take effect");
            configFile.Save();
            configFile.Reload();
        }

        private static string ToBase62(int num)
        {
            string str = "";
            int tempNum = num;
            while(tempNum > 0)
            {
                int val = tempNum % 62;
                tempNum /= 62;
                str = chars.ElementAt(val) + str;
            }
            while(str.Length < 2)
            {
                str = "0" + str;
            }
            return str;
        }
        private static int FromBase62(string str)
        {
            int val = 0;
            for (int i = 0; i < str.Length; i++ )
            {
                char indexedChar = str[i];
                int num;
                if(str.Contains(indexedChar))
                {
                    int base62Val = chars.IndexOf(indexedChar);
                    num = Convert.ToInt32(base62Val * (Math.Pow(62, (1 - i))));
                }
                else
                {
                    num = 0;
                }
                val += num;
            }
            return val;
        }
        [ConCommand(commandName = "ES_getconfig", flags = ConVarFlags.None, helpText = "Get your EggsSkills config setting as a shareable string.")]
        private static void CCGetEggSkillsConfig(ConCommandArgs args)
        {
            GetConfigCode();
        }
        [ConCommand(commandName = "ES_setconfig", flags = ConVarFlags.None, helpText ="Sets all of your EggsSkills config values to the one indicated by the config code.  If none specified, will take value from clipboard.  Case sensitive.  Must restart game to take effect.")]
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
            if (code.Length < minCodeLength)
            {
                Debug.LogError("Invalid code length");
                return;
            }
            foreach (char _ in code)
            {
                if (!chars.Contains(_))
                {
                    Debug.LogError("Invalid character : " + _);
                    return;
                }
            }
            LoadConfigCode(code);
        }
    }
}
