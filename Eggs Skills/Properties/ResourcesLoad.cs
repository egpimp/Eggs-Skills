using System;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EnigmaticThunder.Modules;
using EggsSkills.EntityStates.TeslaMine.MineStates.ArmingStates;
using EggsSkills.EntityStates.TeslaMine.MineStates.MainStates;
using EggsBuffs;
using EntityStates;
using System.Linq;

namespace EggsSkills.Properties
{
    public static class Assets
    {
        public static Texture2D shotgunIcon = LoadTexture2D(Resources.CommandoShotgun);
        public static Sprite shotgunIconS = TexToSprite(shotgunIcon);

        public static Texture2D zapportIcon = LoadTexture2D(Resources.SurgeTeleport);
        public static Sprite zapportIconS = TexToSprite(zapportIcon);

        public static Texture2D slashportIcon = LoadTexture2D(Resources.FatalTeleport);
        public static Sprite slashportIconS = TexToSprite(slashportIcon);

        public static Texture2D rexrootIcon = LoadTexture2D(Resources.RexRoot);
        public static Sprite rexrootIconS = TexToSprite(rexrootIcon);

        public static Texture2D acridpurgeIcon = LoadTexture2D(Resources.Expunge);
        public static Sprite acridpurgeIconS = TexToSprite(acridpurgeIcon);

        public static Texture2D shieldsplosionIcon = LoadTexture2D(Resources.Shieldsplosion);
        public static Sprite shieldsplosionIconS = TexToSprite(shieldsplosionIcon);

        public static Texture2D teslaMineIcon = LoadTexture2D(Resources.TeslaMine);
        public static Sprite teslaMineIconS = TexToSprite(teslaMineIcon);

        public static Texture2D debuffNadeIcon = LoadTexture2D(Resources.DebuffNade);
        public static Sprite debuffNadeIconS = TexToSprite(debuffNadeIcon);

