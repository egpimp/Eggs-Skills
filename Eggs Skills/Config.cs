using BepInEx;
using BepInEx.Configuration;
using EggsUtils.Helpers;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EggsUtils.EggsUtils;

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

        //Fun value configs

        //How many huntress kersplodies from the arrow
        internal static ConfigEntry<int> HuntressArrowBomblets { get; private set; }
        internal const int defaultHuntressArrowBomblets = 8;
        
        //How many commando pewpews
        internal static ConfigEntry<uint> CommandoShotgunPellets { get; private set; }
        internal const uint defaultCommandoShotgunPellets = 6u;

        //What is the missing health% damage of stabby
        internal static ConfigEntry<float> MercSlashHealthfraction { get; private set; }
        internal const float defaultMercSlashHealthfraction = 0.15f;

        //How far do the robot grab yo ass
        internal static ConfigEntry<float> TreebotPullRange { get; private set; }
        internal const float defaultTreebotPullRange = 30f;

        //Should the pull be stopped by mortal limits
        internal static ConfigEntry<bool> TreebotPullSpeedcap { get; private set; }
        internal const bool defaultTreebotPullSpeedcap = true;

        //How far should loader throw her defense out the window
        internal static ConfigEntry<float> LoaderShieldsplodeBaseradius { get; private set; }
        internal const float defaultLoaderShieldsplodeBaseradius = 10f;
        
        //Should she actually throw her defense or should it boomerang
        internal static ConfigEntry<bool> LoaderShieldsplodeRemovebarrieronuse { get; private set; }
        internal const bool defaultLoaderShieldsplodeRemovebarrieronuse = true;

        //How big do the dog explode thing
        internal static ConfigEntry<float> CrocoPurgeBaseradius { get; private set; }
        internal const float defaultCrocoPurgeBaseradius = 16f;

        //How big do the cool high tech magic shid explode thing
        internal static ConfigEntry<float> MageZapportBaseradius { get; private set; }
        internal const float defaultMageZapportBaseradius = 4f;

        //How big do captain make explode
        internal static ConfigEntry<float> CaptainDebuffnadeRadius { get; private set; }
        internal const float defaultCaptainDebuffnadeRadius = 20f;

        //How many smaller MUL-T do the MUL-T make when he make the MUL-T
        internal static ConfigEntry<int> ToolbotNanobotCountperenemy { get; private set; }
        internal const int defaultToolbotNanobotCountperenemy = 3;

        //How long do bandit hit hard 
        internal static ConfigEntry<float> BanditInvissprintBuffduration { get; private set; }
        internal const float defaultBanditInvissprintBuffduration = 3f;
        
        //How many time the mine go zap
        internal static ConfigEntry<int> EngiTeslaminePulses { get; private set; }
        internal const int defaultEngiTeslaminePulses = 5;

        //How many whoosh do the bullet
        internal static ConfigEntry<int> BanditMagicbulletRicochets { get; private set; }
        internal const int defaultMagicBulletRicochets = 1;

        //How long do commando be stronk for after whoosh
        internal static ConfigEntry<float> CommandoDashBuffTimer { get; private set; }
        internal const float defaultCommandoDashBuffTimer = 1f;

        //This is where we load up all of the config values
        internal static void LoadConfig()
        {
            #region Pain
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
            BanditMagicbulletRicochets = configFile.Bind("BanditConfigs", "MagicBulletRicochet", defaultMagicBulletRicochets, "What is the max amount of times Bandit's Magic Bullet should ricochet to new targets");
            CommandoDashBuffTimer = configFile.Bind("CommandoConfigs", "DashBuffTimer", defaultCommandoDashBuffTimer, "How long should the Commando's Tactical Pursuit post-dash invulnerability last");
            #endregion

            //If they fugged with the file note it
            if (ConfigEditingAgreement.Value) LogToConsole("Config file has been edited");
            //Also if they didn't
            else LogToConsole("Config not changed, no values applied");

            //Rev up their code
            PrepareConfigCode();
            
            //Tell them they code and how to get it if they want it
            LogToConsole("Config code is : " + configCode + ".  Type 'es_getconfig' in console to copy it to clipboard.");
        }

        //This is how we get a config value, or default if they have not marked the 'I may be messing things up' box
        internal static T GetConfigValue<T>(ConfigEntry<T> config)
        {
            return (T) (ConfigEditingAgreement.Value ? config.Value : config.DefaultValue);
        }

        //Makes the config code from all of the config values
        private static void PrepareConfigCode()
        {
            //Start with blank string, this will slowly become the config code
            configCode = string.Empty;
            //How many default values we find in a row, this helps us to simply skip over sections where all values are default
            int defaultCounter = 0;
            //For every value in the file...
            foreach (KeyValuePair<ConfigDefinition, ConfigEntryBase> config in configFile)
            {
                //This just is easier reference for us
                ConfigEntryBase value = config.Value;
                //Start with another blank string
                string tempValue = string.Empty;
                //This is the 'type', since we can have bools, floats, ints, and we need to know which one so we convert it properly
                string tempType;
                //If the value is just the default value...
                if (value.BoxedValue.Equals(value.DefaultValue))
                {
                    //Note it
                    defaultCounter += 1;
                    //This just exits if the last value was a default safely, basically prevents things from exploding
                    if(configFile.Last().Equals(config)) tempType = defaultCounter.ToString();
                    //We do this so nothing is added until AFTER all the defaults are found and marked
                    else tempType = string.Empty;
                }
                //Otherwise if the value is NOT the default value...
                else
                {
                    //If there are any default values queue'd up to be marked...
                    if (defaultCounter > 0)
                    {
                        //Put the number where it goes
                        configCode += defaultCounter.ToString();
                        //Reset the counter
                        defaultCounter = 0;
                    }
                    
                    //This gets us b if bool, s if short (float), i if int, and u if uint
                    tempType = value.BoxedValue.GetType().Name[0].ToString().ToLower();

                    //If it is a bool, the string section will be b1 or b0 for true or false
                    if (tempType == "b") tempValue = (bool)value.BoxedValue ? "1" : "0";

                    //If it is a float...
                    else if (tempType == "s")
                    {
                        //Grab the whole float first off
                        float floatValue = (float)value.BoxedValue;
                        //If it's too high...
                        if (floatValue > 3843)
                        {
                            //Tell them it too high and set to default
                            LogToConsole("Value of config field : " + value.Definition.Key + " too high, resetting to default)");
                            value.BoxedValue = value.DefaultValue;
                            //Then perform the usual default stuff so we brick things
                            defaultCounter += 1;
                            if (configFile.Last().Equals(config)) tempType = defaultCounter.ToString();
                            else tempType = string.Empty;
                        }
                        //If it too low...
                        else if (floatValue < 0)
                        {
                            //Tell them too low and set to default
                            LogToConsole("Value of config field : " + value.Definition.Key + " below zero, resetting to default)");
                            value.BoxedValue = value.DefaultValue;
                            //Then perform the usual default stuff so we brick things
                            defaultCounter += 1;
                            if (configFile.Last().Equals(config)) tempType = defaultCounter.ToString();
                            else tempType = string.Empty;
                        }
                        //Otherwise we can move onto encoding it normally
                        else
                        {
                            //Grab the whole number
                            int whole = Convert.ToInt32(System.Math.Floor(floatValue));
                            //Set the value to the whole number as a base62 2 digit value
                            tempValue = Conversions.ToBase62(whole);
                            //Now we get the decimals in the back, as a 2 digit int
                            int dec = Convert.ToInt32((floatValue - whole) * 100f);
                            //Fix boxed value so it's not painfully precise anymore
                            value.BoxedValue = (float)whole + Convert.ToSingle(System.Math.Round(dec / 100f, 2));
                            //Convert the decimal part to base62 also and pass it along
                            tempValue += Conversions.ToBase62(dec);
                        }
                    }
                    //Otherwise if it is an int...
                    else if (tempType == "i")
                    {
                        //intvalue is just the value of the int duh
                        int intValue = (int)value.BoxedValue;
                        //If too big tell them and default it
                        if (intValue > 3843)
                        {
                            LogToConsole("Value of config field : " + value.Definition.Key + " too high, resetting to default)");
                            value.BoxedValue = value.DefaultValue;
                            //Then perform the usual default stuff so we brick things
                            defaultCounter += 1;
                            if (configFile.Last().Equals(config)) tempType = defaultCounter.ToString();
                            else tempType = string.Empty;
                        }
                        //If too small tell them and default it
                        else if (intValue < 0)
                        {
                            LogToConsole("Value of config field : " + value.Definition.Key + " below zero, resetting to default)");
                            value.BoxedValue = value.DefaultValue;
                            //Then perform the usual default stuff so we brick things
                            defaultCounter += 1;
                            if (configFile.Last().Equals(config)) tempType = defaultCounter.ToString();
                            else tempType = string.Empty;
                        }
                        //If value is fine set the value to it
                        else tempValue = Conversions.ToBase62(intValue);
                    }
                    //Otherwise if it is a uint...
                    else if (tempType == "u")
                    {
                        //Grab uint value
                        uint uintValue = (uint)value.BoxedValue;
                        //If too big default and say it
                        if (uintValue > 3843)
                        {
                            LogToConsole("Value of config field : " + value.Definition.Key + " too high, resetting to default)");
                            value.BoxedValue = value.DefaultValue;
                            //Then perform the usual default stuff so we brick things
                            defaultCounter += 1;
                            if (configFile.Last().Equals(config)) tempType = defaultCounter.ToString();
                            else tempType = string.Empty;
                        }
                        //If too small default and say it
                        else if (uintValue < 0)
                        {
                            LogToConsole("Value of config field : " + value.Definition.Key + " below zero, resetting to default)");
                            value.BoxedValue = value.DefaultValue;
                            //Then perform the usual default stuff so we brick things
                            defaultCounter += 1;
                            if (configFile.Last().Equals(config)) tempType = defaultCounter.ToString();
                            else tempType = string.Empty;
                        }
                        //If it works then put it in value
                        else tempValue = Conversions.ToBase62(Convert.ToInt32(uintValue));
                    }
                }
                //Finally add the type + value to the code, loop until end and boom done
                configCode += tempType + tempValue;
            }
            //Reload the file for safety
            configFile.Reload();
        }
        
        //This is run when they do es_getconfig
        internal static void GetConfigCode()
        {
            //Rev up that code
            PrepareConfigCode();
            //Save it to clipboard
            GUIUtility.systemCopyBuffer = configCode;
            //Tell them it on they clipboard
            LogToConsole(configCode + " Copied to clipboard");
        }

        //Turns string code into actual values, AKA pure agony
        internal static void LoadConfigCode(string code)
        {
            //This is the length of the code
            int length = code.Length;
            //Pointer tells us where we are at
            int pointer = 0;
            //Section helps us identify where tf we at in the code
            int section = 0;
            //How many defaults in a row we hittin
            int numDefaults = 0;
            //For every config value, should be in same order as person who did the getconfig
            foreach (KeyValuePair<ConfigDefinition, ConfigEntryBase> config in configFile)
            {
                //Helps us reference it easier
                ConfigEntryBase configEntry = config.Value;
                //Pointed is the character we are pointed at rn
                char pointed = code[pointer];
                //Num helps us identify default values
                int num;
                //If we are dangerously close to indexoutofbounds this will helps us not be that
                bool isLast = false;
                //If we haven't hit any defaults yet, check for them
                if (numDefaults == 0)
                {
                    //Keep scanning through basically until we are no longer viewing a number, so we can read off any length of number of defaults
                    while (int.TryParse(code.Substring(pointer, section + 1), out num))
                    {
                        //Section lets us look further each loop until we hit a nothing
                        section += 1;
                        //Numdefaults is just the num so we know how many defaults we just hit
                        numDefaults = num;
                        //If we are boutta go out of bounds say so and gtfo before we die
                        if(pointer + section >= code.Length)
                        {
                            isLast = true;
                            break;
                        }
                    }
                    //If we did hit any defaults...
                    if(section > 0)
                    {
                        //Mark the pointer for that length so we know how far to be for next config value
                        pointer += section - (isLast ? 1 : 0);
                        //Reset the section
                        section = 0;
                    }
                }
                //If we DID encounter defaults last step...
                if (numDefaults > 0)
                {
                    //Count down the defaults we have handled
                    numDefaults -= 1;
                    //Set the value to default
                    configEntry.BoxedValue = configEntry.DefaultValue;
                    //We ain't gotta do shit else so skip the rest of the logic
                    continue;
                }
                //Push pointer up by one beforehand so it is ready to read off the value
                pointer += 1;
                //If the string is saying we expect a bool...
                if (pointed == Convert.ToChar("b"))
                {
                    //Bools are only 1 digit values, so we read off a length of 1 only
                    section = 1;
                    //Set the value to true if 1, false if 0, simple
                    configEntry.BoxedValue = code.Substring(pointer, section) == "1";
                }
                //If the string says we expect a float (short)...
                else if (pointed == Convert.ToChar("s"))
                {
                    //We will be reading off 2 numbers at a time, once for the whole part and again for the decimal
                    section = 2;
                    //Whole is just the first two parts of the value converted back to an int
                    int whole = Conversions.FromBase62(code.Substring(pointer, section));
                    //Dec is just the second to parts of the value converted to an int
                    int dec = Conversions.FromBase62(code.Substring(pointer + section, section));
                    //Put the two pieces together and make it an actual float
                    float convertedValue = whole + (dec / 100f);
                    //The finished value is put into the converted value part
                    configEntry.BoxedValue = convertedValue;
                    //Section +2 so we know that we skip ahead by 4 total spots in the string
                    section += 2;
                }
                //If the string says we expect an int...
                else if (pointed == Convert.ToChar("i"))
                {
                    //Only a 2 digit value to read and skip off of
                    section = 2;
                    //Set it straight to the value, simple no problem
                    configEntry.BoxedValue = Conversions.FromBase62(code.Substring(pointer, section));
                }
                //If the string says we expect a uint...
                else if (pointed == Convert.ToChar("u"))
                {
                    //Again it's just a 2 digit num to read off
                    section = 2;
                    //Tempval is cause it is slightly more painful than int to do all in a single line
                    uint tempVal = (uint) Conversions.FromBase62(code.Substring(pointer, section));
                    //Then set value to the one we found out
                    configEntry.BoxedValue = tempVal;
                }
                //If somehow the string has no idea what is going on...
                else
                {
                    //Cancel the whole reading and note it in console
                    LogToConsole("Invalid code section, process aborted");
                    return;
                }
                //Move the pointer up the necessary amount to read the next value
                pointer += section;
                //Reset section
                section = 0;
            }
            //Tell them the config code went through just fine
            LogToConsole("Config code loaded, restart game for it to take effect");
            //Save the file
            configFile.Save();
            //Reload the file
            configFile.Reload();
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
            if (args.Count > 1) LogToConsole("Invald args");
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
                    LogToConsole("Invalid character : " + _);
                    return;
                }
            }
            //If it all works fine try to set the config with the code
            LoadConfigCode(code);
        }
    }
}
