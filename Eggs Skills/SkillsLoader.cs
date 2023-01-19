using System;
using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Skills;
using EggsSkills.SkillDefs;
using EggsSkills.EntityStates.TeslaMine.MineStates.MainStates;
using EggsSkills.EntityStates.TeslaMine.MineStates.ArmingStates;
using R2API;
using EggsSkills.EntityStates;
using EggsSkills.Unlocks;
using System.Collections.Generic;
using EntityStates.Bandit2.Weapon;
using static EggsSkills.Config.Configuration;
using EggsSkills.Resources;
using System.Runtime.CompilerServices;
using static EggsSkills.EggsSkills;
using UnityEngine.AddressableAssets;

namespace EggsSkills
{
    internal class SkillsLoader
    {
        #region Characterbody References

        //Nab artificer body
        internal static GameObject artificerRef = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageBody.prefab").WaitForCompletion();
        //Nab merc body
        internal static GameObject mercenaryRef = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Merc/MercBody.prefab").WaitForCompletion();
        //Nab commando body
        internal static GameObject commandoRef = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion();
        //Nab engi body
        internal static GameObject engineerRef = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Engi/EngiBody.prefab").WaitForCompletion();
        //Nab REX body
        internal static GameObject rexRef = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Treebot/TreebotBody.prefab").WaitForCompletion();
        //Nab loader body
        internal static GameObject loaderRef = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Loader/LoaderBody.prefab").WaitForCompletion();
        //Nab acrid body
        internal static GameObject acridRef = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Croco/CrocoBody.prefab").WaitForCompletion();
        //Nab captain body
        internal static GameObject captainRef = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Captain/CaptainBody.prefab").WaitForCompletion();
        //Nab bandit body
        internal static GameObject banditRef = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/Bandit2Body.prefab").WaitForCompletion();
        //Nab MUL-T body
        internal static GameObject multRef = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Toolbot/ToolbotBody.prefab").WaitForCompletion();
        //Nab huntress body
        internal static GameObject huntressRef = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Huntress/HuntressBody.prefab").WaitForCompletion();
        //Nab railgunner Body
        internal static GameObject railgunnerRef = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/RailgunnerBody.prefab").WaitForCompletion();
        //Nab void fiend body
        internal static GameObject voidFiendRef = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBody.prefab").WaitForCompletion();
        #endregion

        //List of all the skilldefs so we don't have to change every skill add case when r2api changes
        internal static List<SkillDef> defList = new List<SkillDef>();
        
        //Helps us add the scepter compat
        internal struct SkillUpgradeContainer
        {
            internal string body;
            internal SkillDef normalSkillDef;
            internal SkillDef upgradedSkillDef;
            internal SkillSlot slot;
            internal int index;
        }
        internal static Dictionary<string, SkillUpgradeContainer> skillsToHandle;