        public static GameObject teslaMinePrefab;
        public static GameObject debuffGrenadePrefab;
        public static Texture2D LoadTexture2D(Byte[] resourceBytes)
        {
            if (resourceBytes == null) throw new ArgumentNullException(nameof(resourceBytes));

            var tempTex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
            tempTex.LoadImage(resourceBytes, false);
            return tempTex;
        }
        public static Sprite TexToSprite(Texture2D tex)
        {
            return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
        public static void ProjectileLoader()
        {
            //Tesla Mine
            teslaMinePrefab = UnityEngine.Resources.Load<GameObject>("prefabs/projectiles/engimine").InstantiateClone("TeslaMine");
            teslaMinePrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed = 40f;

            var teslaArmingStateMachine = teslaMinePrefab.GetComponentsInChildren<EntityStateMachine>().First(machine => machine.customName == "Arming");
            teslaArmingStateMachine.initialStateType = new SerializableEntityStateType(typeof(TeslaArmingUnarmedState));
            teslaArmingStateMachine.mainStateType = new SerializableEntityStateType(typeof(TeslaArmingUnarmedState));

            var teslaMainStateMachine = teslaMinePrefab.GetComponentsInChildren<EntityStateMachine>().First(machine => machine.customName == "Main");
            teslaMainStateMachine.initialStateType = new SerializableEntityStateType(typeof(TeslaWaitForStick));
            teslaMainStateMachine.mainStateType = new SerializableEntityStateType(typeof(TeslaPreDetState));

            //Tracking Grenade
            debuffGrenadePrefab = UnityEngine.Resources.Load<GameObject>("prefabs/projectiles/engigrenadeprojectile").InstantiateClone("DebuffGrenade");
            debuffGrenadePrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed *= 1.5f;

            var debuffGrenadeExplosion = debuffGrenadePrefab.GetComponent<ProjectileImpactExplosion>();
            debuffGrenadeExplosion.destroyOnWorld = true;
            debuffGrenadeExplosion.blastRadius = 20;
            debuffGrenadeExplosion.falloffModel = BlastAttack.FalloffModel.Linear;
            debuffGrenadeExplosion.blastProcCoefficient = BuffsLoading.ProcToDamageTypeEncoder(BuffsLoading.trackingOnHitIndex, 1f);
            debuffGrenadeExplosion.impactEffect = UnityEngine.Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXScavCannonImpactExplosion");

            var debuffGrenadeDamage = debuffGrenadePrefab.GetComponent<ProjectileDamage>();
            debuffGrenadeDamage.damageType = DamageType.NonLethal;

            Projectiles.RegisterProjectile(teslaMinePrefab);
            Projectiles.RegisterProjectile(debuffGrenadePrefab);
        }
        public static void RegisterLanguageTokens()
        {
            //Artificer
            Languages.Add("ARTIFICER_UTILITY_ZAPPORT_NAME", "Quantum Transposition");
            Languages.Add("ARTIFICER_UTILITY_ZAPPORT_DESC", "<style=cIsDamage>Stunning.</style> Charge to <style=cIsUtility>Teleport</style> forward, dealing <style=cIsDamage>250%-1000% damage</style> to enemies near target location.");
            //Merc
            Languages.Add("MERCENARY_UTILITY_SLASHPORT_NAME", "Fatal Assault");
            Languages.Add("MERCENARY_UTILITY_SLASHPORT_DESC", "<style=cIsDamage>Stunning.</style> Target an enemy to <style=cIsUtility>Expose</style> and <style=cIsUtility>Teleport</style> to and strike them for <style=cIsDamage>600% damage, plus 20% of their missing health</style>.");
            //Commando
            Languages.Add("COMMANDO_PRIMARY_COMBATSHOTGUN_NAME", "Flechette Rounds");
            Languages.Add("COMMANDO_PRIMARY_COMBATSHOTGUN_DESC", "Fire flechette rounds, dealing <style=cIsDamage>6x60% damage</style> in a wider but shorter range.  <style=cIsUtility>Spread</style> decreases on <style=cIsDamage>Critical Hits</style>.");
            //Engi
            Languages.Add("ENGI_SECONDARY_TESLAMINE_NAME", "T-3514 Shock Mines");
            Languages.Add("ENGI_SECONDARY_TESLAMINE_DESC", "<style=cIsDamage>Stunning.</style> Place a shock mine, that upon detonation deals <style=cIsDamage>200% damage</style> and leaves a lingering zone for <style=cIsDamage>4</style> seconds that deals <style=cIsDamage>200% damage each second</style>.  Can place up to 4.");
            //Rex
            Languages.Add("REX_SPECIAL_ROOT_NAME", "DIRECTIVE: Respire");
            Languages.Add("REX_SPECIAL_ROOT_DESC", "<style=cIsDamage>Stunning.</style> <style=cIsUtility>Slow</style> yourself, but gain <style=cIsUtility>Armor</style> for up to 8 seconds.  While active, deal <style=cIsDamage>250% damage</style> per second to nearby enemies, gaining <style=cIsDamage>Barrier</style> per enemy hit and <style=cIsDamage>Pulling</style> them towards you.");
            //Loader
            Languages.Add("LOADER_SPECIAL_SHIELDSPLOSION_NAME", "Barrier Buster");
            Languages.Add("LOADER_SPECIAL_SHIELDSPLOSION_DESC", "Consume all of your current <style=cIsHealing>Barrier (Minimum 10%)</style> to gain a burst of <style=cIsUtility>Movement Speed</style> and deal <style=cIsDamage>600-6000% damage</style> around you based on <style=cIsHealing>Barrier</style> consumed.");
            //ACRID
            Languages.Add("ACRID_SPECIAL_PURGE_NAME", "Expunge");
            Languages.Add("ACRID_SPECIAL_PURGE_DESC", "Inflict damage on all <style=cIsHealing>Poisoned</style> or <style=cIsDamage>Blighted</style> enemies in range, dealing either <style=cIsHealing>200% + 10% of their max health</style> or <style=cIsDamage>300% per stack</style> in an area around them, depending on passive selection.");
            //Captain
            Languages.Add("CAPTAIN_SECONDARY_DEBUFFNADE_NAME", "MK-4 Tracking Grenade");
            Languages.Add("CAPTAIN_SECONDARY_DEBUFFNADE_DESC", "Fire a grenade that deals <style=cIsDamage>250% Damage</style> and inflicts <style=cArtifact>Tracking</style> on enemies in a wide area for 5 seconds");
        }
    }
}
