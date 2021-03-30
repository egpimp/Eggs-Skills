using System;
using EntityStates;
using RoR2;
using UnityEngine;
using BepInEx;
using RoR2.Skills;
using EggsSkills.Properties;
using EggsSkills.SkillDefs;
using RoR2.Projectile;
using System.Linq;
using EggsSkills.EntityStates.MineStates.MainStates;
using EggsSkills.EntityStates.MineStates.ArmingStates;
using EnigmaticThunder.Modules;
using System.Collections.Generic;
using EggsSkills.Skills.TeslaMine.MineStates.ArmingStates;
using EggsSkills.EntityStates.MineStates;
using EggsSkills.EntityStates.TeslaMine.MineStates.MainStates;

namespace EggsSkills
{
    [BepInDependency("com.EnigmaDev.EnigmaticThunder",BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin("com.Egg.EggsSkills", "Eggs Skills", "1.0.3")]
    
    public class ModBase : BaseUnityPlugin
    {
        public static Texture2D shotgunIcon = Assets.LoadTexture2D(EggsSkills.Properties.Resources.CommandoShotgun);
        public static Sprite shotgunIconS = Assets.TexToSprite(shotgunIcon);

        public static Texture2D zapportIcon = Assets.LoadTexture2D(EggsSkills.Properties.Resources.SurgeTeleport);
        public static Sprite zapportIconS = Assets.TexToSprite(zapportIcon);

        public static Texture2D slashportIcon = Assets.LoadTexture2D(EggsSkills.Properties.Resources.FatalTeleport);
        public static Sprite slashportIconS = Assets.TexToSprite(slashportIcon);

        public static Texture2D rexrootIcon = Assets.LoadTexture2D(EggsSkills.Properties.Resources.RexRoot);
        public static Sprite rexrootIconS = Assets.TexToSprite(rexrootIcon);

        public static Texture2D acridpurgeIcon = Assets.LoadTexture2D(EggsSkills.Properties.Resources.Expunge);
        public static Sprite acridpurgeIconS = Assets.TexToSprite(acridpurgeIcon);

        public static Texture2D shieldsplosionIcon = Assets.LoadTexture2D(EggsSkills.Properties.Resources.Shieldsplosion);
        public static Sprite shieldsplosionIconS = Assets.TexToSprite(shieldsplosionIcon);

        public static Texture2D speedIcon = UnityEngine.Resources.Load<Texture2D>("textures/bufficons/texMovespeedBuffIcon");
        public static Sprite speedIconS = EggsSkills.Properties.Assets.TexToSprite(speedIcon);

        public static GameObject teslaMinePrefab;

        public static BuffDef buffDefSpeed;

        public void Awake()
        {
            HandleProjectileShit();
            //CharacterbodyRefs
            var artificerRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/MageBody");
            var mercenaryRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/MercBody");
            var commandoRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/CommandoBody");
            var engineerRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/EngiBody");
            var rexRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/TreebotBody");
            var loaderRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/LoaderBody");
            var acridRef = UnityEngine.Resources.Load<GameObject>("prefabs/characterbodies/CrocoBody");
            //Artificer skill locators
            var artificerSkillLocator = artificerRef.GetComponent<SkillLocator>();
            var artificerSkillFamilyUtility = artificerSkillLocator.utility.skillFamily;
            //Merc skill locators
            var mercSkillLocator = mercenaryRef.GetComponent<SkillLocator>();
            var mercSkillFamilyUtility = mercSkillLocator.utility.skillFamily;
            //Commando skill locators
            var commandoSkillLocator = commandoRef.GetComponent<SkillLocator>();
            var commandoSkillFamilyPrimary = commandoSkillLocator.primary.skillFamily;
            //Acrid skill locators
            var acridSkillLocator = acridRef.GetComponent<SkillLocator>();
            var acridSkillFamilySpecial = acridSkillLocator.special.skillFamily;
            //Rex skill locators
            var rexSkillLocator = rexRef.GetComponent<SkillLocator>();
            var rexSkillFamilySpecial = rexSkillLocator.special.skillFamily;
            //Loader skill locators 
            var loaderSkillLocator = loaderRef.GetComponent<SkillLocator>();
            var loaderSkillFamilySpecial = loaderSkillLocator.special.skillFamily;
            //Engi skill locators
            var engiSkillLocator = engineerRef.GetComponent<SkillLocator>();
            var engiSkillFamilySecondary = engiSkillLocator.secondary.skillFamily;

            //Zapport
            var skillDefZapport = ScriptableObject.CreateInstance<SkillDef>();
            skillDefZapport.activationState = new SerializableEntityStateType(typeof(EntityStates.ZapportEntity));
            skillDefZapport.activationStateMachineName = "Weapon";
            skillDefZapport.baseMaxStock = 1;
            skillDefZapport.baseRechargeInterval = 12f;
            skillDefZapport.beginSkillCooldownOnSkillEnd = true;
            skillDefZapport.canceledFromSprinting = false;
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
            skillDefZapport.icon = zapportIconS;
            skillDefZapport.skillDescriptionToken = "<style=cIsDamage>Stunning.</style> Charge to <style=cIsUtility>Teleport</style> forward, dealing <style=cIsDamage>250%-1000% damage</style> to enemies near current and target location.";
            skillDefZapport.skillName = "Zapport";
            skillDefZapport.skillNameToken = "Surge Teleport";
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
            //Slashport
            var skillDefSlashport = ScriptableObject.CreateInstance<MercSlashportDef>();
            skillDefSlashport.activationState = new SerializableEntityStateType(typeof(EntityStates.SlashportEntity));
            skillDefSlashport.activationStateMachineName = "Weapon";
            skillDefSlashport.baseMaxStock = 1;
            skillDefSlashport.baseRechargeInterval = 5f;
            skillDefSlashport.beginSkillCooldownOnSkillEnd = true;
            skillDefSlashport.canceledFromSprinting = false;
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
            skillDefSlashport.icon = slashportIconS;
            skillDefSlashport.skillDescriptionToken = "<style=cIsDamage>Stunning.</style> Target an enemy to <style=cIsUtility>Expose</style> and <style=cIsUtility>Teleport</style> to and strike them for <style=cIsDamage>700% damage, plus 20% of their missing health</style>.";
            skillDefSlashport.skillName = "Slashport";
            skillDefSlashport.skillNameToken = "Fatal Teleport";
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
            //Combat Shotgun
            var skillDefCombatshotgun = ScriptableObject.CreateInstance<SkillDef>();
            skillDefCombatshotgun.activationState = new SerializableEntityStateType(typeof(EggsSkills.EntityStates.CombatShotgunEntity));
            skillDefCombatshotgun.activationStateMachineName = "Weapon";
            skillDefCombatshotgun.baseMaxStock = 8;
            skillDefCombatshotgun.baseRechargeInterval = 0.8f;
            skillDefCombatshotgun.beginSkillCooldownOnSkillEnd = true;
            skillDefCombatshotgun.canceledFromSprinting = false;
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
            skillDefCombatshotgun.icon = shotgunIconS;
            skillDefCombatshotgun.skillDescriptionToken = "Fire a fully automatic combat shotgun, dealing <style=cIsDamage>6x60% damage</style> total.  Spread decreases on crit.";
            skillDefCombatshotgun.skillName = "CombatShotgun";
            skillDefCombatshotgun.skillNameToken = "Combat Shotgun";

            Loadouts.RegisterSkillDef(skillDefCombatshotgun);
            Array.Resize(ref commandoSkillFamilyPrimary.variants, commandoSkillFamilyPrimary.variants.Length + 1);
            commandoSkillFamilyPrimary.variants[commandoSkillFamilyPrimary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefCombatshotgun,
                unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>(),
                viewableNode = new ViewablesCatalog.Node(skillDefCombatshotgun.skillNameToken, false, null)
            };
            //Tesla mines
            var skillDefTeslamine = ScriptableObject.CreateInstance<SkillDef>();
            skillDefTeslamine.activationState = new SerializableEntityStateType(typeof(EggsSkills.EntityStates.TeslaMineFireState));
            skillDefTeslamine.activationStateMachineName = "Weapon";
            skillDefTeslamine.baseMaxStock = 4;
            skillDefTeslamine.baseRechargeInterval = 10f;
            skillDefTeslamine.beginSkillCooldownOnSkillEnd = false;
            skillDefTeslamine.canceledFromSprinting = false;
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
            skillDefTeslamine.icon = zapportIconS;
            skillDefTeslamine.skillDescriptionToken = "<style=cIsDamage>Stunning.</style> Place a tesla mine, that upon detonation deals <style=cIsDamage>100% damage</style> and leaves a lingering zone for 5 seconds that deals <style=cIsDamage>100% damage</style> each second.  Can place up to 4.";
            skillDefTeslamine.skillName = "TeslaMine";
            skillDefTeslamine.skillNameToken = "Tesla Mines";
            skillDefTeslamine.keywordTokens = new string[]
            {
                "KEYWORD_STUNNING"
            };

            Loadouts.RegisterSkillDef(skillDefTeslamine);
            RegisterTeslaMineStates();
            Array.Resize(ref engiSkillFamilySecondary.variants, engiSkillFamilySecondary.variants.Length + 1);
            engiSkillFamilySecondary.variants[engiSkillFamilySecondary.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefTeslamine,
                unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>(),
                viewableNode = new ViewablesCatalog.Node(skillDefTeslamine.skillNameToken, false, null) 
            };
            //Directive Root
            var skillDefRoot = ScriptableObject.CreateInstance<GroundedSkillDef>();
            skillDefRoot.activationState = new SerializableEntityStateType(typeof(EggsSkills.EntityStates.DirectiveRoot));
            skillDefRoot.activationStateMachineName = "Weapon";
            skillDefRoot.baseMaxStock = 1;
            skillDefRoot.baseRechargeInterval = 12f;
            skillDefRoot.beginSkillCooldownOnSkillEnd = true;
            skillDefRoot.canceledFromSprinting = true;
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
            skillDefRoot.icon = rexrootIconS;
            skillDefRoot.skillDescriptionToken = "<style=cIsDamage>Stunning.</style> <style=cIsUtility>Slow</style> yourself, but gain <style=cIsUtility>Armor</style> for up to 8 seconds.  While active, deal <style=cIsDamage>250% damage</style> per second to nearby enemies, gaining <style=cIsDamage>Barrier</style> per enemy hit and <style=cIsDamage>Pulling</style> them towards you.";
            skillDefRoot.skillName = "Root";
            skillDefRoot.skillNameToken = "DIRECTIVE: Pull";
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
            //Shieldsplosion
            var skillDefShieldsplode = ScriptableObject.CreateInstance<ShieldsplosionDef>();
            skillDefShieldsplode.activationState = new SerializableEntityStateType(typeof(EggsSkills.EntityStates.ShieldSplosionEntity));
            skillDefShieldsplode.activationStateMachineName = "Weapon";
            skillDefShieldsplode.baseMaxStock = 1;
            skillDefShieldsplode.baseRechargeInterval = 8f;
            skillDefShieldsplode.beginSkillCooldownOnSkillEnd = false;
            skillDefShieldsplode.canceledFromSprinting = false;
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
            skillDefShieldsplode.icon = shieldsplosionIconS;
            skillDefShieldsplode.skillDescriptionToken = "Consume all of your current <style=cIsDamage>Barrier (Minimum 10%)</style> to gain a burst of movement speed and deal <style=cIsDamage>600-6000% damage</style> around you based on <style=cIsDamage>Barrier</style> consumed.";
            skillDefShieldsplode.skillName = "ShieldSplosion";
            skillDefShieldsplode.skillNameToken = "Barrier Buster";

            Loadouts.RegisterSkillDef(skillDefShieldsplode);
            Array.Resize(ref loaderSkillFamilySpecial.variants, loaderSkillFamilySpecial.variants.Length + 1);
            loaderSkillFamilySpecial.variants[loaderSkillFamilySpecial.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefShieldsplode,
                unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>(),
                viewableNode = new ViewablesCatalog.Node(skillDefShieldsplode.skillNameToken, false, null)
            };
            //AcridPurge
            var skillDefExpunge = ScriptableObject.CreateInstance<AcridPurgeDef>();
            skillDefExpunge.activationState = new SerializableEntityStateType(typeof(EggsSkills.EntityStates.AcridPurgeEntity));
            skillDefExpunge.activationStateMachineName = "Weapon";
            skillDefExpunge.baseMaxStock = 1;
            skillDefExpunge.baseRechargeInterval = 12f;
            skillDefExpunge.beginSkillCooldownOnSkillEnd = false;
            skillDefExpunge.canceledFromSprinting = false;
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
            skillDefExpunge.icon = acridpurgeIconS;
            skillDefExpunge.skillDescriptionToken = "Inflict damage on all <style=cIsDamage>Poisoned</style> enemies in range, dealing either <style=cIsDamage>200% + 10% of their max health</style> around them, or <style=cIsDamage>300% per stack</style>, depending on passive <style=cIsDamage>Poison</style>";
            skillDefExpunge.skillName = "Purge";
            skillDefExpunge.skillNameToken = "Expunge";

            Loadouts.RegisterSkillDef(skillDefExpunge);
            Array.Resize(ref acridSkillFamilySpecial.variants, acridSkillFamilySpecial.variants.Length + 1);
            acridSkillFamilySpecial.variants[acridSkillFamilySpecial.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = skillDefExpunge,
                unlockableDef = ScriptableObject.CreateInstance<UnlockableDef>(),
                viewableNode = new ViewablesCatalog.Node(skillDefExpunge.skillNameToken, false, null)
            };
        }
        public void HandleProjectileShit()
        {
            teslaMinePrefab = UnityEngine.Resources.Load<GameObject>("prefabs/projectiles/engimine");
            teslaMinePrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed = 40f;

            var teslaArmingStateMachine = teslaMinePrefab.GetComponentsInChildren<EntityStateMachine>().First(machine => machine.customName == "Arming");
            teslaArmingStateMachine.initialStateType = new SerializableEntityStateType(typeof(TeslaArmingUnarmedState));
            teslaArmingStateMachine.mainStateType = new SerializableEntityStateType(typeof(TeslaArmingUnarmedState));

            var teslaMainStateMachine = teslaMinePrefab.GetComponentsInChildren<EntityStateMachine>().First(machine => machine.customName == "Main");
            teslaMainStateMachine.initialStateType = new SerializableEntityStateType(typeof(TeslaWaitForStick));
            teslaMainStateMachine.mainStateType = new SerializableEntityStateType(typeof(TeslaArmingUnarmedState));

            EnigmaticThunder.Modules.Projectiles.RegisterProjectile(teslaMinePrefab);
        }
        public void RegisterTeslaMineStates()
        {
            Loadouts.RegisterEntityState(typeof(TeslaArmingUnarmedState));
            Loadouts.RegisterEntityState(typeof(TeslaArmingFullState));

            Loadouts.RegisterEntityState(typeof(TeslaArmState));
            Loadouts.RegisterEntityState(typeof(TeslaWaitForStick));
            Loadouts.RegisterEntityState(typeof(TeslaWaitForTargetState));
            Loadouts.RegisterEntityState(typeof(TeslaPreDetState));
            Loadouts.RegisterEntityState(typeof(TeslaDetonateState));
        }
        private void SetupBuffs()
        {
            buffDefSpeed = ScriptableObject.CreateInstance<BuffDef>();
            buffDefSpeed.name = "ShieldBroken";
            buffDefSpeed.buffColor = Color.yellow;
            buffDefSpeed.canStack = false;
            buffDefSpeed.isDebuff = false;
            buffDefSpeed.iconSprite = speedIconS;
            EnigmaticThunder.Modules.Buffs.RegisterBuff(buffDefSpeed);
        }
        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self)
            {
                if (self.HasBuff(buffDefSpeed))
                {
                    self.baseMoveSpeed = self.baseMoveSpeed * 1.2f;
                }
            }
        }
    }
}