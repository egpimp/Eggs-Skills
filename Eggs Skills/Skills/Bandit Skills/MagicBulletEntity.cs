using EggsSkills.Config;
using EntityStates.Bandit2.Weapon;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace EggsSkills.EntityStates
{
    class MagicBulletEntity : Bandit2FirePrimaryBase
    {
        //We use this for referencing assets
        private Bandit2FireRifle assetRef = new Bandit2FireRifle();

        //Whether or not it crit
        private bool isCrit;

        //The actual bulletattack
        private BulletAttack attack;

        //Damage coefficient of the skill
        private readonly float baseDamageCoef = 2f;
        //End damage of the ability
        private float damage;
        //Proc coefficient of the skill
        private readonly float procCoef = 1f;

        //How many richochets
        private readonly int maxRecursion = Configuration.GetConfigValue(Configuration.BanditMagicbulletRicochets);
        //Helps us track how many more times it can bounce
        private int recursion;

        //Keeps track of enemies we already hit
        private List<HurtBox> hitHurtBoxes = new List<HurtBox>();

        //Called on entry of the skillstate
        public override void OnEnter()
        {
            //Base duration of the skill + animations
            this.baseDuration = 1.2f;
            //Lowest possible duration of the skill
            this.minimumBaseDuration = 0.1f;
            //Find out if it is critting
            this.isCrit = base.RollCrit();
            //It hasn't recursed yet, set to 0
            this.recursion = 0;
            //Make player face the aimdirection
            base.StartAimMode();
            //Do standard onenter stuff
            base.OnEnter();
        }

        //Called by base onenter, we replace it with our own special bullet
        public override void FireBullet(Ray aimRay)
        {
            //Play the sound
            Util.PlaySound(this.assetRef.fireSoundString, base.gameObject);
            //Network check
            if (base.isAuthority)
            {
                //Set the damage accordingly
                this.damage = this.baseDamageCoef * base.characterBody.damage;
                //Create the bulletattack
                attack = new BulletAttack
                {
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    tracerEffectPrefab = this.assetRef.tracerEffectPrefab,
                    muzzleName = this.assetRef.muzzleName,
                    hitEffectPrefab = this.assetRef.hitEffectPrefab,
                    damage = this.damage,
                    owner = base.gameObject,
                    isCrit = this.isCrit,
                    maxDistance = 1000f,
                    smartCollision = true,
                    procCoefficient = this.procCoef,
                    HitEffectNormal = false,
                    falloffModel = BulletAttack.FalloffModel.None,
                    weapon = base.gameObject,
                    hitCallback = CallBack
                };
                //Fire
                attack.Fire();
            }
            //Apply the muzzleflash
            EffectManager.SimpleMuzzleFlash(assetRef.muzzleFlashPrefab, base.gameObject, this.assetRef.muzzleName, false);
        }
        
        //Called when exiting the skill state
        public override void OnExit()
        {
            //Proceed with standard exit procedure
            base.OnExit();
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
            HandleRichochet(pos);
            //Return the previously found hit / nohit
            return hit;
        }

        //What to do when attempting to richochet the bullet
        private void HandleRichochet(Vector3 pos)
        {
            //Hold variable for whether target is found or not
            bool targetFound = false;
            //If the bullet can still recurse...
            if (this.recursion < this.maxRecursion)
            {
                //Half the damage
                this.damage /= 2;
                //Loop via spheresearch
                foreach (HurtBox hurtBox in new SphereSearch
                {
                    origin = pos,
                    radius = 25f,
                    mask = LayerIndex.entityPrecise.mask,
                }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
                {
                    //Get the mainhurtbox
                    HurtBox mainBox = hurtBox.hurtBoxGroup.mainHurtBox;
                    //If the mainhurtbox is not yet in our list...
                    if (!hitHurtBoxes.Contains(mainBox))
                    {
                        //Add the found target to the list
                        this.hitHurtBoxes.Add(mainBox);
                        //Trip the bool flag
                        targetFound = true;
                        //Mark that we recursed 
                        this.recursion += 1;
                        //Emulate the bullet, because doing another ACTUAL bullet attack is hell and a half, also feels bad to have all your shots miss cause some geometry
                        SimulateBullet(pos, mainBox);
                        //End the loop cause we found our target
                        break;
                    }
                }
                //If we go through having never found a target, max out the recursion so the loop ends completely
                if(!targetFound) this.recursion = this.maxRecursion;
            }
        }

        //Just like the simulations
        private void SimulateBullet(Vector3 pos, HurtBox box)
        {
            //Get the pos of where the bullet hit
            Vector3 origin = pos;
            //Get the direction between enemy and bullet hit
            Vector3 dir = EggsUtils.Helpers.Math.GetDirection(origin, box.transform.position);
            //Distance between enemy and bullet hit pos
            float dist = Vector3.Distance(box.transform.position, origin);
            //Setup effectdata
            EffectData data = new EffectData()
            {
                start = origin,
                origin = box.transform.position
            };
            //Play the effect
            EffectManager.SpawnEffect(assetRef.tracerEffectPrefab, data, true);
            //This used to be takedamage but it was unreliable lol, just explode them point blank
            new BlastAttack
            {
                radius = 0.1f,
                baseDamage = this.damage,
                procCoefficient = 1f,
                position = box.transform.position,
                attacker = base.gameObject,
                teamIndex = base.teamComponent.teamIndex,
                baseForce = 0F,
                bonusForce = Vector3.zero,
                crit = base.RollCrit(),
                damageType = DamageType.Generic,
                falloffModel = BlastAttack.FalloffModel.None,
                losType = BlastAttack.LoSType.None,
                inflictor = base.gameObject
            }.Fire();
            //Continue to attempt richocet
            this.HandleRichochet(box.transform.position);
        }
    }
}
