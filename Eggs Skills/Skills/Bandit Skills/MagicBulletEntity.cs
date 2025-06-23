using EggsSkills.Config;
using EntityStates.Bandit2.Weapon;
using RoR2;
using RoR2.Projectile;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace EggsSkills.EntityStates
{
    class MagicBulletEntity : Bandit2FirePrimaryBase
    {
        //Skills++
        public static int spp_richochetMod = 0;
        public static float spp_bounceMod = 0f;

        //sound string
        private static readonly string soundString = "Play_bandit2_m1_rifle";
        private static readonly string muzzleString = "MuzzleShotgun";

        //Whether or not it crit
        private bool isCrit;

        //Damage coefficient of the skill
        private static readonly float baseDamageCoef = 2f;
        //End damage of the ability
        private float damage;
        //Proc coefficient of the skill
        private static readonly float procCoef = 1f;
        //What is damage multiplied by per richochet
        private static readonly float richochetMod = 0.6f + spp_bounceMod;

        //How many richochets
        private static readonly int maxBounces = Configuration.GetConfigValue(Configuration.BanditMagicbulletRicochets) + spp_richochetMod;

        //Fx
        private GameObject tracerEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/TracerBandit2Rifle.prefab").WaitForCompletion();
        private GameObject hitEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/HitsparkBandit.prefab").WaitForCompletion();
        private GameObject muzzleEffect = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/MuzzleflashBandit2.prefab").WaitForCompletion();

        //Keeps track of enemies we already hit
        private List<HurtBox> hitHurtBoxes = new List<HurtBox>();

        //Called on entry of the skillstate
        public override void OnEnter()
        {
            //Base duration of the skill + animations
            baseDuration = 1.2f;
            //Lowest possible duration of the skill
            minimumBaseDuration = 0.1f;
            //Find out if it is critting
            isCrit = base.RollCrit();
            //Make player face the aimdirection
            base.StartAimMode();
            //Do standard onenter stuff
            base.OnEnter();
        }

        //Called by base onenter, we replace it with our own special bullet
        public override void FireBullet(Ray aimRay)
        {
            //Play the sound
            Util.PlaySound(soundString, gameObject);
            //Network check
            if (base.isAuthority)
            {
                //Set the damage accordingly
                damage = baseDamageCoef * base.characterBody.damage;
                //Create the bulletattack
                BulletAttack attack = new BulletAttack
                {
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    tracerEffectPrefab = tracerEffect,
                    muzzleName = muzzleString,
                    hitEffectPrefab = hitEffect,
                    damage = damage,
                    owner = gameObject,
                    isCrit = isCrit,
                    maxDistance = 1000f,
                    smartCollision = true,
                    procCoefficient = procCoef,
                    falloffModel = BulletAttack.FalloffModel.None,
                    weapon = gameObject,
                    hitCallback = CallBack,
                    radius = 1f,
                    damageType = DamageTypeCombo.GenericPrimary
                };
                //Fire
                attack.Fire();
            }
            //Apply the muzzleflash
            EffectManager.SimpleMuzzleFlash(muzzleEffect, gameObject, muzzleString, false);
        }
        
        //Event called when bullet hits anything
        private bool CallBack(BulletAttack bulletattack, ref BulletAttack.BulletHit hitInfo)
        {
            //Run the hit as normal first
            bool hit = BulletAttack.DefaultHitCallbackImplementation(bulletattack, ref hitInfo);
            //Pos is where the bullet hit
            Vector3 pos = hitInfo.point;
            //If it actually hit something, add the main hurtbox of them to our list
            if (hitInfo.hitHurtBox) hitHurtBoxes.Add(hitInfo.hitHurtBox.hurtBoxGroup.mainHurtBox);
            //Execute our richochet code
            //If it was dynamite, we perform a second ricochet
            if (hitInfo.hitHurtBox && hitInfo.hitHurtBox.transform.parent && hitInfo.hitHurtBox.transform.parent.gameObject.name == "SkillsReturnsDynamiteProjectile(Clone)")
            {
                HandleRichochet(pos, maxBounces, damage);
                HandleRichochet(pos, maxBounces, damage);
            }
            else HandleRichochet(pos, maxBounces, damage, richochetMod);
            //Return the previously found hit / nohit
            return hit;
        }

        //What to do when attempting to richochet the bullet
        private void HandleRichochet(Vector3 pos, int bouncesRemaining, float currentDamage, float multiplier = 1f)
        {
            //Hold variable for whether target is found or not
            HurtBox target = null;

            //Loop via spheresearch
            foreach (HurtBox hurtBox in new SphereSearch
            {
                origin = pos,
                radius = 25f,
                mask = LayerIndex.entityPrecise.mask,
            }.RefreshCandidates().OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
            {
                //Get the mainhurtbox
                HurtBox mainBox = hurtBox.hurtBoxGroup.mainHurtBox;

                //Check if its dynamite
                //SkillsReturnsDynamiteProjectile(Clone) parent

                //If the mainhurtbox is not yet in our list...
                if (!hitHurtBoxes.Contains(mainBox))
                {
                    //Check for dynamite
                    if (hurtBox.transform.parent && hurtBox.transform.parent.gameObject.name == "SkillsReturnsDynamiteProjectile(Clone)")
                    {
                        hitHurtBoxes.Add(mainBox);
                        SimulateBullet(pos, mainBox, bouncesRemaining, true, currentDamage * multiplier);
                        target = null;
                        break;
                    }

                    //If not enemy, skip
                    if (!TeamMask.GetEnemyTeams(teamComponent.teamIndex).HasTeam(mainBox.teamIndex)) continue;

                    //Set target if not assigned
                    if (!target) target = mainBox;
                }
                //Try to hit target
            }
            if (target)
            {
                hitHurtBoxes.Add(target);
                SimulateBullet(pos, target, bouncesRemaining - 1, false, currentDamage * multiplier);
            }
        }

        //Just like the simulations
        private void SimulateBullet(Vector3 pos, HurtBox box, int remainingBounces, bool dynamite, float currentDamage)
        {
            //Get the pos of where the bullet hit
            Vector3 origin = pos;
            //Setup effectdata
            EffectData data = new EffectData()
            {
                start = origin,
                origin = box.transform.position
            };
            //Play the effect
            EffectManager.SpawnEffect(tracerEffect, data, true);

            DamageInfo damageInfo = new DamageInfo()
            {
                attacker = base.gameObject,
                inflictor = base.gameObject,
                crit = isCrit,
                damage = currentDamage,
                position = box.transform.position,
                procCoefficient = procCoef,
                force = Vector3.zero,
                damageType = DamageTypeCombo.GenericPrimary
            };
            box.healthComponent.TakeDamage(damageInfo);
            //Apply onhits
            GlobalEventManager.instance.OnHitEnemy(damageInfo, box.healthComponent.body.gameObject);
            GlobalEventManager.instance.OnHitAll(damageInfo, box.healthComponent.body.gameObject);

            //Continue to attempt ricochet
            if (remainingBounces > 0)
            {
                if (dynamite)
                {
                    HandleRichochet(box.transform.position, remainingBounces, currentDamage);
                    HandleRichochet(box.transform.position, remainingBounces, currentDamage);
                }
                else HandleRichochet(box.transform.position, remainingBounces, currentDamage, richochetMod);
            }
        }
    }
}
