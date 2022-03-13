using UnityEngine;
using EggsSkills.EntityStates.TeslaMine.MineStates.MainStates;
using EggsSkills.EntityStates.TeslaMine.MineStates.ArmingStates;
using R2API;
using static R2API.ContentAddition;
using EggsUtils.Buffs;
using System.Linq;
using System.Collections.Generic;
using EggsSkills.Config;
using RoR2;
using RoR2.Projectile;
using EntityStates;

namespace EggsSkills.Resources
{
    class Projectiles
    {
        //List for holding all of the projectiles we add
        internal static List<GameObject> projList = new List<GameObject>();


        //Prefab for huntress cluster-bomb arrow bomblets
        internal static GameObject bombletPrefab;
        //Prefab for captain tracking grenade
        internal static GameObject debuffGrenadePrefab;
        //Prefab for MUL-T nanobeacon
        internal static GameObject nanoBeaconPrefab;
        //Prefab for engi tesla mine
        internal static GameObject teslaMinePrefab;

        //Main method for adding projectiles to the game
        internal static void RegisterProjectiles()
        {
            //Check if config allows skill so we don't load unused projectiles
            if (Configuration.GetConfigValue<bool>(Configuration.EnableEngiSkills))
            {
                //Register the tesla mine
                RegisterTeslaMine();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableCaptainSkills))
            {
                //Register the tracking grenade
                RegisterDebuffNade();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableHuntressSkills))
            {
                //Register the cluster bomblet
                RegisterArrowBomblet();
            }
            if (Configuration.GetConfigValue<bool>(Configuration.EnableToolbotSkills))
            {
                //Register the nanomachines son
                RegisterNanoBeacon();
            }
            //If any projectile are queued for loading
            if (projList.Count > 0)
            {
                //Take each proj in the list
                foreach (GameObject proj in projList)
                {
                    //And as long as it actually exists
                    if (proj != null)
                    {
                        //Register it to network via prefabapi
                        PrefabAPI.RegisterNetworkPrefab(proj);
                        //Add it to the game via projectileapi
                        AddProjectile(proj);
                        //Tell the console (for debugging) that we registered the projectile
                        EggsUtils.EggsUtils.LogToConsole("Projectile: " + proj.name + " Registered");
                    }
                }
            }
            //If all projectile skills are disabled also inform console of that
            else EggsUtils.EggsUtils.LogToConsole("No projectiles needed to be registered");
        }
        //Adds tesla mine to projectile register list
        internal static void RegisterTeslaMine()
        {
            //Start with the base engi mine
            teslaMinePrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/projectiles/engimine").InstantiateClone("TeslaMine", true);
            //If it exists (AKA if shit no break)
            if (teslaMinePrefab)
            {
                //Change the forward speed on it a bit, not sure why but it feels nice so keeping it
                teslaMinePrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed = 40f;

                //Grab the arming state machine (Handler for the arming states, I think)
                EntityStateMachine teslaArmingStateMachine = teslaMinePrefab.GetComponentsInChildren<EntityStateMachine>().First(machine => machine.customName == "Arming");
                //Replace the initial and main states of the arming state to our own, cooler states
                teslaArmingStateMachine.initialStateType = new SerializableEntityStateType(typeof(TeslaArmingUnarmedState));
                teslaArmingStateMachine.mainStateType = new SerializableEntityStateType(typeof(TeslaArmingUnarmedState));

                //Grab the main state machine (Handler for the mine after it has landed and is ready to splode)
                EntityStateMachine teslaMainStateMachine = teslaMinePrefab.GetComponentsInChildren<EntityStateMachine>().First(machine => machine.customName == "Main");
                //Replace initial and main with our own: Initial is the waiting for target, main state is predet state
                //Note that the main state is also called when a mine is about to be unloaded (For ex too many mines)
                teslaMainStateMachine.initialStateType = new SerializableEntityStateType(typeof(TeslaWaitForStick));
                teslaMainStateMachine.mainStateType = new SerializableEntityStateType(typeof(TeslaPreDetState));

                //Add it to the projectile queue after modifying it
                projList.Add(teslaMinePrefab);
            }
        }
        //Adds tracking grenade to projectile register list
        internal static void RegisterDebuffNade()
        {
            //Start with base engi grenade cause it has a nice grenade shape
            debuffGrenadePrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/projectiles/engigrenadeprojectile").InstantiateClone("DebuffGrenade", true);
            //As long as it actually exists we can do stuff with it
            if (debuffGrenadePrefab)
            {
                //We want ours to go a bit faster than the shitty nade that engi uses
                debuffGrenadePrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed *= 1.5f;

                //We gotta take hold of the impact explosion component
                ProjectileImpactExplosion debuffGrenadeExplosion = debuffGrenadePrefab.GetComponent<ProjectileImpactExplosion>();
                //We want ours to explode on impact instead of bouncing around
                debuffGrenadeExplosion.destroyOnWorld = true;
                //We want hella more radius
                debuffGrenadeExplosion.blastRadius = Configuration.GetConfigValue<float>(Configuration.CaptainDebuffnadeRadius);
                //Linear falloff, idk what it normally is but this way we know for sure its linear
                debuffGrenadeExplosion.falloffModel = BlastAttack.FalloffModel.Linear;
                //Implement our own proc coefficient-to-damage-type stuff
                debuffGrenadeExplosion.blastProcCoefficient = BuffsLoading.ProcToDamageTypeEncoder(BuffsLoading.trackingOnHit.procIndex, 1f);
                //Change the explosion fx to something cooler as well
                debuffGrenadeExplosion.impactEffect = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXScavCannonImpactExplosion");

                //Nab the damage component of it
                ProjectileDamage debuffGrenadeDamage = debuffGrenadePrefab.GetComponent<ProjectileDamage>();
                //This lets us use our own special damage type, it converts back to generic when being read off
                debuffGrenadeDamage.damageType = DamageType.NonLethal;

                //Add the projectile to the list
                projList.Add(debuffGrenadePrefab);
            }
        }
        //Adds nano beacon to the projectile register list
        internal static void RegisterNanoBeacon()
        {
            //Nab the projectile that's usually used for MUL-T scrap launcher
            nanoBeaconPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/projectiles/toolbotgrenadelauncherprojectile").InstantiateClone("NanoBeacon", true);
            //Ofc check if it actually exists
            if (nanoBeaconPrefab)
            {
                //Make it move at our own speed of choice
                nanoBeaconPrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed = 140f;

                //Make it so it won't kersplode itself on impact
                ProjectileSingleTargetImpact nanoBeaconSingle = nanoBeaconPrefab.AddComponent<ProjectileSingleTargetImpact>();
                nanoBeaconSingle.destroyWhenNotAlive = false;
                nanoBeaconSingle.destroyOnWorld = false;

                //This timed buff only applies on direct hit with nano beacon
                ProjectileInflictTimedBuff nanoDebuff = nanoBeaconPrefab.AddComponent<ProjectileInflictTimedBuff>();
                nanoDebuff.buffDef = BuffsLoading.buffDefTracking;
                //Tracking lasts for 5 seconds from this
                nanoDebuff.duration = 5f;

                //We do in fact want it to stick on impact
                ProjectileStickOnImpact nanoBeaconStick = nanoBeaconPrefab.AddComponent<ProjectileStickOnImpact>();
                //This makes it so it doesn't pass through anything
                nanoBeaconStick.ignoreCharacters = false;
                nanoBeaconStick.ignoreWorld = false;
                //If we enable this the beacon sticks at gross angles, it feels more natural this way
                nanoBeaconStick.alignNormals = false;

                //This handles the 'explosion' of the beacon, we are changing it a lot
                ProjectileImpactExplosion nanoBeaconExplosion = nanoBeaconPrefab.GetComponent<ProjectileImpactExplosion>();
                //No more self-destruction
                nanoBeaconExplosion.destroyOnEnemy = false;
                nanoBeaconExplosion.destroyOnWorld = false;
                //No more blast radius
                nanoBeaconExplosion.blastRadius = 0f;
                //Live 'forever' or until our code says we want it to vanish
                nanoBeaconExplosion.lifetime = 100f;
                //Which is here, it should die 3.5 seconds after impact
                nanoBeaconExplosion.lifetimeAfterImpact = 3.5f;
                //No damage for the blast
                nanoBeaconExplosion.blastDamageCoefficient = 0f;
                //No blast force
                nanoBeaconExplosion.bonusBlastForce = Vector3.zero;
                //Basically no blast, so no falloffmodel either
                nanoBeaconExplosion.falloffModel = BlastAttack.FalloffModel.None;

                //This is our component that 'talks' to the MUL-T player's swarm component
                nanoBeaconPrefab.AddComponent<OnStuckCaller>();

                //Add it to the list
                projList.Add(nanoBeaconPrefab);
            }
        }
        //Adds cluster bomblets to projectile register list
        internal static void RegisterArrowBomblet()
        {
            //As usual grab the prefab we want to base it off of
            bombletPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/projectiles/engigrenadeprojectile").InstantiateClone("ClusterBomblets", true);
            //If it exists
            if (bombletPrefab)
            {
                //We want ours to actually be a quarter the speed of the actual engi bomblets
                bombletPrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed /= 4;
                //This helps them not collide with things except the world
                bombletPrefab.gameObject.layer = 13;

                //Grab the explodey component
                ProjectileImpactExplosion bombletExplosion = bombletPrefab.GetComponent<ProjectileImpactExplosion>();
                //Higher blast radius
                bombletExplosion.blastRadius = 8;
                //Falloffmodel makes these feel bad so it's same damage regardless of the (small) distance
                bombletExplosion.falloffModel = BlastAttack.FalloffModel.None;
                //Reduce the proc coeff a bit for the sake of not making it busted
                bombletExplosion.blastProcCoefficient = 0.6f;
                //Make it a cooler kersplodey fx
                bombletExplosion.impactEffect = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXCommandoGrenade");
                //We want it to blow up on world only, if it's on enemy it just instantly all explodes which is no fun
                bombletExplosion.destroyOnWorld = true;
                bombletExplosion.destroyOnEnemy = false;
                //The projectiles 'live' a long time after impact, but not actually
                bombletExplosion.lifetimeAfterImpact = 100f;
                //Explode themselves after 0.8 seconds
                bombletExplosion.lifetime = 0.8f;
                //Give or take one second
                bombletExplosion.lifetimeRandomOffset = 1f;

                //Add the projectile to the list
                projList.Add(bombletPrefab);
            }
        }
    }
}
