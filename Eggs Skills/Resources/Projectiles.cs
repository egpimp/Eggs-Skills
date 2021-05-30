using UnityEngine;
using RoR2;
using EggsSkills.EntityStates.TeslaMine.MineStates.MainStates;
using EntityStates;
using RoR2.Projectile;
using EggsSkills.EntityStates.TeslaMine.MineStates.ArmingStates;
using R2API;
using EggsBuffs;
using System.Linq;
using System.Collections.Generic;
using EggsSkills.Config;

namespace EggsSkills.Resources
{
    class Projectiles
    {
        internal static GameObject teslaMinePrefab;
        internal static GameObject debuffGrenadePrefab;
        internal static GameObject bombletPrefab;
        internal static GameObject nanoBeaconPrefab;
        internal static List<GameObject> projList = new List<GameObject>();

        internal static void RegisterProjectiles()
        {
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableEngiSkills.Value : true)
            {
                RegisterTeslaMine();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableCaptainSkills.Value : true)
            {
                RegisterDebuffNade();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableHuntressSkills.Value : true)
            {
                RegisterArrowBomblet();
            }
            if (Configuration.ConfigEditingAgreement.Value ? Configuration.EnableToolbotSkills.Value : true)
            {
                RegisterNanoBeacon();
            }
            if (projList.Count > 0)
            {
                foreach (GameObject proj in projList)
                {
                    if (proj != null)
                    {
                        ProjectileAPI.Add(proj);
                        PrefabAPI.RegisterNetworkPrefab(proj);
                    }
                }
            }
            else
            {
                Debug.Log("No projectiles needed to be registered");
            }
        }
        internal static void RegisterTeslaMine()
        {
            teslaMinePrefab = UnityEngine.Resources.Load<GameObject>("prefabs/projectiles/engimine").InstantiateClone("TeslaMine");
            if (teslaMinePrefab)
            {
                teslaMinePrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed = 40f;

                EntityStateMachine teslaArmingStateMachine = teslaMinePrefab.GetComponentsInChildren<EntityStateMachine>().First(machine => machine.customName == "Arming");
                teslaArmingStateMachine.initialStateType = new SerializableEntityStateType(typeof(TeslaArmingUnarmedState));
                teslaArmingStateMachine.mainStateType = new SerializableEntityStateType(typeof(TeslaArmingUnarmedState));

                EntityStateMachine teslaMainStateMachine = teslaMinePrefab.GetComponentsInChildren<EntityStateMachine>().First(machine => machine.customName == "Main");
                teslaMainStateMachine.initialStateType = new SerializableEntityStateType(typeof(TeslaWaitForStick));
                teslaMainStateMachine.mainStateType = new SerializableEntityStateType(typeof(TeslaPreDetState));

                projList.Add(teslaMinePrefab);
            }
        }
        internal static void RegisterDebuffNade()
        {
            debuffGrenadePrefab = UnityEngine.Resources.Load<GameObject>("prefabs/projectiles/engigrenadeprojectile").InstantiateClone("DebuffGrenade");
            if (debuffGrenadePrefab)
            {
                debuffGrenadePrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed *= 1.5f;

                ProjectileImpactExplosion debuffGrenadeExplosion = debuffGrenadePrefab.GetComponent<ProjectileImpactExplosion>();
                debuffGrenadeExplosion.destroyOnWorld = true;
                debuffGrenadeExplosion.blastRadius = 20;
                debuffGrenadeExplosion.falloffModel = BlastAttack.FalloffModel.Linear;
                debuffGrenadeExplosion.blastProcCoefficient = BuffsLoading.ProcToDamageTypeEncoder(BuffsLoading.trackingOnHitIndex, 1f);
                debuffGrenadeExplosion.impactEffect = UnityEngine.Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXScavCannonImpactExplosion");

                ProjectileDamage debuffGrenadeDamage = debuffGrenadePrefab.GetComponent<ProjectileDamage>();
                debuffGrenadeDamage.damageType = DamageType.NonLethal;

                projList.Add(debuffGrenadePrefab);
            }
        }
        internal static void RegisterNanoBeacon()
        {
            nanoBeaconPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/projectiles/toolbotgrenadelauncherprojectile").InstantiateClone("NanoBeacon");
            if (nanoBeaconPrefab)
            {
                nanoBeaconPrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed = 140f;

                ProjectileSingleTargetImpact nanoBeaconSingle = nanoBeaconPrefab.AddComponent<ProjectileSingleTargetImpact>();
                nanoBeaconSingle.destroyWhenNotAlive = false;
                nanoBeaconSingle.destroyOnWorld = false;

                ProjectileInflictTimedBuff nanoDebuff = nanoBeaconPrefab.AddComponent<ProjectileInflictTimedBuff>();
                nanoDebuff.buffDef = BuffsLoading.buffDefTracking;
                nanoDebuff.duration = 5f;

                ProjectileStickOnImpact nanoBeaconStick = nanoBeaconPrefab.AddComponent<ProjectileStickOnImpact>();
                nanoBeaconStick.ignoreCharacters = false;
                nanoBeaconStick.ignoreWorld = false;
                nanoBeaconStick.alignNormals = false;

                ProjectileImpactExplosion nanoBeaconExplosion = nanoBeaconPrefab.GetComponent<ProjectileImpactExplosion>();
                nanoBeaconExplosion.destroyOnEnemy = false;
                nanoBeaconExplosion.destroyOnWorld = false;
                nanoBeaconExplosion.blastRadius = 0f;
                nanoBeaconExplosion.lifetime = 100f;
                nanoBeaconExplosion.lifetimeAfterImpact = 3f;
                nanoBeaconExplosion.blastDamageCoefficient = 0f;
                nanoBeaconExplosion.bonusBlastForce = Vector3.zero;
                nanoBeaconExplosion.falloffModel = BlastAttack.FalloffModel.None;

                OnStuckCaller onStuckCaller = nanoBeaconPrefab.AddComponent<OnStuckCaller>();

                projList.Add(nanoBeaconPrefab);
            }
        }
        internal static void RegisterArrowBomblet()
        {
            bombletPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/projectiles/engigrenadeprojectile").InstantiateClone("ArrowBomblets");
            if (bombletPrefab)
            {
                bombletPrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed /= 4;
                bombletPrefab.gameObject.layer = 13;

                ProjectileImpactExplosion bombletExplosion = bombletPrefab.GetComponent<ProjectileImpactExplosion>();
                bombletExplosion.blastRadius = 8;
                bombletExplosion.falloffModel = BlastAttack.FalloffModel.None;
                bombletExplosion.blastProcCoefficient = 0.6f;
                bombletExplosion.impactEffect = UnityEngine.Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXCommandoGrenade");
                bombletExplosion.destroyOnWorld = true;
                bombletExplosion.destroyOnEnemy = false;
                bombletExplosion.lifetimeAfterImpact = 100f;
                bombletExplosion.lifetime = 0.8f;
                bombletExplosion.lifetimeRandomOffset = 1f;

                projList.Add(bombletPrefab);
            }
        }
    }
}