        //Main method for setting up all our skills
        internal static void RegisterSkills()
        {
            //For each character, if the configvalue says they are allowed to load up their skills load them, otherwise don't.  Simple.
            //Artificer
            try { if (GetConfigValue(EnableMageSkills)) RegisterArtificerSkills(); } catch { Log.LogError("Failed to load Artificer skills"); }
            //Mercenary
            try { if (GetConfigValue(EnableMercSkills)) RegisterMercenarySkills(); } catch { Log.LogError("Failed to load Mercenary skills"); }
            //Commando
            try { if (GetConfigValue(EnableCommandoSkills)) RegisterCommandoSkills(); } catch { Log.LogError("Failed to load Commando skills"); }
            //Engineer
            try { if (GetConfigValue(EnableEngiSkills)) RegisterEngiSkills(); } catch { Log.LogError("Failed to load Engineer skills"); }
            //Acrid
            try { if (GetConfigValue(EnableCrocoSkills)) RegisterAcridSkills(); } catch { Log.LogError("Failed to load Acrid skills"); }
            //Loader
            try { if (GetConfigValue(EnableLoaderSkills)) RegisterLoaderSkills(); } catch { Log.LogError("Failed to load Loader skills"); }
            //Captain
            try { if (GetConfigValue(EnableCaptainSkills)) RegisterCaptainSkills(); } catch { Log.LogError("Failed to load Captain skills"); }
            //REX
            try { if (GetConfigValue(EnableTreebotSkills)) RegisterRexSkills(); } catch { Log.LogError("Failed to load REX skills"); }
            //Bandit
            try { if (GetConfigValue(EnableBanditSkills)) RegisterBanditSkills(); } catch { Log.LogError("Failed to load Bandit skills"); }
            //Huntress
            try { if (GetConfigValue(EnableHuntressSkills)) RegisterHuntressSkills(); } catch { Log.LogError("Failed to laod Huntress skills"); }
            //MUL-T
            try { if (GetConfigValue(EnableToolbotSkills)) RegisterMultSkills(); } catch { Log.LogError("Failed to load MUL-T skills"); }
            //Railgunner
            try { if (GetConfigValue(EnableRailgunnerSkills)) RegisterRailgunnerSkills(); } catch { Log.LogError("Failed to load Railgunner skills"); }
            //Void fiend
            try { if (GetConfigValue(EnableVoidfiendSkills)) RegisterVoidfiendSkills(); } catch { Log.LogError("Failed to load Void Fiend skills"); }

            //Load scepter skills if supposed to
            try { if (classicItemsLoaded) RegisterScepterSkills(); } catch { Log.LogError("Failed to load one or more scepter replacements"); }

            //Register any states that aren't directly tied to skill activation (Basically, don't need skilldefs)
            try { RegisterExtraStates(); } catch { Log.LogError("Failed to load one or more extra entity-states"); }

            //As long as there are any skilldefs waiting to be added...
            if (defList.Count > 0)
            {
                //For every skilldef queue'd up to be added...
                foreach (SkillDef def in defList)
                {
                    try
                    {
                        //Add skilldef via R2API
                        ContentAddition.AddSkillDef(def);
                        //Tell us each time a skill is registered, helps with sanity checks
                        Log.LogMessage("Skill: " + def.skillName + " Registered");
                    }
                    catch
                    {
                        Log.LogError("Skilldef failed to be registered");
                    }
                }
            }
            //Sadness check
            else Log.LogMessage("Did you really install my mod just to disable all the skills :(");
        }
        #region Skills
        private static void RegisterAcridSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator acridSkillLocator = acridRef.GetComponent<SkillLocator>();
            SkillFamily acridSkillFamilySpecial = acridSkillLocator.special.skillFamily;
            SkillFamily acricSkillFamilyPrimary = acridSkillLocator.primary.skillFamily;

            //AcridPurge
            AcridPurgeDef skillDefPurge = ScriptableObject.CreateInstance<AcridPurgeDef>();
            skillDefPurge.activationState = new SerializableEntityStateType(typeof(AcridPurgeEntity));
            skillDefPurge.activationStateMachineName = "Body";
            skillDefPurge.baseMaxStock = 1;
            skillDefPurge.baseRechargeInterval = 16f;
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
            skillDefPurge.skillDescriptionToken = "ES_CROCO_SPECIAL_PURGE_DESCRIPTION";
            skillDefPurge.skillName = ((ScriptableObject)skillDefPurge).name = "ESPurge";
            skillDefPurge.skillNameToken = "ES_CROCO_SPECIAL_PURGE_NAME";

            defList.Add(skillDefPurge);
            Array.Resize(ref acridSkillFamilySpecial.variants, acridSkillFamilySpecial.variants.Length + 1);
            acridSkillFamilySpecial.variants[acridSkillFamilySpecial.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefPurge,
                unlockableDef = UnlocksRegistering.acridPurgeUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefPurge.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<AcridPurgeEntityUpgrade>(out _);

            if(classicItemsLoaded) skillsToHandle.Add(skillDefPurge.skillName, new SkillUpgradeContainer { normalSkillDef = skillDefPurge, body = "Croco", slot = SkillSlot.Special, index = acridSkillFamilySpecial.variants.Length - 1});
        }

