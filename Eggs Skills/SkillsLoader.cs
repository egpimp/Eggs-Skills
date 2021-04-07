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
using EnigmaticThunder.Modules;
using System.Security;
using System.Security.Permissions;
using EggsSkills.EntityStates;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace EggsSkills
{
    [BepInDependency("com.Egg.EggsBuffs", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.EnigmaDev.EnigmaticThunder", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("com.Egg.EggsSkills", "Eggs Skills", "1.0.3")]
    public class SkillsLoader : BaseUnityPlugin
    {
        GameObject artificerRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/MageBody");
        GameObject mercenaryRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/MercBody");
        GameObject commandoRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/CommandoBody");
        GameObject engineerRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/EngiBody");
        GameObject rexRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/TreebotBody");
        GameObject loaderRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/LoaderBody");
        GameObject acridRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/CrocoBody");
        GameObject captainRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/CaptainBody");
        private void Awake()
        {
            Assets.ProjectileLoader();
            Assets.RegisterLanguageTokens();
            RegisterSkills();
        }
        private void RegisterSkills()
        {
            RegisterArtificerSkills();
            RegisterMercenarySkills();
            RegisterCommandoSkills();
            RegisterEngiSkills();
            RegisterAcridSkills();
            RegisterLoaderSkills();
            RegisterCaptainSkills();
            RegisterRexSkills();
        }

        private void RegisterArtificerSkills()
        {
            SkillLocator artificerSkillLocator = artificerRef.GetComponent<SkillLocator>();
            SkillFamily artificerSkillFamilyUtility = artificerSkillLocator.utility.skillFamily;

            //Zapport
            SkillDef skillDefZapport = ScriptableObject.CreateInstance<SkillDef>();
            skillDefZapport.activationState = new SerializableEntityStateType(typeof(EntityStates.ZapportChargeEntity));
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
            skillDefZapport.icon = Assets.zapportIconS;
            skillDefZapport.skillDescriptionToken = "ARTIFICER_UTILITY_ZAPPORT_DESC";
            skillDefZapport.skillName = "Zapport";
            skillDefZapport.skillNameToken = "ARTIFICER_UTILITY_ZAPPORT_NAME";
            skillDefZapport.keywordTokens = new string[]
            {
                "KEYWORD_STUNNING"
            };

            Loadouts.RegisterSkillDef(skillDefZapport);
            Array.Resize(ref artificerSkillFamilyUtility.variants, artificerSkillFamilyUtility.variants.Length + 1);
            artificerSkillFamilyUtility.variants[artificerSkillFamilyUtility.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefZapport,
                unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>(),
                viewableNode = new ViewablesCatalog.Node(skillDefZapport.skillNameToken, false, null)
            };
            Loadouts.RegisterEntityState(typeof(ZapportFireEntity));
        }

        private void RegisterMercenarySkills()
        {
            SkillLocator mercSkillLocator = mercenaryRef.GetComponent<SkillLocator>();
            SkillFamily mercSkillFamilyUtility = mercSkillLocator.utility.skillFamily;

            //Slashport
            MercSlashportDef skillDefSlashport = ScriptableObject.CreateInstance<MercSlashportDef>();
            skillDefSlashport.activationState = new SerializableEntityStateType(typeof(EntityStates.SlashportEntity));
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
            skillDefSlashport.icon = Assets.slashportIconS;
            skillDefSlashport.skillDescriptionToken = "MERCENARY_UTILITY_SLASHPORT_DESC";
            skillDefSlashport.skillName = "Slashport";
            skillDefSlashport.skillNameToken = "MERCENARY_UTILITY_SLASHPORT_NAME";
            skillDefSlashport.keywordTokens = new string[]
            {
                "KEYWORD_EXPOSE",
                "KEYWORD_STUNNING"
            };

            Loadouts.RegisterSkillDef(skillDefSlashport);
            Array.Resize(ref mercSkillFamilyUtility.variants, mercSkillFamilyUtility.variants.Length + 1);
            mercSkillFamilyUtility.variants[mercSkillFamilyUtility.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefSlashport,
                unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>(),
                viewableNode = new ViewablesCatalog.Node(skillDefSlashport.skillNameToken, false, null)
            };
        }

        private void RegisterCommandoSkills()
        {
            SkillLocator commandoSkillLocator = commandoRef.GetComponent<SkillLocator>();
            SkillFamily commandoSkillFamilyPrimary = commandoSkillLocator.primary.skillFamily;

            //Combat Shotgun
            SkillDef skillDefCombatshotgun = ScriptableObject.CreateInstance<SkillDef>();
            skillDefCombatshotgun.activationState = new SerializableEntityStateType(typeof(EggsSkills.EntityStates.CombatShotgunEntity));
            skillDefCombatshotgun.activationStateMachineName = "Weapon";
            skillDefCombatshotgun.baseMaxStock = 8;
            skillDefCombatshotgun.baseRechargeInterval = 0.8f;
            skillDefCombatshotgun.beginSkillCooldownOnSkillEnd = true;
            skillDefCombatshotgun.fullRestockOnAssign = true;
            skillDefCombatshotgun.interruptPriority = InterruptPriority.Any;
            skillDefCombatshotgun.isCombatSkill = true;
            skillDefCombatshotgun.mustKeyPress = false;
            skillDefCombatshotgun.canceledFromSprinting = false;
            skillDefCombatshotgun.cancelSprintingOnActivation = true;
            skillDefCombatshotgun.forceSprintDuringState = false;
            skillDefCombatshotgun.rechargeStock = 8;
            skillDefCombatshotgun.requiredStock = 1;
            skillDefCombatshotgun.stockToConsume = 1;
            skillDefCombatshotgun.icon = Assets.shotgunIconS;
            skillDefCombatshotgun.skillDescriptionToken = "COMMANDO_PRIMARY_COMBATSHOTGUN_DESC";
            skillDefCombatshotgun.skillName = "CombatShotgun";
            skillDefCombatshotgun.skillNameToken = "COMMANDO_PRIMARY_COMBATSHOTGUN_NAME";

            Loadouts.RegisterSkillDef(skillDefCombatshotgun);
            Array.Resize(ref commandoSkillFamilyPrimary.variants, commandoSkillFamilyPrimary.variants.Length + 1);
            commandoSkillFamilyPrimary.variants[commandoSkillFamilyPrimary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefCombatshotgun,
                unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>(),
                viewableNode = new ViewablesCatalog.Node(skillDefCombatshotgun.skillNameToken, false, null)
            };
        }

        private void RegisterCaptainSkills()
        {
            SkillLocator captainSkillLocator = captainRef.GetComponent<SkillLocator>();
            SkillFamily captainSkillFamilySecondary = captainSkillLocator.secondary.skillFamily;

            //DebuffGrenade
            SkillDef skillDefDebuffnade = ScriptableObject.CreateInstance<SkillDef>();
            skillDefDebuffnade.activationState = new SerializableEntityStateType(typeof(EggsSkills.EntityStates.DebuffGrenadeEntity));
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
            skillDefDebuffnade.icon = Assets.debuffNadeIconS;
            skillDefDebuffnade.skillDescriptionToken = "CAPTAIN_SECONDARY_DEBUFFNADE_DESC";
            skillDefDebuffnade.skillName = "Debuffnade";
            skillDefDebuffnade.skillNameToken = "CAPTAIN_SECONDARY_DEBUFFNADE_NAME";
            skillDefDebuffnade.keywordTokens = new string[]
            {
                "KEYWORD_MARKING",
            };

            Loadouts.RegisterSkillDef(skillDefDebuffnade);
            Array.Resize(ref captainSkillFamilySecondary.variants, captainSkillFamilySecondary.variants.Length + 1);
            captainSkillFamilySecondary.variants[captainSkillFamilySecondary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefDebuffnade,
                unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>(),
                viewableNode = new ViewablesCatalog.Node(skillDefDebuffnade.skillNameToken, false, null)
            };
        }

        private void RegisterEngiSkills()
        {
            SkillLocator engiSkillLocator = engineerRef.GetComponent<SkillLocator>();
            SkillFamily engiSkillFamilySecondary = engiSkillLocator.secondary.skillFamily;

            SkillDef skillDefTeslamine = ScriptableObject.CreateInstance<SkillDef>();
            skillDefTeslamine.activationState = new SerializableEntityStateType(typeof(EggsSkills.EntityStates.TeslaMineFireState));
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
            skillDefTeslamine.icon = Assets.teslaMineIconS;
            skillDefTeslamine.skillDescriptionToken = "ENGI_SECONDARY_TESLAMINE_DESC";
            skillDefTeslamine.skillName = "TeslaMine";
            skillDefTeslamine.skillNameToken = "ENGI_SECONDARY_TESLAMINE_NAME";
            skillDefTeslamine.keywordTokens = new string[]
            {
                "KEYWORD_STUNNING"
            };

            Loadouts.RegisterSkillDef(skillDefTeslamine);
            Array.Resize(ref engiSkillFamilySecondary.variants, engiSkillFamilySecondary.variants.Length + 1);
            engiSkillFamilySecondary.variants[engiSkillFamilySecondary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefTeslamine,
                unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>(),
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
            skillDefRoot.activationState = new SerializableEntityStateType(typeof(EggsSkills.EntityStates.DirectiveRoot));
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
            skillDefRoot.icon = Assets.rexrootIconS;
            skillDefRoot.skillDescriptionToken = "REX_SPECIAL_ROOT_DESC";
            skillDefRoot.skillName = "Root";
            skillDefRoot.skillNameToken = "REX_SPECIAL_ROOT_NAME";
            skillDefRoot.keywordTokens = new string[]
            {
                "KEYWORD_STUNNING",
            };

            Loadouts.RegisterSkillDef(skillDefRoot);
            Array.Resize(ref rexSkillFamilySpecial.variants, rexSkillFamilySpecial.variants.Length + 1);
            rexSkillFamilySpecial.variants[rexSkillFamilySpecial.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefRoot,
                unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>(),
                viewableNode = new ViewablesCatalog.Node(skillDefRoot.skillNameToken, false, null)
            };
        }

        private void RegisterLoaderSkills()
        {
            SkillLocator loaderSkillLocator = loaderRef.GetComponent<SkillLocator>();
            SkillFamily loaderSkillFamilySpecial = loaderSkillLocator.special.skillFamily;

            //Shieldsplosion
            ShieldsplosionDef skillDefShieldsplode = ScriptableObject.CreateInstance<ShieldsplosionDef>();
            skillDefShieldsplode.activationState = new SerializableEntityStateType(typeof(EggsSkills.EntityStates.ShieldSplosionEntity));
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
            skillDefShieldsplode.icon = Assets.shieldsplosionIconS;
            skillDefShieldsplode.skillDescriptionToken = "LOADER_SPECIAL_SHIELDSPLOSION_DESC";
            skillDefShieldsplode.skillName = "ShieldSplosion";
            skillDefShieldsplode.skillNameToken = "LOADER_SPECIAL_SHIELDSPLOSION_NAME";

            Loadouts.RegisterSkillDef(skillDefShieldsplode);
            Array.Resize(ref loaderSkillFamilySpecial.variants, loaderSkillFamilySpecial.variants.Length + 1);
            loaderSkillFamilySpecial.variants[loaderSkillFamilySpecial.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefShieldsplode,
                unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>(),
                viewableNode = new ViewablesCatalog.Node(skillDefShieldsplode.skillNameToken, false, null)
            };
        }

        private void RegisterAcridSkills()
        {
            SkillLocator acridSkillLocator = acridRef.GetComponent<SkillLocator>();
            SkillFamily acridSkillFamilySpecial = acridSkillLocator.special.skillFamily;

            //AcridPurge
            AcridPurgeDef skillDefExpunge = ScriptableObject.CreateInstance<AcridPurgeDef>();
            skillDefExpunge.activationState = new SerializableEntityStateType(typeof(EggsSkills.EntityStates.AcridPurgeEntity));
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
            skillDefExpunge.icon = Assets.acridpurgeIconS;
            skillDefExpunge.skillDescriptionToken = "ACRID_SPECIAL_PURGE_DESC";
            skillDefExpunge.skillName = "Purge";
            skillDefExpunge.skillNameToken = "ACRID_SPECIAL_PURGE_NAME";

            Loadouts.RegisterSkillDef(skillDefExpunge);
            Array.Resize(ref acridSkillFamilySpecial.variants, acridSkillFamilySpecial.variants.Length + 1);
            acridSkillFamilySpecial.variants[acridSkillFamilySpecial.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefExpunge,
                unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>(),
                viewableNode = new ViewablesCatalog.Node(skillDefExpunge.skillNameToken, false, null)
            };
        }

        private void RegisterTeslaMineStates()
        {
            Loadouts.RegisterEntityState(typeof(TeslaArmingUnarmedState));
            Loadouts.RegisterEntityState(typeof(TeslaArmingWeakState));
            Loadouts.RegisterEntityState(typeof(TeslaArmingFullState));

            Loadouts.RegisterEntityState(typeof(TeslaArmState));
            Loadouts.RegisterEntityState(typeof(TeslaWaitForStick));
            Loadouts.RegisterEntityState(typeof(TeslaWaitForTargetState));
            Loadouts.RegisterEntityState(typeof(TeslaPreDetState));
            Loadouts.RegisterEntityState(typeof(TeslaDetonateState));
        }
    }
}