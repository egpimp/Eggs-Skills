using EntityStates;
using EntityStates.Huntress;
using RoR2;
using UnityEngine;
using System.Linq;
using EntityStates.VagrantMonster;
using EggsSkills.Config;
using static UnityEngine.Random;

namespace EggsSkills.EntityStates
{
    class ZapportChargeEntity : BaseSkillState
    {
        //Min uncharged damage
        private readonly float baseDamageMult = 2.5f;
        //Initial seconds to charge
        private readonly float baseMaxCharge = 2f;
        //Min uncharged distance
        private readonly float baseMinDistance = 6f;
        //Max distance to be added to min at full charge (Total of 20f)
        private readonly float baseMaxDistanceBonus = 20f;
        //Min uncharged kersplodey radius
        private readonly float baseRadius = Configuration.GetConfigValue(Configuration.MageZapportBaseradius);
        //0-100% how charged are you
        private float chargePercent;
        //Damage multiplier
        private float damageMult;
        //Current distance based on charge amount
        private float distance;
        //How long can the ability be charged for
        private float maxCharge;
        //Max amount to boost damagemult by from basedamagemult (Total of 10f)
        private readonly float maxDamageMultBonus = 7.5f;
        //Move speed penalty at full charge
        private readonly float maxMovePenalty = 0.2f;
        //What radius is multiplied by at max charge
        private readonly float maxRadiusMult = 2.5f;
        //Minimum charge to use
        private readonly float minChargePercent = 0.2f;
        //Radius of the explosion
        private float radius;
        //100% charge % chance of fx occuring
        private readonly float ranNumMax = 1f;
        //0% charge % chance of fx occuring
        private readonly float ranNumMin = 0.4f;
        //Timer between frequency checks
        private float ranStopwatch;
        //Num to reset to when timer counts down
        private readonly float ranStopwatchMax = 0.05f;
        //Additive multiplier per stock consumed.  Note we calc radius slightly differently because it scales waaaaay too hard otherwise
        private readonly float stockBonusPerMultiplier = 1.5f;
        private readonly float stockBonusRadiusPerMultiplier = 1.2f;
        //Actual multiplier once calculated
        private float stockMultiplier = 1f;
        private float stockRadiusMultiplier = 1f;

        //Where my kersplodey go indicator
        private GameObject areaIndicator;
        //Cool charging fx
        private GameObject effectPrefab = UnityEngine.Resources.Load<GameObject>("Prefabs/effects/LightningStakeNova");

        //Maximum movement determined
        private Vector3 calculatedMovePos;

        public override void OnEnter()
        {
            //Enter base component
            base.OnEnter();
            //Establish ran num at maxtimer
            ranStopwatch = ranStopwatchMax;
            //Start with 0% charge
            this.chargePercent = 0f;
            //Used to find min
            float[] findMinSpeed = new float[] { base.attackSpeedStat, 4f };
            //Gets the lower value between attackspeed and 4, then determine the maxcharge based on the basemaxcharge divided by the attack speed
            this.maxCharge = this.baseMaxCharge / findMinSpeed.Min();
            //Play the wall holding animation artificer has
            base.PlayAnimation("Gesture, Additive", "PrepWall", "PrepWall.playbackRate", 1);
            //Set the area indicator
            this.areaIndicator = Object.Instantiate(ArrowRain.areaIndicatorPrefab);
            //Activate the area indicator
            this.areaIndicator.SetActive(true);
            //Calculate the stock bonus with multiplier and remaining stocks
            if (base.skillLocator.utility.stock > 0)
            {
                this.stockMultiplier = this.stockBonusPerMultiplier * base.skillLocator.utility.stock;
                this.stockRadiusMultiplier = this.stockBonusRadiusPerMultiplier * base.skillLocator.utility.stock;
            }
            //Deduct the stocks
            base.skillLocator.utility.RemoveAllStocks();
            //Play the charging sound
            Util.PlaySound(ChargeTrackingBomb.chargingSoundString, base.gameObject);
        }