        private static void RegisterArtificerSkills()
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
            skillDefZapport.skillDescriptionToken = "ES_MAGE_UTILITY_ZAPPORT_DESCRIPTION";
            skillDefZapport.skillName = ((ScriptableObject)skillDefZapport).name = "ESZapport";
            skillDefZapport.skillNameToken = "ES_MAGE_UTILITY_ZAPPORT_NAME";
            skillDefZapport.keywordTokens = new string[]
            {
                "KEYWORD_STUNNING",
                "ES_KEYWORD_ENHANCING"
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

        private static void RegisterBanditSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator banditSkillLocator = banditRef.GetComponent<SkillLocator>();
            SkillFamily banditSkillFamilyUtility = banditSkillLocator.utility.skillFamily;
            SkillFamily banditSkillFamilyPrimary = banditSkillLocator.primary.skillFamily;

            //Thieves Cunning
            InvisOnSprintSkillDef skillDefInvisSprint = ScriptableObject.CreateInstance<InvisOnSprintSkillDef>();
            //These two are dummies, don't actually exist, just there to stop errors
            skillDefInvisSprint.activationState = new SerializableEntityStateType(typeof(InvisDummyState));
            skillDefInvisSprint.activationStateMachineName = "Body";
            skillDefInvisSprint.baseMaxStock = 1;
            skillDefInvisSprint.baseRechargeInterval = 6f;
            skillDefInvisSprint.fullRestockOnAssign = false;
            skillDefInvisSprint.rechargeStock = 1;
            skillDefInvisSprint.requiredStock = 1;
            skillDefInvisSprint.stockToConsume = 1;
            skillDefInvisSprint.icon = Sprites.invisSprintIconS;
            skillDefInvisSprint.skillDescriptionToken = "ES_BANDIT2_UTILITY_INVISSPRINT_DESCRIPTION";
            skillDefInvisSprint.skillName = ((ScriptableObject)skillDefInvisSprint).name = "ESInvisSprint";
            skillDefInvisSprint.skillNameToken = "ES_BANDIT2_UTILITY_INVISSPRINT_NAME";

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
            skillDefMagicBullet.skillDescriptionToken = "ES_BANDIT2_PRIMARY_MAGICBULLET_DESCRIPTION";
            skillDefMagicBullet.skillName = ((ScriptableObject)skillDefMagicBullet).name = "ESMagicBullet";
            skillDefMagicBullet.skillNameToken = "ES_BANDIT2_PRIMARY_MAGICBULLET_NAME";

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

        private static void RegisterCaptainSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator captainSkillLocator = captainRef.GetComponent<SkillLocator>();
            SkillFamily captainSkillFamilySecondary = captainSkillLocator.secondary.skillFamily;
            SkillFamily captainSkillFamilyPrimary = captainSkillLocator.primary.skillFamily;

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
            skillDefDebuffnade.skillDescriptionToken = "ES_CAPTAIN_SECONDARY_DEBUFFNADE_DESCRIPTION";
            skillDefDebuffnade.skillName = ((ScriptableObject)skillDefDebuffnade).name = "ESDebuffNade";
            skillDefDebuffnade.skillNameToken = "ES_CAPTAIN_SECONDARY_DEBUFFNADE_NAME";
            skillDefDebuffnade.keywordTokens = new string[]
            {
                "ES_KEYWORD_MARKING",
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

            //Autoshotgun
            
        }

        private static void RegisterCommandoSkills()
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
            skillDefCombatshotgun.skillDescriptionToken = "ES_COMMANDO_PRIMARY_COMBATSHOTGUN_DESCRIPTION";
            skillDefCombatshotgun.skillName = ((ScriptableObject)skillDefCombatshotgun).name = "ESCombatShotgun";
            skillDefCombatshotgun.skillNameToken = "ES_COMMANDO_PRIMARY_COMBATSHOTGUN_NAME";

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
            skillDefDash.skillDescriptionToken = "ES_COMMANDO_UTILITY_DASH_DESCRIPTION";
            skillDefDash.skillName = ((ScriptableObject)skillDefDash).name = "ESDash";
            skillDefDash.skillNameToken = "ES_COMMANDO_UTILITY_DASH_NAME";
            skillDefDash.keywordTokens = new string[]
            {
                 "ES_KEYWORD_PREPARE"
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

        private static void RegisterEngiSkills()
        {
            //Nab the skillocator and skillfamilies
            SkillLocator engiSkillLocator = engineerRef.GetComponent<SkillLocator>();
            SkillFamily engiSkillFamilySecondary = engiSkillLocator.secondary.skillFamily;
            SkillFamily engiSkillFamilyPrimary = engiSkillLocator.primary.skillFamily;

            SkillDef skillDefTeslamine = ScriptableObject.CreateInstance<SkillDef>();
            skillDefTeslamine.activationState = new SerializableEntityStateType(typeof(TeslaMineFireState));
            skillDefTeslamine.activationStateMachineName = "Weapon";
            skillDefTeslamine.baseMaxStock = 4;
            skillDefTeslamine.baseRechargeInterval = 8f;
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
            skillDefTeslamine.skillDescriptionToken = "ES_ENGI_SECONDARY_TESLAMINE_DESCRIPTION";
            skillDefTeslamine.skillName = ((ScriptableObject)skillDefTeslamine).name = "ESTeslaMine";
            skillDefTeslamine.skillNameToken = "ES_ENGI_SECONDARY_TESLAMINE_NAME";
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

        private static void RegisterHuntressSkills()
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
            skillDefClusterArrow.skillDescriptionToken = "ES_HUNTRESS_SECONDARY_CLUSTERARROW_DESCRIPTION";
            skillDefClusterArrow.skillName = ((ScriptableObject)skillDefClusterArrow).name = "ESClusterArrow";
            skillDefClusterArrow.skillNameToken = "ES_HUNTRESS_SECONDARY_CLUSTERARROW_NAME";
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

        private static void RegisterLoaderSkills()
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
            skillDefShieldsplode.skillDescriptionToken = "ES_LOADER_SPECIAL_SHIELDSPLOSION_DESCRIPTION";
            skillDefShieldsplode.skillName = ((ScriptableObject)skillDefShieldsplode).name = "ESShieldSplosion";
            skillDefShieldsplode.skillNameToken = "ES_LOADER_SPECIAL_SHIELDSPLOSION_NAME";

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

        private static void RegisterMercenarySkills()
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
            skillDefSlashport.skillDescriptionToken = "ES_MERC_SPECIAL_SLASHPORT_DESCRIPTION";
            skillDefSlashport.skillName = ((ScriptableObject)skillDefSlashport).name = "ESSlashport";
            skillDefSlashport.skillNameToken = "ES_MERC_SPECIAL_SLASHPORT_NAME";
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

            if (classicItemsLoaded) skillsToHandle.Add(skillDefSlashport.skillName, new SkillUpgradeContainer { normalSkillDef = skillDefSlashport, body = "Merc", slot = SkillSlot.Special, index = mercSkillFamilySpecial.variants.Length - 1 });
        }

        private static void RegisterMultSkills()
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
            skillDefNanoSwarm.skillDescriptionToken = "ES_TOOLBOT_SECONDARY_NANOBOTS_DESCRIPTION";
            skillDefNanoSwarm.skillName = ((ScriptableObject)skillDefNanoSwarm).name = "ESNanobots";
            skillDefNanoSwarm.skillNameToken = "ES_TOOLBOT_SECONDARY_NANOBOTS_NAME";
            skillDefNanoSwarm.keywordTokens = new string[]
            {
                "ES_KEYWORD_MARKING"
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

        private static void RegisterRailgunnerSkills()
        {
            SkillLocator railgunnerSkillLocator = railgunnerRef.GetComponent<SkillLocator>();
            SkillFamily railgunnerSkillFamilyPrimary = railgunnerSkillLocator.primary.skillFamily;

            //Lance rounds
            SteppedSkillDef skillDefLanceRounds = ScriptableObject.CreateInstance<SteppedSkillDef>();
            skillDefLanceRounds.activationState = new SerializableEntityStateType(typeof(LancerRoundsEntity));
            skillDefLanceRounds.activationStateMachineName = "Weapon";
            skillDefLanceRounds.beginSkillCooldownOnSkillEnd = true;
            skillDefLanceRounds.fullRestockOnAssign = true;
            skillDefLanceRounds.interruptPriority = InterruptPriority.Any;
            skillDefLanceRounds.isCombatSkill = true;
            skillDefLanceRounds.mustKeyPress = false;
            skillDefLanceRounds.canceledFromSprinting = false;
            skillDefLanceRounds.forceSprintDuringState = false;
            skillDefLanceRounds.stockToConsume = 0;
            skillDefLanceRounds.icon = Sprites.placeholderIconS;
            skillDefLanceRounds.skillDescriptionToken = "ES_RAILGUNNER_PRIMARY_LANCEROUNDS_DESCRIPTION";
            skillDefLanceRounds.skillName = ((ScriptableObject)skillDefLanceRounds).name = "ESLanceRounds";
            skillDefLanceRounds.skillNameToken = "ES_RAILGUNNER_PRIMARY_LANCEROUNDS_NAME";

            defList.Add(skillDefLanceRounds);
            Array.Resize(ref railgunnerSkillFamilyPrimary.variants, railgunnerSkillFamilyPrimary.variants.Length + 1);
            railgunnerSkillFamilyPrimary.variants[railgunnerSkillFamilyPrimary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefLanceRounds,
                unlockableDef = UnlocksRegistering.railgunnerLanceroundsUnlockDef,
                viewableNode = new ViewablesCatalog.Node(skillDefLanceRounds.skillNameToken, false, null)
            };
            ContentAddition.AddEntityState<LancerRoundsEntity>(out _);
        }

        private static void RegisterRexSkills()
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
            skillDefRoot.skillDescriptionToken = "ES_TREEBOT_SPECIAL_ROOT_DESCRIPTION";
            skillDefRoot.skillName = ((ScriptableObject)skillDefRoot).name = "ESRoot";
            skillDefRoot.skillNameToken = "ES_TREEBOT_SPECIAL_ROOT_NAME";
            skillDefRoot.keywordTokens = new string[]
            {
                "KEYWORD_STUNNING",
                "ES_KEYWORD_ADAPTIVE"
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

            if (classicItemsLoaded) skillsToHandle.Add(skillDefRoot.skillName, new SkillUpgradeContainer { normalSkillDef = skillDefRoot, body = "Treebot", slot = SkillSlot.Special, index = rexSkillFamilySpecial.variants.Length - 1 });
        }

        private static void RegisterVoidfiendSkills()
        {
            SkillLocator voidFiendSkillLocator = voidFiendRef.GetComponent<SkillLocator>();
            SkillFamily voidFiendSkillFamilySpecial = voidFiendSkillLocator.special.skillFamily;
        }
        #endregion

        private static void RegisterExtraStates()
        {
            //Artificer extra states
            if (GetConfigValue(EnableMageSkills))
            {
                //Artificer Zappot Fire State
                ContentAddition.AddEntityState<ZapportFireEntity>(out _);
                Log.LogMessage("Zapport fire state loaded!");
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
                Log.LogMessage("Tesla mine states loaded!");
            }
        }

        private static void RegisterScepterSkills()
        {
            //Upgraded AcridPurge
            AcridPurgeDef skillDefPurge = ScriptableObject.CreateInstance<AcridPurgeDef>();
            skillDefPurge.activationState = new SerializableEntityStateType(typeof(AcridPurgeEntityUpgrade));
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
            skillDefPurge.icon = Sprites.acridpurgeUpgradedIconS;
            skillDefPurge.skillDescriptionToken = "ES_CROCO_SPECIAL_PURGE_UGPRADED_DESCRIPTION";
            skillDefPurge.skillName = ((ScriptableObject)skillDefPurge).name = "ESPurge_UG";
            skillDefPurge.skillNameToken = "ES_CROCO_SPECIAL_PURGE_UGPRADED_NAME";
            defList.Add(skillDefPurge);
            SkillUpgradeContainer container = skillsToHandle["ESPurge"];
            container.upgradedSkillDef = skillDefPurge;
            skillsToHandle["ESPurge"] = container;
            ContentAddition.AddEntityState<AcridPurgeEntityUpgrade>(out _);

            //Upgraded Slashport
            MercSlashportDef skillDefSlashport = ScriptableObject.CreateInstance<MercSlashportDef>();
            skillDefSlashport.activationState = new SerializableEntityStateType(typeof(SlashportEntityUpgrade));
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
            skillDefSlashport.icon = Sprites.slashportUpgradedIconS;
            skillDefSlashport.skillDescriptionToken = "ES_MERC_SPECIAL_SLASHPORT_UPGRADED_DESCRIPTION";
            skillDefSlashport.skillName = ((ScriptableObject)skillDefSlashport).name = "ESSlashport_UG";
            skillDefSlashport.skillNameToken = "ES_MERC_SPECIAL_SLASHPORT_UPGRADED_NAME";
            defList.Add(skillDefSlashport);
            container = skillsToHandle["ESSlashport"];
            container.upgradedSkillDef = skillDefSlashport;
            skillsToHandle["ESSlashport"] = container;
            ContentAddition.AddEntityState<SlashportEntityUpgrade>(out _);

            //Upgraded Directive Root
            GroundedSkillDef skillDefRoot = ScriptableObject.CreateInstance<GroundedSkillDef>();
            skillDefRoot.activationState = new SerializableEntityStateType(typeof(DirectiveRootUpgraded));
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
            skillDefRoot.icon = Sprites.rexrootUpgradedIconS;
            skillDefRoot.skillDescriptionToken = "ES_TREEBOT_SPECIAL_ROOT_UPGRADED_DESCRIPTION";
            skillDefRoot.skillName = ((ScriptableObject)skillDefRoot).name = "ESRoot_UG";
            skillDefRoot.skillNameToken = "ES_TREEBOT_SPECIAL_ROOT_UPGRADED_NAME";
            defList.Add(skillDefRoot);
            container = skillsToHandle["ESRoot"];
            container.upgradedSkillDef = skillDefRoot;
            skillsToHandle["ESRoot"] = container;
            ContentAddition.AddEntityState<DirectiveRootUpgraded>(out _);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static void SetScepterReplacements()
        {
            //Loop through and register all the scepter skills
            foreach(KeyValuePair<string, SkillUpgradeContainer> skillEntry in skillsToHandle)
            {
                SkillUpgradeContainer skillUpgrade = skillEntry.Value;
                ThinkInvisible.ClassicItems.Scepter.instance.RegisterScepterSkill(skillUpgrade.upgradedSkillDef, skillUpgrade.body + "Body", skillUpgrade.slot, skillUpgrade.normalSkillDef);
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static void SetStandaloneScepterReplacements()
        {
            //Same thing above, just using the standalone methods
            foreach (KeyValuePair<string, SkillUpgradeContainer> skillEntry in skillsToHandle)
            {
                SkillUpgradeContainer skillUpgrade = skillEntry.Value;
                AncientScepter.AncientScepterItem.instance.RegisterScepterSkill(skillUpgrade.upgradedSkillDef, skillUpgrade.body + "Body", skillUpgrade.slot, skillUpgrade.index);
            }
        }
    }
}