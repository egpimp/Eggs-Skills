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
using EggsSkills.Config;
using EntityStates.Bandit2.Weapon;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace EggsSkills
{
    [BepInDependency("com.Egg.EggsUtils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("com.Egg.EggsSkills", "Eggs Skills", "2.0.10")]
    [R2APISubmoduleDependency(new string[]
{
    nameof(ProjectileAPI),
    nameof(LanguageAPI),
    nameof(LoadoutAPI),
    nameof(PrefabAPI),
    nameof(UnlockableAPI),
    nameof(CommandHelper)
})]
    internal class SkillsLoader : BaseUnityPlugin
    {
        internal static GameObject artificerRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/MageBody");
        internal static GameObject mercenaryRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/MercBody");
        internal static GameObject commandoRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/CommandoBody");
        internal static GameObject engineerRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/EngiBody");
        internal static GameObject rexRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/TreebotBody");
        internal static GameObject loaderRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/LoaderBody");
        internal static GameObject acridRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/CrocoBody");
        internal static GameObject captainRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/CaptainBody");
        internal static GameObject banditRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/Bandit2Body");
        internal static GameObject multRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/ToolbotBody");
        internal static GameObject huntressRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/HuntressBody");

        internal static List<SkillDef> defList = new List<SkillDef>();
        internal static List<Type> extraStates = new List<Type>();
        private void Awake()
        {
            EggsUtils.EggsUtils.LogToConsole("Thanks SOM for the icon work <3");
            CommandHelper.AddToConsoleWhenReady();
            Configuration.LoadConfig();
            Assets.LoadResources();
            UnlocksRegistering.RegisterUnlockables();
            RegisterSkills();
            EggsUtils.EggsUtils.LogToConsole("EggsSkills fully loaded!");
        }
        private void RegisterSkills()
        {
            if (Configuration.GetConfigValue<bool>(Configuration.EnableMageSkills))
            {
                RegisterArtificerSkills();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableMercSkills))
            {
                RegisterMercenarySkills();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableCommandoSkills))
            {
                RegisterCommandoSkills();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableEngiSkills))
            {
                RegisterEngiSkills();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableCrocoSkills))
            {
                RegisterAcridSkills();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableLoaderSkills))
            {
                RegisterLoaderSkills();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableCaptainSkills))
            {
                RegisterCaptainSkills();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableTreebotSkills))
            {
                RegisterRexSkills();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableToolbotSkills))
            {
                RegisterBanditSkills();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableHuntressSkills))
            {
                RegisterHuntressSkills();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableToolbotSkills))
            {
                RegisterMultSkills();
            }
            if (defList.Count > 0)
            {
                foreach (SkillDef def in defList)
                {
                    LoadoutAPI.AddSkillDef(def);
                    EggsUtils.EggsUtils.LogToConsole("Skill: " + def.skillName + " Registered");
                }
                foreach (Type skill in extraStates)
                {
                    LoadoutAPI.AddSkill(skill);
                }
            }
            else
            {
                EggsUtils.EggsUtils.LogToConsole("Did you really install my mod just to disable all the skills :(");
            }
        }

        private void RegisterArtificerSkills()
        {
            SkillLocator artificerSkillLocator = artificerRef.GetComponent<SkillLocator>();
            SkillFamily artificerSkillFamilyUtility = artificerSkillLocator.utility.skillFamily;

            //Zapport
            SkillDef skillDefZapport = ScriptableObject.CreateInstance<SkillDef>();
            skillDefZapport.activationState = new SerializableEntityStateType(typeof(ZapportChargeEntity));
            skillDefZapport.activationStateMachineName = "Weapon";
            skillDefZapport.baseMaxStock = 1;
            skillDefZapport.baseRechargeInterval = 12f;
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
            skillDefZapport.icon = Resources.Sprites.zapportIconS;
            skillDefZapport.skillDescriptionToken = "ARTIFICER_UTILITY_ZAPPORT_DESC";
            skillDefZapport.skillName = "Zapport";
            skillDefZapport.skillNameToken = "ARTIFICER_UTILITY_ZAPPORT_NAME";
            skillDefZapport.keywordTokens = new string[]
            {
                "KEYWORD_STUNNING"
            };

            defList.Add(skillDefZapport);
            Array.Resize(ref artificerSkillFamilyUtility.variants, artificerSkillFamilyUtility.variants.Length + 1);
            artificerSkillFamilyUtility.variants[artificerSkillFamilyUtility.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefZapport,
                unlockableDef = UnlocksRegistering.artificerZapportUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefZapport.skillNameToken, false, null)
            };
            extraStates.Add(typeof(ZapportFireEntity));
        }

        private void RegisterMercenarySkills()
        {
            SkillLocator mercSkillLocator = mercenaryRef.GetComponent<SkillLocator>();
            SkillFamily mercSkillFamilyUtility = mercSkillLocator.utility.skillFamily;

            //Slashport
            MercSlashportDef skillDefSlashport = ScriptableObject.CreateInstance<MercSlashportDef>();
            skillDefSlashport.activationState = new SerializableEntityStateType(typeof(SlashportEntity));
            skillDefSlashport.activationStateMachineName = "Weapon";
            skillDefSlashport.baseMaxStock = 1;
            skillDefSlashport.baseRechargeInterval = 5f;
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
            skillDefSlashport.icon = Resources.Sprites.slashportIconS;
            skillDefSlashport.skillDescriptionToken = "MERCENARY_UTILITY_SLASHPORT_DESC";
            skillDefSlashport.skillName = "Slashport";
            skillDefSlashport.skillNameToken = "MERCENARY_UTILITY_SLASHPORT_NAME";
            skillDefSlashport.keywordTokens = new string[]
            {
                "KEYWORD_EXPOSE",
                "KEYWORD_STUNNING"
            };

            defList.Add(skillDefSlashport);
            Array.Resize(ref mercSkillFamilyUtility.variants, mercSkillFamilyUtility.variants.Length + 1);
            mercSkillFamilyUtility.variants[mercSkillFamilyUtility.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefSlashport,
                unlockableDef = UnlocksRegistering.mercSlashportUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefSlashport.skillNameToken, false, null)
            };
        }

        private void RegisterCommandoSkills()
        {
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
            skillDefCombatshotgun.icon = Resources.Sprites.shotgunIconS;
            skillDefCombatshotgun.skillDescriptionToken = "COMMANDO_PRIMARY_COMBATSHOTGUN_DESC";
            skillDefCombatshotgun.skillName = "CombatShotgun";
            skillDefCombatshotgun.skillNameToken = "COMMANDO_PRIMARY_COMBATSHOTGUN_NAME";

            defList.Add(skillDefCombatshotgun);
            Array.Resize(ref commandoSkillFamilyPrimary.variants, commandoSkillFamilyPrimary.variants.Length + 1);
            commandoSkillFamilyPrimary.variants[commandoSkillFamilyPrimary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefCombatshotgun,
                unlockableDef = UnlocksRegistering.commandoShotgunUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefCombatshotgun.skillNameToken, false, null)
            };

            //Dash
            SkillDef skillDefDash = ScriptableObject.CreateInstance<SkillDef>();
            skillDefDash.activationState = new SerializableEntityStateType(typeof(CommandoDashEntity));
            skillDefDash.activationStateMachineName = "Body";
            skillDefDash.baseMaxStock = 2;
            skillDefDash.baseRechargeInterval = 8f;
            skillDefDash.beginSkillCooldownOnSkillEnd = true;
            skillDefDash.fullRestockOnAssign = false;
            skillDefDash.interruptPriority = InterruptPriority.PrioritySkill;
            skillDefDash.isCombatSkill = false;
            skillDefDash.mustKeyPress = true;
            skillDefDash.canceledFromSprinting = false;
            skillDefDash.cancelSprintingOnActivation = false;
            skillDefDash.forceSprintDuringState = false;
            skillDefDash.stockToConsume = 1;
            skillDefDash.requiredStock = 1;
            skillDefDash.rechargeStock = 1;
            skillDefDash.icon = Resources.Sprites.dashIconS;
            skillDefDash.skillDescriptionToken = "COMMANDO_UTILITY_DASH_DESC";
            skillDefDash.skillName = "Dash";
            skillDefDash.skillNameToken = "COMMANDO_UTILITY_DASH_NAME";
            skillDefDash.keywordTokens = new string[]
            {
                "KEYWORD_AGILE",
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
        }

        private void RegisterCaptainSkills()
        {
            SkillLocator captainSkillLocator = captainRef.GetComponent<SkillLocator>();
            SkillFamily captainSkillFamilySecondary = captainSkillLocator.secondary.skillFamily;

            //DebuffGrenade
            SkillDef skillDefDebuffnade = ScriptableObject.CreateInstance<SkillDef>();
            skillDefDebuffnade.activationState = new SerializableEntityStateType(typeof(DebuffGrenadeEntity));
            skillDefDebuffnade.activationStateMachineName = "Weapon";
            skillDefDebuffnade.baseMaxStock = 2;
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
            skillDefDebuffnade.icon = Resources.Sprites.debuffNadeIconS;
            skillDefDebuffnade.skillDescriptionToken = "CAPTAIN_SECONDARY_DEBUFFNADE_DESC";
            skillDefDebuffnade.skillName = "Debuffnade";
            skillDefDebuffnade.skillNameToken = "CAPTAIN_SECONDARY_DEBUFFNADE_NAME";
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
        }

        private void RegisterEngiSkills()
        {
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
            skillDefTeslamine.icon = Resources.Sprites.teslaMineIconS;
            skillDefTeslamine.skillDescriptionToken = "ENGI_SECONDARY_TESLAMINE_DESC";
            skillDefTeslamine.skillName = "TeslaMine";
            skillDefTeslamine.skillNameToken = "ENGI_SECONDARY_TESLAMINE_NAME";
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
            RegisterTeslaMineStates();
        }

        private void RegisterRexSkills()
        {
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
            skillDefRoot.icon = Resources.Sprites.rexrootIconS;
            skillDefRoot.skillDescriptionToken = "REX_SPECIAL_ROOT_DESC";
            skillDefRoot.skillName = "Root";
            skillDefRoot.skillNameToken = "REX_SPECIAL_ROOT_NAME";
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
        }

        private void RegisterLoaderSkills()
        {
            SkillLocator loaderSkillLocator = loaderRef.GetComponent<SkillLocator>();
            SkillFamily loaderSkillFamilySpecial = loaderSkillLocator.special.skillFamily;

            //Shieldsplosion
            ShieldsplosionDef skillDefShieldsplode = ScriptableObject.CreateInstance<ShieldsplosionDef>();
            skillDefShieldsplode.activationState = new SerializableEntityStateType(typeof(ShieldSplosionEntity));
            skillDefShieldsplode.activationStateMachineName = "Weapon";
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
            skillDefShieldsplode.icon = Resources.Sprites.shieldsplosionIconS;
            skillDefShieldsplode.skillDescriptionToken = "LOADER_SPECIAL_SHIELDSPLOSION_DESC";
            skillDefShieldsplode.skillName = "ShieldSplosion";
            skillDefShieldsplode.skillNameToken = "LOADER_SPECIAL_SHIELDSPLOSION_NAME";

            defList.Add(skillDefShieldsplode);
            Array.Resize(ref loaderSkillFamilySpecial.variants, loaderSkillFamilySpecial.variants.Length + 1);
            loaderSkillFamilySpecial.variants[loaderSkillFamilySpecial.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefShieldsplode,
                unlockableDef = UnlocksRegistering.loaderShieldsplosionUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefShieldsplode.skillNameToken, false, null)
            };
        }

        private void RegisterAcridSkills()
        {
            SkillLocator acridSkillLocator = acridRef.GetComponent<SkillLocator>();
            SkillFamily acridSkillFamilySpecial = acridSkillLocator.special.skillFamily;

            //AcridPurge
            AcridPurgeDef skillDefExpunge = ScriptableObject.CreateInstance<AcridPurgeDef>();
            skillDefExpunge.activationState = new SerializableEntityStateType(typeof(AcridPurgeEntity));
            skillDefExpunge.activationStateMachineName = "Weapon";
            skillDefExpunge.baseMaxStock = 1;
            skillDefExpunge.baseRechargeInterval = 12f;
            skillDefExpunge.beginSkillCooldownOnSkillEnd = false;
            skillDefExpunge.fullRestockOnAssign = false;
            skillDefExpunge.interruptPriority = InterruptPriority.Skill;
            skillDefExpunge.isCombatSkill = true;
            skillDefExpunge.mustKeyPress = true;
            skillDefExpunge.canceledFromSprinting = false;
            skillDefExpunge.cancelSprintingOnActivation = false;
            skillDefExpunge.forceSprintDuringState = false;
            skillDefExpunge.rechargeStock = 1;
            skillDefExpunge.requiredStock = 1;
            skillDefExpunge.stockToConsume = 1;
            skillDefExpunge.icon = Resources.Sprites.acridpurgeIconS;
            skillDefExpunge.skillDescriptionToken = "ACRID_SPECIAL_PURGE_DESC";
            skillDefExpunge.skillName = "Purge";
            skillDefExpunge.skillNameToken = "ACRID_SPECIAL_PURGE_NAME";

            defList.Add(skillDefExpunge);
            Array.Resize(ref acridSkillFamilySpecial.variants, acridSkillFamilySpecial.variants.Length + 1);
            acridSkillFamilySpecial.variants[acridSkillFamilySpecial.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefExpunge,
                unlockableDef = UnlocksRegistering.acridExpungeUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefExpunge.skillNameToken, false, null)
            };
        }

        private void RegisterBanditSkills()
        {
            SkillLocator banditSkillLocator = banditRef.GetComponent<SkillLocator>();
            SkillFamily banditSkillFamilyUtility = banditSkillLocator.utility.skillFamily;
            SkillFamily banditSkillFamilyPrimary = banditSkillLocator.primary.skillFamily;

            //Thieves Cunning
            InvisOnSprintSkillDef skillDefInvisSprint = ScriptableObject.CreateInstance<InvisOnSprintSkillDef>();
            skillDefInvisSprint.baseMaxStock = 1;
            skillDefInvisSprint.baseRechargeInterval = 6f;
            skillDefInvisSprint.fullRestockOnAssign = false;
            skillDefInvisSprint.rechargeStock = 1;
            skillDefInvisSprint.requiredStock = 1;
            skillDefInvisSprint.stockToConsume = 1;
            skillDefInvisSprint.icon = Resources.Sprites.invisSprintIconS;
            skillDefInvisSprint.skillDescriptionToken = "BANDIT_UTILITY_INVISSPRINT_DESC";
            skillDefInvisSprint.skillName = "ThievesCunning";
            skillDefInvisSprint.skillNameToken = "BANDIT_UTILITY_INVISSPRINT_NAME";

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
            skillDefMagicBullet.icon = Resources.Sprites.magicBulletIconS;
            skillDefMagicBullet.skillDescriptionToken = "BANDIT_PRIMARY_MAGICBULLET_DESC";
            skillDefMagicBullet.skillName = "MagicBullet";
            skillDefMagicBullet.skillNameToken = "BANDIT_PRIMARY_MAGICBULLET_NAME";
            skillDefMagicBullet.keywordTokens = new string[]
            {
                "KEYWORD_LUCKY"
            };

            defList.Add(skillDefMagicBullet);
            Array.Resize(ref banditSkillFamilyPrimary.variants, banditSkillFamilyPrimary.variants.Length + 1);
            banditSkillFamilyPrimary.variants[banditSkillFamilyPrimary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefMagicBullet,
                unlockableDef = UnlocksRegistering.banditMagicBulletUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefMagicBullet.skillNameToken, false, null)
            };
        }

        private void RegisterHuntressSkills()
        {
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
            skillDefClusterArrow.icon = Resources.Sprites.clusterArrowIconS;
            skillDefClusterArrow.skillDescriptionToken = "HUNTRESS_SECONDARY_CLUSTERARROW_DESC";
            skillDefClusterArrow.skillName = "ClusterArrow";
            skillDefClusterArrow.skillNameToken = "HUNTRESS_SECONDARY_CLUSTERARROW_NAME";
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
        }

        private void RegisterMultSkills()
        {
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
            skillDefNanoSwarm.icon = Resources.Sprites.nanoBotsIconS;
            skillDefNanoSwarm.skillDescriptionToken = "MULT_SECONDARY_NANOBOT_DESC";
            skillDefNanoSwarm.skillName = "NanobotSwarm";
            skillDefNanoSwarm.skillNameToken = "MULT_SECONDARY_NANOBOT_NAME";
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
        }


        private void RegisterTeslaMineStates()
        {
            extraStates.Add(typeof(TeslaArmingUnarmedState));
            extraStates.Add(typeof(TeslaArmingWeakState));
            extraStates.Add(typeof(TeslaArmingFullState));

            extraStates.Add(typeof(TeslaArmState));
            extraStates.Add(typeof(TeslaWaitForStick));
            extraStates.Add(typeof(TeslaWaitForTargetState));
            extraStates.Add(typeof(TeslaPreDetState));
            extraStates.Add(typeof(TeslaDetonateState));
        }
    }
}