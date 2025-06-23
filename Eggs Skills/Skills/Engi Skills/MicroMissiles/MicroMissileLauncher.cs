using EggsSkills.Config;
using EggsSkills.Skills.Engi_Skills.MicroMissiles;
using RoR2;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace EggsSkills
{
    internal class MicroMissileLauncher : MonoBehaviour
    {
        private static int salvoBonusCount = Configuration.GetConfigValue(Configuration.EngiMicromissileSalvocount);

        private GameObject owner;

        private ProjectileDamage damage;
        private ProjectileStickOnImpact stick;

        private float delay = 0.6f;

        private bool fired = false;
        private bool hasStuck = false;

        private void Start()
        {
            stick = GetComponent<ProjectileStickOnImpact>();
        }

        private void FixedUpdate()
        {
            if (delay > 0) delay -= Time.fixedDeltaTime;
            else
            {
                if (!fired)
                {
                    LaunchMissiles();
                    fired = true;
                }
            }
            if (stick.stuck && !hasStuck) hasStuck = true;
            if(!stick.stuck && hasStuck) Destroy(gameObject);
            if(stick.stuckBody && stick.stuckBody.healthComponent && !stick.stuckBody.healthComponent.alive) Destroy(gameObject);
        }

        internal void LaunchMissiles()
        {
            //Get the controller first, to find the owner
            ProjectileController component = base.GetComponent<ProjectileController>();
            //Also grab damage component
            damage = base.GetComponent<ProjectileDamage>();
            //Get the owner
            owner = component.owner;
            //Fire orbs from the owner
            FireEngi(4);
            //Get the turrets of the owner, first with the master
            CharacterMaster master = owner.GetComponent<CharacterBody>().master;
            if (master.deployablesList == null) return;
            foreach(DeployableInfo info in master.deployablesList)
            {
                if (info.slot == DeployableSlot.EngiTurret && Vector3.Distance(info.deployable.GetComponent<CharacterMaster>().GetBody().corePosition, transform.position) <= 60f) FireTurret(info.deployable.gameObject, 2 + salvoBonusCount);
            }
        }

        private void FireEngi(int count)
        {
            string muzzle = "MuzzleLeft";
            for(int i = 1; i <= count; i++)
            {
                Vector3 origin = owner.transform.position;
                ModelLocator locator = owner.GetComponent<ModelLocator>();
                if(locator)
                {
                    Transform modelTransform = locator.modelTransform;
                    if (modelTransform)
                    {
                        ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                        if (component)
                        {
                            Transform transform = component.FindChild(muzzle);
                            if (transform) origin = transform.position;
                        }
                    }
                }
                Fire(origin);
                //Swap muzzle
                muzzle = muzzle == "MuzzleLeft" ? "MuzzleRight" : "MuzzleLeft";
            }
        }

        private void FireTurret(GameObject turretObject, int count)
        {
            CharacterMaster turretMaster = turretObject.GetComponent<CharacterMaster>();
            GameObject turretBodyObject = turretMaster.GetBodyObject();
            ModelLocator locator = turretBodyObject.GetComponent<ModelLocator>();
            Vector3 origin = turretObject.transform.position;
            if (locator)
            {
                Transform modelTransform = locator.modelTransform;
                if (modelTransform)
                {
                    ChildLocator component = modelTransform.GetComponent<ChildLocator>();
                    if (component)
                    {
                        Transform transform = component.FindChild("Muzzle");
                        if (transform) origin = transform.position;
                    }
                }
            }
            for (int i = 1; i <= count; i++) Fire(origin);
        }

        private void Fire(Vector3 origin)
        {
            //Get player rotation stuff
            Vector3 dir = owner.transform.forward;
            InputBankTest test = owner.GetComponent<InputBankTest>();
            if(test)
            {
                dir = test.aimDirection;
            }

            if (NetworkServer.active) ProjectileManager.instance.FireProjectile(Resources.Projectiles.micromissilePrefab, origin, Quaternion.LookRotation(dir), owner, damage.damage / MicroMissileEntity.damageCoef, 0f, damage.crit, target:gameObject, damageType: DamageTypeCombo.GenericPrimary) ;
        }
    }
}