        public override void OnExit()
        {
            //Fix the walkspeed
            base.characterMotor.walkSpeedPenaltyCoefficient = 1f;
            //If indicator exists
            if (this.areaIndicator)
            {
                //Deactivate and delet this
                this.areaIndicator.SetActive(false);
                Destroy(areaIndicator);
            }
            //Base exit component
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            //Handle base fixedupdate stuff
            base.FixedUpdate();
            //Calculate the percent charge based on how long ability been held and the maxcharge
            this.chargePercent = base.fixedAge / this.maxCharge;
            //Originally for testing purposes, it was actually more fun this way though so welcome to the age of holding max charge
            if (chargePercent >= 1f) chargePercent = 1f;
            //Calculate the radius, convert the charge percent (0.2 to 1) to the radius mult (1 to 2.5)
            this.radius = this.baseRadius * EggsUtils.Helpers.Math.ConvertToRange(0f, 1f, 1f, this.maxRadiusMult, this.chargePercent) * this.stockRadiusMultiplier;
            //Recalculate the intended move pos
            this.RecalculatePos();
            //Adjust indicator accordingly
            this.IndicatorUpdator();
            //Play the fx while charging the skill
            this.PlayChargeFX();

            //Set movespeed based on charge percent, 1x to (1 - maxMovePenalty) normal speed while charging
            base.characterMotor.walkSpeedPenaltyCoefficient = 1f - this.chargePercent * this.maxMovePenalty;
            //Calculate damage, if not min charge the damage is the minimum, if min charge met damage is min + percent converted from 0.2 - 1 to 0 - 7.5, weirdness stops NREs
            this.damageMult = chargePercent >= minChargePercent ? baseDamageMult + EggsUtils.Helpers.Math.ConvertToRange(minChargePercent, 1f, 0f, this.maxDamageMultBonus, chargePercent) : baseDamageMult;
            this.damageMult *= this.stockMultiplier;

            //If button is no longer held down AND the charge percent is at or above the minimum
            if ((!base.IsKeyDownAuthority() && this.chargePercent >= this.minChargePercent))
            {
                //Network check
                if (base.isAuthority)
                {
                    //Set next state
                    ZapportFireEntity nextState = new ZapportFireEntity();
                    //Transfer these stats to fire state
                    nextState.damageMult = damageMult;
                    nextState.radius = radius;
                    nextState.movePos = calculatedMovePos;
                    //Execute next state
                    this.outer.SetNextState(nextState);
                }
                return;
            }
        }

        private void RecalculatePos()
        {
            //Should give us a amount of speed based on movespeed, 2x ms = 2x teleport distance
            float speedMod = base.moveSpeedStat / 7f;
            //Calculate distance with min distance, then add the bonus times the max distance times the speedmod.
            this.distance = this.baseMinDistance + this.chargePercent * this.baseMaxDistanceBonus * speedMod * this.stockMultiplier;
            //Create the aimray
            Ray aimRay = GetAimRay();
            //Spherecast with the players radius, get all objects hit
            RaycastHit[] hits = Physics.SphereCastAll(aimRay.origin - aimRay.direction * 0.5f, 1.4f, aimRay.direction, distance + 0.5f, LayerIndex.CommonMasks.bullet);
            //Look through the everything hit, filter out the player
            hits = hits.Where(n => n.transform.root.gameObject.name != "mdlMage").ToArray();
            //Now we have our list, if the only thing hit was the player (and thus the array is now empty) we can do a normal full-range teleport
            if (hits.Length == 0) calculatedMovePos = aimRay.GetPoint(distance);
            //But if there is any ACTUAL collisions
            else
            {
                //Grab the first, nearest hit
                RaycastHit hit = hits.OrderBy(n => n.distance).FirstOrDefault();
                //Calculate center of the sphere that collided
                Vector3 cCent = aimRay.origin + aimRay.direction * hit.distance;
                //Set position to bottom-most point of that sphere, which is the center pos moved down by the radius
                calculatedMovePos = cCent;
            }
        }

        private void IndicatorUpdator()
        {
            //Set pos to the intended position
            this.areaIndicator.transform.position = this.calculatedMovePos;
            //Set scale to the radius
            this.areaIndicator.transform.localScale = this.radius * Vector3.one;
        }

        private void PlayChargeFX()
        {
            //Convert percent charge to a value ranging from minran% to maxran%
            float ranPercent = EggsUtils.Helpers.Math.ConvertToRange(0f, 1f, ranNumMin, ranNumMax, chargePercent);
            //If stopwatch isn't ticked all the way down, tick it down
            if (this.ranStopwatch >= 0f) this.ranStopwatch -= Time.fixedDeltaTime;
            //If stopwatch is fully ticked down...
            else
            {
                //Reset stopwatch before performing other stuff
                this.ranStopwatch = this.ranStopwatchMax;
                //Grab random num 0 - 1
                float ranNum = Range(0f, 1f);
                //Check if it is lower than the ranPercent, functionally makes ranpercent a % chance of hitting & play at target pos
                if (ranNum <= ranPercent) EffectManager.SimpleEffect(this.effectPrefab, this.calculatedMovePos + insideUnitSphere * this.radius, Quaternion.identity, true);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}