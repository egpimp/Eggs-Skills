using UnityEngine;
using RoR2.Projectile;
using UnityEngine.Networking;
using EntityStates.Huntress;

namespace EggsSkills
{
    public class OnStuckCaller : MonoBehaviour
    {
        //Prevents the activation event from occuring more than once
        bool flag = true;
        //Makes sure things continue after it sticks even if it no longer stuck
        bool flag2 = true;

        //Time til thing happen
        private readonly float baseCountDown = 3f;
        //Keep track o time
        private float countDown;
        //Radius of aoe
        private readonly float radiusMax = 25f;
        //Countdown for pulse fx
        private float stopwatch;
        //How long between pulse fx
        private readonly float stopwatchMax = 1f;

        //Indicator
        private GameObject indicator;
        //Owner of the projectile
        private GameObject owner;

        //Projectilecontroller component
        private ProjectileController controller;
        //Stick component of projectile
        private ProjectileStickOnImpact stick;

        private void Start()
        {
            //Start countdown
            this.countDown = this.baseCountDown;
            //Prep stopwatch
            this.stopwatch = this.stopwatchMax;
            //Grab components and owner
            this.owner = GetComponent<ProjectileController>().owner;
            this.stick = GetComponent<ProjectileStickOnImpact>();
            this.controller = GetComponent<ProjectileController>();
            //Establish indicator 
            this.indicator = Object.Instantiate(ArrowRain.areaIndicatorPrefab);
            this.indicator.SetActive(true);
            this.indicator.transform.position = base.transform.position;
            this.indicator.transform.localScale = Vector3.zero;
        }
        private void FixedUpdate()
        {
            //Once it is found to be stuck, flip the flag indicating that it stuck to something
            if (this.stick.stuck) this.flag2 = false;

            //If it has stuck to anything at any point, this will run
            if (!this.flag2)
            {
                //If countdown still above 0...
                if (this.countDown > 0)
                {
                    //Run indicator stuff
                    MarkAffectedZone();
                    //Countdown the timer
                    this.countDown -= Time.fixedDeltaTime;
                }
                //Otherwise if timer has fully countdown...
                else
                {
                    //If flag not tripped yet...
                    if (this.flag)
                    {
                        //Trip flag
                        this.flag = !this.flag;
                        //Kill indicator
                        this.indicator.transform.localScale = Vector3.zero;
                        this.indicator.SetActive(false);
                        GameObject.Destroy(indicator);
                        //Check network, then send position to owner component
                        if (NetworkServer.active) this.owner.GetComponent<SwarmComponent>()?.GetTargets(this.controller.transform.position);
                    }
                }
            }
        }

        private void MarkAffectedZone()
        {
            //If timer, tick it down
            if (this.stopwatch >= 0f) this.stopwatch -= Time.fixedDeltaTime;
            //Otherwise reset the timer
            else this.stopwatch = this.stopwatchMax;

            //Make sure position doesn't fcuk up
            this.indicator.transform.position = base.transform.position;
            //Set scale based on curve generator
            this.indicator.transform.localScale = Vector3.one * GenerateLogScaleCurve();
        }

        private float GenerateLogScaleCurve()
        {
            //Makes counter move lo -> hi instead of hi -> low
            float invert = this.stopwatchMax - this.stopwatch;
            //Then convert that to a 1 -> max range instead
            float convertedScale = EggsUtils.Helpers.Math.ConvertToRange(0f, this.stopwatchMax, 1f, this.radiusMax, invert);
            //Generate the value
            float lnFactor = -this.radiusMax / Mathf.Log(1f / radiusMax);
            //Return the log of the scale times the lnfactor
            return Mathf.Log(convertedScale) * lnFactor;
        }
    }
}
