using BepInEx;
using System.Security;
using System.Security.Permissions;
using System.Collections.Generic;
using BepInEx.Bootstrap;
using EggsSkills.Unlocks;
using System.Runtime.CompilerServices;
using static EggsUtils.EggsUtils;
using static EggsSkills.Config.Configuration;
using EggsSkills.Properties;
using static EggsSkills.SkillsLoader;
using R2API.Utils;
using EggsUtils.Properties;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace EggsSkills
{
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    //Required compats
    [BepInDependency(COMPAT_NAME, BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency(API_NAME, BepInDependency.DependencyFlags.HardDependency)]
    //Optional compats
    [BepInDependency(SKILLSPLUS_NAME, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(AUTOSPRINT_NAME, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(CLASSICITEMS_NAME, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(STANDALONESCEPTER_NAME, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency(PLASMACORESPIKESTRIP_NAME, BepInDependency.DependencyFlags.SoftDependency)]
    //My mod
    [BepInPlugin(MODNAME, MODTITLE, MODVERS)]
    internal class EggsSkills : BaseUnityPlugin
    {
        //Mod focused stuff / strings
        public const string MODNAME = "com.Egg.EggsSkills";
        public const string MODTITLE = "Eggs Skills";
        public const string MODVERS = "2.4.5";
        //Hard dependancy strings
        public const string API_NAME = "com.bepis.r2api";
        //Soft dependancy strings
        public const string SKILLSPLUS_NAME = "com.cwmlolzlz.skills";
        public const string AUTOSPRINT_NAME = "com.johnedwa.RTAutoSprintEx";
        public const string CLASSICITEMS_NAME = "com.ThinkInvisible.ClassicItems";
        public const string STANDALONESCEPTER_NAME = "com.DestroyedClone.AncientScepter";
        public const string PLASMACORESPIKESTRIP_NAME = "com.plasmacore.PlasmaCoreSpikestripContent";

        public static bool skillsPlusLoaded = false;
        public static bool classicItemsLoaded = false;
        public static bool standaloneScepterLoaded = false;
        public static bool plasmacoreSpikestripLoaded = false;

        private void Awake()
        {
            //Log init
            Log.Init(Logger);
            #region Compats
            //Do the skills++ exist
            skillsPlusLoaded = Chainloader.PluginInfos.ContainsKey(SKILLSPLUS_NAME);
            //Do the classicitems exist
            classicItemsLoaded = Chainloader.PluginInfos.ContainsKey(CLASSICITEMS_NAME);
            //Standalone too
            standaloneScepterLoaded = Chainloader.PluginInfos.ContainsKey(STANDALONESCEPTER_NAME);
            //Deeprot specifically is all we need from here
            plasmacoreSpikestripLoaded = Chainloader.PluginInfos.ContainsKey(PLASMACORESPIKESTRIP_NAME);
            #endregion
            #region Assets loading
            //Autosprint
            AutosprintAgonyEngage();
            //Load up the config file
            LoadConfig();
            //Load up all the resources
            SkillsAssets.LoadResources();
            #endregion
            #region Skills stuff
            //Load up achievements and unlockables before skills
            UnlocksRegistering.RegisterUnlockables();
            //Also before skills handle the scepter compat
            if(classicItemsLoaded || standaloneScepterLoaded) ScepterCompatibility();
            //Finally load up the skills
            RegisterSkills();
            //Classicitems (Scepter) compat
            if (classicItemsLoaded) SetScepterReplacements();
            //Standalone scepter compat
            else if (standaloneScepterLoaded) SetStandaloneScepterReplacements();
            //Skills++ compat
            if (skillsPlusLoaded) SkillsPlusPlusCompatibility();
            #endregion
            //Tell the console that things went just as expected :)
            Log.LogMessage("EggsSkills fully loaded!");
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void SkillsPlusPlusCompatibility()
        {
            SkillsPlusPlus.SkillModifierManager.LoadSkillModifiers();
        }

        private void ScepterCompatibility()
        {
            skillsToHandle = new Dictionary<string, SkillUpgradeContainer>();
        }

        private void AutosprintAgonyEngage()
        {
            if (Chainloader.PluginInfos.ContainsKey("com.johnedwa.RTAutoSprintEx"))
            {
                SendMessage("RT_SprintDisableMessage", "EggsSkills.EntityStates.DirectiveRoot");
                SendMessage("RT_AnimationDelayMessage", "EggsSkills.EntityStates.CombatShotgunEntity");
                SendMessage("RT_AnimationDelayMessage", "EggsSkills.EntityStates.TeslaMineFireState");
            }
        }
    }
}
