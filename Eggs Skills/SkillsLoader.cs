using System;
using EntityStates;
using RoR2;
using UnityEngine;
using BepInEx;
using RoR2.Skills;
using EggsSkills.Properties;
using EggsSkills.SkillDefs;
using EggsSkills.EntityStates.TeslaMine.MineStates.MainStates;
using EggsSkills.EntityStates.TeslaMine.MineStates.ArmingStates;
using R2API;
using System.Security;
using System.Security.Permissions;
using EggsSkills.EntityStates;
using R2API.Utils;
using EggsSkills.Unlocks;
using System.Collections.Generic;
using EntityStates.Bandit2.Weapon;
using EntityStates.Bandit2;
using static EggsUtils.EggsUtils;
using static EggsSkills.Config.Configuration;
using EggsSkills.Resources;
using BepInEx.Bootstrap;
using System.Runtime.CompilerServices;

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
    //My mod
    [BepInPlugin(MODNAME, MODTITLE, MODVERS)]
    [R2APISubmoduleDependency(new string[]
{
    nameof(LanguageAPI),
    nameof(LoadoutAPI),
    nameof(PrefabAPI),
    //nameof(CommandHelper),
})]
    internal class SkillsLoader : BaseUnityPlugin
    {
        //Mod focused stuff / strings
        public const string MODNAME = "com.Egg.EggsSkills";
        public const string MODTITLE = "Eggs Skills";
        public const string MODVERS = "2.2.0";
        //Hard dependancy strings
        public const string API_NAME = "com.bepis.r2api";
        //Soft dependancy strings
        public const string SKILLSPLUS_NAME = "com.cwmlolzlz.skills";
        public const string AUTOSPRINT_NAME = "com.johnedwa.RTAutoSprintEx";
        public const string CLASSICITEMS_NAME = "";

        #region Characterbody References And Strings

        //Nab artificer body
        internal static readonly string artificerName = "Mage";
        internal static GameObject artificerRef = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/" + artificerName + "Body");
        //Nab merc body
        internal static readonly string mercenaryName = "Merc";
        internal static GameObject mercenaryRef = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/" + mercenaryName + "Body");
        //Nab commando body
        internal static readonly string commandoName = "Commando";
        internal static GameObject commandoRef = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/" + commandoName + "Body");
        //Nab engi body
        internal static readonly string engineerName = "Engi";
        internal static GameObject engineerRef = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/" + engineerName + "Body");
        //Nab REX body
        internal static readonly string rexName = "Treebot";
        internal static GameObject rexRef = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/" + rexName + "Body");
        //Nab loader body
        internal static readonly string loaderName = "Loader";
        internal static GameObject loaderRef = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/"+ loaderName + "Body");
        //Nab acrid body
        internal static readonly string acridName = "Croco";
        internal static GameObject acridRef = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/" + acridName + "Body");
        //Nab captain body
        internal static readonly string captainName = "Captain";
        internal static GameObject captainRef = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/" + captainName + "Body");
        //Nab bandit body
        internal static readonly string banditName = "Bandit2";
        internal static GameObject banditRef = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/" + banditName + "Body");
        //Nab MUL-T body
        internal static readonly string multName = "Toolbot";
        internal static GameObject multRef = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/" + multName + "Body");
        //Nab huntress body
        internal static readonly string huntressName = "Huntress";
        internal static GameObject huntressRef = LegacyResourcesAPI.Load<GameObject>("prefabs/characterbodies/" + huntressName + "Body");
        #endregion

        //List of all the skilldefs so we don't have to change every skill add case when r2api changes
        internal static List<SkillDef> defList = new List<SkillDef>();
        private void Awake()
        {

            //Thank SOM for being a poggers
            LogToConsole("Thanks SOM for the icon work <3");
            //Autosprint
            AutosprintAgonyEngage();
            //Add our commands to the console
            CommandHelper.AddToConsoleWhenReady();
            //Load up the config file
            LoadConfig();
            //Load up all the resources
            Assets.LoadResources();
            //Load up achievements and unlockables before skills
            UnlocksRegistering.RegisterUnlockables();
            //Finally load up the skills
            RegisterSkills();
            //SkillsPlusPlus
            if (Chainloader.PluginInfos.ContainsKey(SKILLSPLUS_NAME)) SkillsPlusPlusCompatibility();
            //Tell the console that things went just as expected :)
            LogToConsole("EggsSkills fully loaded!");
        }

        //Main method for setting up all our skills
        private void RegisterSkills()
        {
            //For each character, if the configvalue says they are allowed to load up their skills load them, otherwise don't.  Simple.
            //Artificer
            if (GetConfigValue(EnableMageSkills)) RegisterArtificerSkills();
            //Mercenary
            if (GetConfigValue(EnableMercSkills)) RegisterMercenarySkills();
            //Commando
            if (GetConfigValue(EnableCommandoSkills)) RegisterCommandoSkills();
            //Engineer
            if (GetConfigValue(EnableEngiSkills)) RegisterEngiSkills();
            //Acrid (Yes he is called croco in the official code and it is beautiful)
            if (GetConfigValue(EnableCrocoSkills)) RegisterAcridSkills();
            //Loader
            if (GetConfigValue(EnableLoaderSkills)) RegisterLoaderSkills();
            //Captain
            if (GetConfigValue(EnableCaptainSkills)) RegisterCaptainSkills();
            //REX
            if (GetConfigValue(EnableTreebotSkills)) RegisterRexSkills();
            //Bandit
            if (GetConfigValue(EnableBanditSkills)) RegisterBanditSkills();
            //Huntress
            if (GetConfigValue(EnableHuntressSkills)) RegisterHuntressSkills();
            //MUL-T
            if (GetConfigValue(EnableToolbotSkills)) RegisterMultSkills();

            //Register any states that aren't directly tied to skill activation (Basically, don't need skilldefs)
            RegisterExtraStates();

            //As long as there are any skilldefs waiting to be added...
            if (defList.Count > 0)
            {
                //For every skilldef queue'd up to be added...
                foreach (SkillDef def in defList)
                {
                    //Add skilldef via R2API
                    ContentAddition.AddSkillDef(def);
                    //Tell us each time a skill is registered, helps with sanity checks
                    LogToConsole("Skill: " + def.skillName + " Registered");
                }
            }
            //Sadness check
            else LogToConsole("Did you really install my mod just to disable all the skills :(");
        }
        #region Skills
        private void RegisterAcridSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator acridSkillLocator = acridRef.GetComponent<SkillLocator>();
            SkillFamily acridSkillFamilySpecial = acridSkillLocator.special.skillFamily;

            //AcridPurge
            AcridPurgeDef skillDefPurge = ScriptableObject.CreateInstance<AcridPurgeDef>();
            skillDefPurge.activationState = new SerializableEntityStateType(typeof(AcridPurgeEntity));
            skillDefPurge.activationStateMachineName = "Body";
            skillDefPurge.baseMaxStock = 1;
            skillDefPurge.baseRechargeInterval = 12f;
            skillDefPurge.beginSkillCooldownOnSkillEnd = false;
            skillDefPurge.fullRestockOnAssign = false;
            skillDefPurge.interruptPriority = InterruptPriority.Skill;
            skillDefPurge.isCombatSkill = true;
            skillDefPurge.mustKeyPress = true;
            skillDefPurge.canceledFromSprinting = false;
            skillDefPurge.cancelSprintingOnActivation = false;
            skillDefPurge.forceSprintDuringState = false;
            skillDefPurge.rechargeStock = 1;
            skillDefPurge.requiredStock = 1;
            skillDefPurge.stockToConsume = 1;
            skillDefPurge.icon = Sprites.acridpurgeIconS;
            skillDefPurge.skillDescriptionToken = LanguageTokens.prefix + acridName.ToUpper() + "_" + "SPECIAL_PURGE" + LanguageTokens.dSuffix;
            skillDefPurge.skillName = "Purge";
            skillDefPurge.skillNameToken = LanguageTokens.prefix + acridName.ToUpper() + "_" + "SPECIAL_PURGE" + LanguageTokens.nSuffix;

            defList.Add(skillDefPurge);
            Array.Resize(ref acridSkillFamilySpecial.variants, acridSkillFamilySpecial.variants.Length + 1);
            acridSkillFamilySpecial.variants[acridSkillFamilySpecial.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefPurge,
                unlockableDef = UnlocksRegistering.acridPurgeUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefPurge.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<AcridPurgeEntity>(out _);
        }

        private void RegisterArtificerSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator artificerSkillLocator = artificerRef.GetComponent<SkillLocator>();
            SkillFamily artificerSkillFamilyUtility = artificerSkillLocator.utility.skillFamily;

            //Zapport
            SkillDef skillDefZapport = ScriptableObject.CreateInstance<SkillDef>();
            skillDefZapport.activationState = new SerializableEntityStateType(typeof(ZapportChargeEntity));
            skillDefZapport.activationStateMachineName = "Weapon";
            skillDefZapport.baseMaxStock = 2;
            skillDefZapport.baseRechargeInterval = 10f;
            skillDefZapport.beginSkillCooldownOnSkillEnd = true;
            skillDefZapport.fullRestockOnAssign = false;
            skillDefZapport.interruptPriority = InterruptPriority.PrioritySkill;
            skillDefZapport.isCombatSkill = true;
            skillDefZapport.mustKeyPress = false;
            skillDefZapport.canceledFromSprinting = false;
            skillDefZapport.cancelSprintingOnActivation = true;
            skillDefZapport.forceSprintDuringState = false;
            skillDefZapport.rechargeStock = 1;
            skillDefZapport.requiredStock = 1;
            skillDefZapport.stockToConsume = 1;
            skillDefZapport.icon = Sprites.zapportIconS;
            skillDefZapport.skillDescriptionToken = LanguageTokens.prefix + artificerName.ToUpper() + "_" + "UTILITY_ZAPPORT" + LanguageTokens.dSuffix;
            skillDefZapport.skillName = "Zapport";
            skillDefZapport.skillNameToken = LanguageTokens.prefix + artificerName.ToUpper() + "_" + "UTILITY_ZAPPORT" + LanguageTokens.nSuffix;
            skillDefZapport.keywordTokens = new string[]
            {
                "KEYWORD_STUNNING",
                "KEYWORD_ENHANCING"
            };

            defList.Add(skillDefZapport);
            Array.Resize(ref artificerSkillFamilyUtility.variants, artificerSkillFamilyUtility.variants.Length + 1);
            artificerSkillFamilyUtility.variants[artificerSkillFamilyUtility.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefZapport,
                unlockableDef = UnlocksRegistering.artificerZapportUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefZapport.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<ZapportChargeEntity>(out _);
        }

        private void RegisterBanditSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator banditSkillLocator = banditRef.GetComponent<SkillLocator>();
            SkillFamily banditSkillFamilyUtility = banditSkillLocator.utility.skillFamily;
            SkillFamily banditSkillFamilyPrimary = banditSkillLocator.primary.skillFamily;

            //Thieves Cunning
            InvisOnSprintSkillDef skillDefInvisSprint = ScriptableObject.CreateInstance<InvisOnSprintSkillDef>();
            //These two are dummies, don't actually exist, just there to stop errors
            skillDefInvisSprint.activationState = new SerializableEntityStateType(typeof(ThrowSmokebomb));
            skillDefInvisSprint.activationStateMachineName = "Body";
            skillDefInvisSprint.baseMaxStock = 1;
            skillDefInvisSprint.baseRechargeInterval = 6f;
            skillDefInvisSprint.fullRestockOnAssign = false;
            skillDefInvisSprint.rechargeStock = 1;
            skillDefInvisSprint.requiredStock = 1;
            skillDefInvisSprint.stockToConsume = 1;
            skillDefInvisSprint.icon = Sprites.invisSprintIconS;
            skillDefInvisSprint.skillDescriptionToken = LanguageTokens.prefix + banditName.ToUpper() + "_" + "UTILITY_INVISSPRINT" + LanguageTokens.dSuffix;
            skillDefInvisSprint.skillName = "InvisSprint";
            skillDefInvisSprint.skillNameToken = LanguageTokens.prefix + banditName.ToUpper() + "_" + "UTILITY_INVISSPRINT" + LanguageTokens.nSuffix;

            defList.Add(skillDefInvisSprint);
            Array.Resize(ref banditSkillFamilyUtility.variants, banditSkillFamilyUtility.variants.Length + 1);
            banditSkillFamilyUtility.variants[banditSkillFamilyUtility.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefInvisSprint,
                unlockableDef = UnlocksRegistering.banditInvisSprintUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefInvisSprint.skillNameToken, false, null)
            };

            //Magic bullet
            ReloadSkillDef skillDefMagicBullet = ScriptableObject.CreateInstance<ReloadSkillDef>();
            skillDefMagicBullet.activationState = new SerializableEntityStateType(typeof(MagicBulletEntity));
            skillDefMagicBullet.reloadState = new SerializableEntityStateType(typeof(EnterReload));
            skillDefMagicBullet.activationStateMachineName = "Weapon";
            skillDefMagicBullet.baseMaxStock = 4;
            skillDefMagicBullet.graceDuration = 0.8f;
            skillDefMagicBullet.beginSkillCooldownOnSkillEnd = true;
            skillDefMagicBullet.fullRestockOnAssign = false;
            skillDefMagicBullet.isCombatSkill = true;
            skillDefMagicBullet.mustKeyPress = true;
            skillDefMagicBullet.reloadInterruptPriority = InterruptPriority.Any;
            skillDefMagicBullet.canceledFromSprinting = true;
            skillDefMagicBullet.cancelSprintingOnActivation = true;
            skillDefMagicBullet.forceSprintDuringState = false;
            skillDefMagicBullet.baseRechargeInterval = 0f;
            skillDefMagicBullet.rechargeStock = 0;
            skillDefMagicBullet.requiredStock = 1;
            skillDefMagicBullet.stockToConsume = 1;
            skillDefMagicBullet.icon = Sprites.magicBulletIconS;
            skillDefMagicBullet.skillDescriptionToken = LanguageTokens.prefix + banditName.ToUpper() + "_" + "PRIMARY_MAGICBULLET" + LanguageTokens.dSuffix;
            skillDefMagicBullet.skillName = "MagicBullet";
            skillDefMagicBullet.skillNameToken = LanguageTokens.prefix + banditName.ToUpper() + "_" + "PRIMARY_MAGICBULLET" + LanguageTokens.nSuffix;

            defList.Add(skillDefMagicBullet);
            Array.Resize(ref banditSkillFamilyPrimary.variants, banditSkillFamilyPrimary.variants.Length + 1);
            banditSkillFamilyPrimary.variants[banditSkillFamilyPrimary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefMagicBullet,
                unlockableDef = UnlocksRegistering.banditMagicBulletUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefMagicBullet.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<MagicBulletEntity>(out _);
        }

        private void RegisterCaptainSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator captainSkillLocator = captainRef.GetComponent<SkillLocator>();
            SkillFamily captainSkillFamilySecondary = captainSkillLocator.secondary.skillFamily;

            //DebuffGrenade
            SkillDef skillDefDebuffnade = ScriptableObject.CreateInstance<SkillDef>();
            skillDefDebuffnade.activationState = new SerializableEntityStateType(typeof(DebuffGrenadeEntity));
            skillDefDebuffnade.activationStateMachineName = "Weapon";
            skillDefDebuffnade.baseMaxStock = 1;
            skillDefDebuffnade.baseRechargeInterval = 10f;
            skillDefDebuffnade.beginSkillCooldownOnSkillEnd = true;
            skillDefDebuffnade.fullRestockOnAssign = false;
            skillDefDebuffnade.interruptPriority = InterruptPriority.Skill;
            skillDefDebuffnade.isCombatSkill = true;
            skillDefDebuffnade.mustKeyPress = false;
            skillDefDebuffnade.canceledFromSprinting = false;
            skillDefDebuffnade.cancelSprintingOnActivation = true;
            skillDefDebuffnade.forceSprintDuringState = false;
            skillDefDebuffnade.rechargeStock = 1;
            skillDefDebuffnade.requiredStock = 1;
            skillDefDebuffnade.stockToConsume = 1;
            skillDefDebuffnade.icon = Sprites.debuffNadeIconS;
            skillDefDebuffnade.skillDescriptionToken = LanguageTokens.prefix + captainName.ToUpper() + "_" + "SECONDARY_DEBUFFNADE" + LanguageTokens.dSuffix;
            skillDefDebuffnade.skillName = "Debuffnade";
            skillDefDebuffnade.skillNameToken = LanguageTokens.prefix + captainName.ToUpper() + "_" + "SECONDARY_DEBUFFNADE" + LanguageTokens.nSuffix;
            skillDefDebuffnade.keywordTokens = new string[]
            {
                "KEYWORD_MARKING",
            };

            defList.Add(skillDefDebuffnade);
            Array.Resize(ref captainSkillFamilySecondary.variants, captainSkillFamilySecondary.variants.Length + 1);
            captainSkillFamilySecondary.variants[captainSkillFamilySecondary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefDebuffnade,
                unlockableDef = UnlocksRegistering.captainDebuffnadeUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefDebuffnade.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<DebuffGrenadeEntity>(out _);
        }

        private void RegisterCommandoSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator commandoSkillLocator = commandoRef.GetComponent<SkillLocator>();
            SkillFamily commandoSkillFamilyPrimary = commandoSkillLocator.primary.skillFamily;
            SkillFamily commandoSkillFamilyUtility = commandoSkillLocator.utility.skillFamily;

            //Combat Shotgun
            SteppedSkillDef skillDefCombatshotgun = ScriptableObject.CreateInstance<SteppedSkillDef>();
            skillDefCombatshotgun.activationState = new SerializableEntityStateType(typeof(CombatShotgunEntity));
            skillDefCombatshotgun.activationStateMachineName = "Weapon";
            skillDefCombatshotgun.beginSkillCooldownOnSkillEnd = true;
            skillDefCombatshotgun.fullRestockOnAssign = true;
            skillDefCombatshotgun.interruptPriority = InterruptPriority.Any;
            skillDefCombatshotgun.isCombatSkill = true;
            skillDefCombatshotgun.mustKeyPress = false;
            skillDefCombatshotgun.canceledFromSprinting = false;
            skillDefCombatshotgun.cancelSprintingOnActivation = true;
            skillDefCombatshotgun.forceSprintDuringState = false;
            skillDefCombatshotgun.stockToConsume = 0;
            skillDefCombatshotgun.icon = Sprites.shotgunIconS;
            skillDefCombatshotgun.skillDescriptionToken = LanguageTokens.prefix + commandoName.ToUpper() + "_" + "PRIMARY_COMBATSHOTGUN" + LanguageTokens.dSuffix;
            skillDefCombatshotgun.skillName = "CombatShotgun";
            skillDefCombatshotgun.skillNameToken = LanguageTokens.prefix + commandoName.ToUpper() + "_" + "PRIMARY_COMBATSHOTGUN" + LanguageTokens.nSuffix;

            defList.Add(skillDefCombatshotgun);
            Array.Resize(ref commandoSkillFamilyPrimary.variants, commandoSkillFamilyPrimary.variants.Length + 1);
            commandoSkillFamilyPrimary.variants[commandoSkillFamilyPrimary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefCombatshotgun,
                unlockableDef = UnlocksRegistering.commandoShotgunUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefCombatshotgun.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<CombatShotgunEntity>(out _);

            //Dash
            SkillDef skillDefDash = ScriptableObject.CreateInstance<SkillDef>();
            skillDefDash.activationState = new SerializableEntityStateType(typeof(CommandoDashEntity));
            skillDefDash.activationStateMachineName = "Body";
            skillDefDash.baseMaxStock = 2;
            skillDefDash.baseRechargeInterval = 10f;
            skillDefDash.beginSkillCooldownOnSkillEnd = true;
            skillDefDash.fullRestockOnAssign = false;
            skillDefDash.interruptPriority = InterruptPriority.PrioritySkill;
            skillDefDash.isCombatSkill = false;
            skillDefDash.mustKeyPress = true;
            skillDefDash.canceledFromSprinting = false;
            skillDefDash.cancelSprintingOnActivation = false;
            skillDefDash.forceSprintDuringState = true;
            skillDefDash.stockToConsume = 1;
            skillDefDash.requiredStock = 1;
            skillDefDash.rechargeStock = 1;
            skillDefDash.icon = Sprites.dashIconS;
            skillDefDash.skillDescriptionToken = LanguageTokens.prefix + commandoName.ToUpper() + "_" + "UTILITY_DASH" + LanguageTokens.dSuffix;
            skillDefDash.skillName = "Dash";
            skillDefDash.skillNameToken = LanguageTokens.prefix + commandoName.ToUpper() + "_" + "UTILITY_DASH" + LanguageTokens.nSuffix;
            skillDefDash.keywordTokens = new string[]
            {
                 "KEYWORD_PREPARE"
            };

            defList.Add(skillDefDash);
            Array.Resize(ref commandoSkillFamilyUtility.variants, commandoSkillFamilyUtility.variants.Length + 1);
            commandoSkillFamilyUtility.variants[commandoSkillFamilyUtility.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefDash,
                unlockableDef = UnlocksRegistering.commandoDashUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefDash.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<CommandoDashEntity>(out _);
        }

        private void RegisterEngiSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator engiSkillLocator = engineerRef.GetComponent<SkillLocator>();
            SkillFamily engiSkillFamilySecondary = engiSkillLocator.secondary.skillFamily;

            SkillDef skillDefTeslamine = ScriptableObject.CreateInstance<SkillDef>();
            skillDefTeslamine.activationState = new SerializableEntityStateType(typeof(TeslaMineFireState));
            skillDefTeslamine.activationStateMachineName = "Weapon";
            skillDefTeslamine.baseMaxStock = 4;
            skillDefTeslamine.baseRechargeInterval = 10f;
            skillDefTeslamine.beginSkillCooldownOnSkillEnd = false;
            skillDefTeslamine.fullRestockOnAssign = false;
            skillDefTeslamine.interruptPriority = InterruptPriority.Skill;
            skillDefTeslamine.isCombatSkill = true;
            skillDefTeslamine.mustKeyPress = false;
            skillDefTeslamine.canceledFromSprinting = false;
            skillDefTeslamine.cancelSprintingOnActivation = true;
            skillDefTeslamine.forceSprintDuringState = false;
            skillDefTeslamine.rechargeStock = 1;
            skillDefTeslamine.requiredStock = 1;
            skillDefTeslamine.stockToConsume = 1;
            skillDefTeslamine.icon = Sprites.teslaMineIconS;
            skillDefTeslamine.skillDescriptionToken = LanguageTokens.prefix + engineerName.ToUpper() + "_" + "SECONDARY_TESLAMINE" + LanguageTokens.dSuffix;
            skillDefTeslamine.skillName = "TeslaMine";
            skillDefTeslamine.skillNameToken = LanguageTokens.prefix + engineerName.ToUpper() + "_" + "SECONDARY_TESLAMINE" + LanguageTokens.nSuffix;
            skillDefTeslamine.keywordTokens = new string[]
            {
                "KEYWORD_STUNNING"
            };

            defList.Add(skillDefTeslamine);
            Array.Resize(ref engiSkillFamilySecondary.variants, engiSkillFamilySecondary.variants.Length + 1);
            engiSkillFamilySecondary.variants[engiSkillFamilySecondary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefTeslamine,
                unlockableDef = UnlocksRegistering.engiTeslaUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefTeslamine.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<TeslaMineFireState>(out _);
        }

        private void RegisterHuntressSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator huntressSkillLocator = huntressRef.GetComponent<SkillLocator>();
            SkillFamily huntressSkillFamilySecondary = huntressSkillLocator.secondary.skillFamily;

            //Cluster bomb arrow
            HuntressTrackingSkillDef skillDefClusterArrow = ScriptableObject.CreateInstance<HuntressTrackingSkillDef>();
            skillDefClusterArrow.activationState = new SerializableEntityStateType(typeof(ClusterBombArrow));
            skillDefClusterArrow.activationStateMachineName = "Weapon";
            skillDefClusterArrow.baseMaxStock = 1;
            skillDefClusterArrow.baseRechargeInterval = 8f;
            skillDefClusterArrow.beginSkillCooldownOnSkillEnd = false;
            skillDefClusterArrow.fullRestockOnAssign = false;
            skillDefClusterArrow.interruptPriority = InterruptPriority.Skill;
            skillDefClusterArrow.isCombatSkill = true;
            skillDefClusterArrow.mustKeyPress = false;
            skillDefClusterArrow.canceledFromSprinting = false;
            skillDefClusterArrow.cancelSprintingOnActivation = false;
            skillDefClusterArrow.forceSprintDuringState = false;
            skillDefClusterArrow.rechargeStock = 1;
            skillDefClusterArrow.requiredStock = 1;
            skillDefClusterArrow.stockToConsume = 1;
            skillDefClusterArrow.icon = Sprites.clusterArrowIconS;
            skillDefClusterArrow.skillDescriptionToken = LanguageTokens.prefix + huntressName.ToUpper() + "_" + "SECONDARY_CLUSTERARROW" + LanguageTokens.dSuffix;
            skillDefClusterArrow.skillName = "ClusterArrow";
            skillDefClusterArrow.skillNameToken = LanguageTokens.prefix + huntressName.ToUpper() + "_" + "SECONDARY_CLUSTERARROW" + LanguageTokens.nSuffix;
            skillDefClusterArrow.keywordTokens = new string[]
            {
                "KEYWORD_AGILE"
            };

            defList.Add(skillDefClusterArrow);
            Array.Resize(ref huntressSkillFamilySecondary.variants, huntressSkillFamilySecondary.variants.Length + 1);
            huntressSkillFamilySecondary.variants[huntressSkillFamilySecondary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefClusterArrow,
                unlockableDef = UnlocksRegistering.huntressClusterarrowUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefClusterArrow.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<ClusterBombArrow>(out _);
        }

        private void RegisterLoaderSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator loaderSkillLocator = loaderRef.GetComponent<SkillLocator>();
            SkillFamily loaderSkillFamilySpecial = loaderSkillLocator.special.skillFamily;

            //Shieldsplosion
            ShieldsplosionDef skillDefShieldsplode = ScriptableObject.CreateInstance<ShieldsplosionDef>();
            skillDefShieldsplode.activationState = new SerializableEntityStateType(typeof(ShieldSplosionEntity));
            skillDefShieldsplode.activationStateMachineName = "Body";
            skillDefShieldsplode.baseMaxStock = 1;
            skillDefShieldsplode.baseRechargeInterval = 8f;
            skillDefShieldsplode.beginSkillCooldownOnSkillEnd = false;
            skillDefShieldsplode.fullRestockOnAssign = false;
            skillDefShieldsplode.interruptPriority = InterruptPriority.Skill;
            skillDefShieldsplode.isCombatSkill = true;
            skillDefShieldsplode.mustKeyPress = true;
            skillDefShieldsplode.canceledFromSprinting = false;
            skillDefShieldsplode.cancelSprintingOnActivation = false;
            skillDefShieldsplode.forceSprintDuringState = false;
            skillDefShieldsplode.rechargeStock = 1;
            skillDefShieldsplode.requiredStock = 1;
            skillDefShieldsplode.stockToConsume = 1;
            skillDefShieldsplode.icon = Sprites.shieldsplosionIconS;
            skillDefShieldsplode.skillDescriptionToken = LanguageTokens.prefix + loaderName.ToUpper() + "_" + "SPECIAL_SHIELDSPLOSION" + LanguageTokens.dSuffix;
            skillDefShieldsplode.skillName = "ShieldSplosion";
            skillDefShieldsplode.skillNameToken = LanguageTokens.prefix + loaderName.ToUpper() + "_" + "SPECIAL_SHIELDSPLOSION" + LanguageTokens.nSuffix;

            defList.Add(skillDefShieldsplode);
            Array.Resize(ref loaderSkillFamilySpecial.variants, loaderSkillFamilySpecial.variants.Length + 1);
            loaderSkillFamilySpecial.variants[loaderSkillFamilySpecial.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefShieldsplode,
                unlockableDef = UnlocksRegistering.loaderShieldsplosionUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefShieldsplode.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<ShieldSplosionEntity>(out _);
        }

        private void RegisterMercenarySkills()
        {
            SkillLocator mercSkillLocator = mercenaryRef.GetComponent<SkillLocator>();
            SkillFamily mercSkillFamilySpecial = mercSkillLocator.special.skillFamily;

            //Slashport
            MercSlashportDef skillDefSlashport = ScriptableObject.CreateInstance<MercSlashportDef>();
            skillDefSlashport.activationState = new SerializableEntityStateType(typeof(SlashportEntity));
            skillDefSlashport.activationStateMachineName = "Weapon";
            skillDefSlashport.baseMaxStock = 1;
            skillDefSlashport.baseRechargeInterval = 8f;
            skillDefSlashport.beginSkillCooldownOnSkillEnd = true;
            skillDefSlashport.fullRestockOnAssign = false;
            skillDefSlashport.interruptPriority = InterruptPriority.PrioritySkill;
            skillDefSlashport.isCombatSkill = true;
            skillDefSlashport.mustKeyPress = false;
            skillDefSlashport.canceledFromSprinting = false;
            skillDefSlashport.cancelSprintingOnActivation = false;
            skillDefSlashport.forceSprintDuringState = false;
            skillDefSlashport.rechargeStock = 1;
            skillDefSlashport.requiredStock = 1;
            skillDefSlashport.stockToConsume = 1;
            skillDefSlashport.icon = Sprites.slashportIconS;
            skillDefSlashport.skillDescriptionToken = LanguageTokens.prefix + mercenaryName.ToUpper() + "_" + "SPECIAL_SLASHPORT" + LanguageTokens.dSuffix;
            skillDefSlashport.skillName = "Slashport";
            skillDefSlashport.skillNameToken = LanguageTokens.prefix + mercenaryName.ToUpper() + "_" + "SPECIAL_SLASHPORT" + LanguageTokens.nSuffix;
            skillDefSlashport.keywordTokens = new string[]
            {
                "KEYWORD_EXPOSE",
                "KEYWORD_STUNNING",
                "KEYWORD_SLAYER"
            };

            defList.Add(skillDefSlashport);
            Array.Resize(ref mercSkillFamilySpecial.variants, mercSkillFamilySpecial.variants.Length + 1);
            mercSkillFamilySpecial.variants[mercSkillFamilySpecial.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefSlashport,
                unlockableDef = UnlocksRegistering.mercSlashportUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefSlashport.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<SlashportEntity>(out _);
        }

        private void RegisterMultSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator multSkillLocator = multRef.GetComponent<SkillLocator>();
            SkillFamily multSkillFamilySecondary = multSkillLocator.secondary.skillFamily;

            //NanobotSwarm
            NanoSkilldef skillDefNanoSwarm = ScriptableObject.CreateInstance<NanoSkilldef>();
            skillDefNanoSwarm.activationState = new SerializableEntityStateType(typeof(NanobotEntity));
            skillDefNanoSwarm.activationStateMachineName = "Weapon";
            skillDefNanoSwarm.baseMaxStock = 1;
            skillDefNanoSwarm.baseRechargeInterval = 12f;
            skillDefNanoSwarm.beginSkillCooldownOnSkillEnd = true;
            skillDefNanoSwarm.fullRestockOnAssign = false;
            skillDefNanoSwarm.interruptPriority = InterruptPriority.Skill;
            skillDefNanoSwarm.isCombatSkill = true;
            skillDefNanoSwarm.mustKeyPress = true;
            skillDefNanoSwarm.canceledFromSprinting = false;
            skillDefNanoSwarm.cancelSprintingOnActivation = true;
            skillDefNanoSwarm.forceSprintDuringState = false;
            skillDefNanoSwarm.rechargeStock = 1;
            skillDefNanoSwarm.requiredStock = 1;
            skillDefNanoSwarm.stockToConsume = 1;
            skillDefNanoSwarm.icon = Sprites.nanoBotsIconS;
            skillDefNanoSwarm.skillDescriptionToken = LanguageTokens.prefix + multName.ToUpper() + "_" + "SECONDARY_NANOBOT" + LanguageTokens.dSuffix;
            skillDefNanoSwarm.skillName = "NanobotSwarm";
            skillDefNanoSwarm.skillNameToken = LanguageTokens.prefix + multName.ToUpper() + "_" + "SECONDARY_NANOBOT" + LanguageTokens.nSuffix;
            skillDefNanoSwarm.keywordTokens = new string[]
            {
                "KEYWORD_MARKING"
            };

            defList.Add(skillDefNanoSwarm);
            Array.Resize(ref multSkillFamilySecondary.variants, multSkillFamilySecondary.variants.Length + 1);
            multSkillFamilySecondary.variants[multSkillFamilySecondary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefNanoSwarm,
                unlockableDef = UnlocksRegistering.multNanobeaconUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefNanoSwarm.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<NanobotEntity>(out _);
        }

        private void RegisterRexSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator rexSkillLocator = rexRef.GetComponent<SkillLocator>();
            SkillFamily rexSkillFamilySpecial = rexSkillLocator.special.skillFamily;

            //Directive Root
            GroundedSkillDef skillDefRoot = ScriptableObject.CreateInstance<GroundedSkillDef>();
            skillDefRoot.activationState = new SerializableEntityStateType(typeof(DirectiveRoot));
            skillDefRoot.activationStateMachineName = "Weapon";
            skillDefRoot.baseMaxStock = 1;
            skillDefRoot.baseRechargeInterval = 12f;
            skillDefRoot.beginSkillCooldownOnSkillEnd = true;
            skillDefRoot.fullRestockOnAssign = false;
            skillDefRoot.interruptPriority = InterruptPriority.Skill;
            skillDefRoot.isCombatSkill = true;
            skillDefRoot.mustKeyPress = true;
            skillDefRoot.canceledFromSprinting = true;
            skillDefRoot.cancelSprintingOnActivation = true;
            skillDefRoot.forceSprintDuringState = false;
            skillDefRoot.rechargeStock = 1;
            skillDefRoot.requiredStock = 1;
            skillDefRoot.stockToConsume = 1;
            skillDefRoot.icon = Sprites.rexrootIconS;
            skillDefRoot.skillDescriptionToken = LanguageTokens.prefix + rexName.ToUpper() + "_" + "SPECIAL_ROOT" + LanguageTokens.dSuffix;
            skillDefRoot.skillName = "Root";
            skillDefRoot.skillNameToken = LanguageTokens.prefix + rexName.ToUpper() + "_" + "SPECIAL_ROOT" + LanguageTokens.nSuffix;
            skillDefRoot.keywordTokens = new string[]
            {
                "KEYWORD_STUNNING",
                "KEYWORD_ADAPTIVE"
            };

            defList.Add(skillDefRoot);
            Array.Resize(ref rexSkillFamilySpecial.variants, rexSkillFamilySpecial.variants.Length + 1);
            rexSkillFamilySpecial.variants[rexSkillFamilySpecial.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefRoot,
                unlockableDef = UnlocksRegistering.rexRootUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefRoot.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<DirectiveRoot>(out _);
        }
        #endregion

        private void RegisterExtraStates()
        {
            //Artificer extra states
            if (GetConfigValue(EnableMageSkills))
            {
                //Artificer Zappot Fire State
                ContentAddition.AddEntityState<ZapportFireEntity>(out _);
                LogToConsole("Zapport fire state loaded!");
            }

            //Engi extra states
            if (GetConfigValue(EnableEngiSkills))
            {
                //Tesla mine arming states
                ContentAddition.AddEntityState<TeslaArmingUnarmedState>(out _);
                ContentAddition.AddEntityState<TeslaArmingWeakState>(out _);
                ContentAddition.AddEntityState<TeslaArmingFullState>(out _);
                //Main states
                ContentAddition.AddEntityState<TeslaArmState>(out _);
                ContentAddition.AddEntityState<TeslaWaitForStick>(out _);
                ContentAddition.AddEntityState<TeslaWaitForTargetState>(out _);
                ContentAddition.AddEntityState<TeslaPreDetState>(out _);
                ContentAddition.AddEntityState<TeslaDetonateState>(out _);
                LogToConsole("Tesla mine states loaded!");
            }
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

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void SkillsPlusPlusCompatibility()
        {
            SkillsPlusPlus.SkillModifierManager.LoadSkillModifiers();
        }
    }
}