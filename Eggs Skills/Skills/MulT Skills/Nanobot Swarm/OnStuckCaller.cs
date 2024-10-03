using UnityEngine;
using RoR2.Projectile;
using UnityEngine.Networking;
using EntityStates.Huntress;
using UnityEngine.AddressableAssets;

namespace EggsSkills
{
    public class OnStuckCaller : MonoBehaviour
    {
        //Prevents the activation event from occuring more than once
        bool flag = true;
        //Makes sure things continue after it sticks even if it no longer stuck
        bool flag2 = true;

        //Time til thing happen
        private static readonly float baseCountDown = 3f;
        //Keep track o time
        private float countDown;
        //Radius of aoe
        private static readonly float radiusMax = 25f;
        //Countdown for pulse fx
        private float stopwatch;
        //How long between pulse fx
        private static readonly float stopwatchMax = 1f;

        //Indicator
        private GameObject indicator;
        //Owner of the projectile
        private GameObject owner;
        //Area prefab
        private GameObject areaPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Huntress/HuntressArrowRainIndicator.prefab").WaitForCompletion();

        //Projectilecontroller component
        private ProjectileController controller;
        //Stick component of projectile
        private ProjectileStickOnImpact stick;

        private void Start()
        {
            //Start countdown
            countDown = baseCountDown;
            //Prep stopwatch
            stopwatch = stopwatchMax;
            //Grab components and owner
            owner = GetComponent<ProjectileController>().owner;
            stick = GetComponent<ProjectileStickOnImpact>();
            controller = GetComponent<ProjectileController>();
            //Establish indicator 
            indicator = Instantiate(areaPrefab);
            indicator.SetActive(true);
            indicator.transform.position = base.transform.position;
            indicator.transform.localScale = Vector3.zero;
        }

        private void FixedUpdate()
        {
            //Once it is found to be stuck, flip the flag indicating that it stuck to something
            if (stick.stuck) flag2 = false;

            //If it has stuck to anything at any point, this will run
            if (!flag2)
            {
                //If countdown still above 0...
                if (countDown > 0)
                {
                    //Run indicator stuff
                    MarkAffectedZone();
                    //Countdown the timer
                    countDown -= Time.fixedDeltaTime;
                }
                //Otherwise if timer has fully countdown...
                else
                {
                    //If flag not tripped yet...
                    if (flag)
                    {
                        //Trip flag
                        flag = false;
                        //Kill indicator
                        indicator.transform.localScale = Vector3.zero;
                        indicator.SetActive(false);
                        Destroy(indicator);
                        //Check network, then send position to owner component
                        if (NetworkServer.active) owner.GetComponent<SwarmComponent>()?.GetTargets(controller.transform.position);
                    }
                }
            }
        }

        private void MarkAffectedZone()
        {
            //If timer, tick it down
            if (stopwatch >= 0f) stopwatch -= Time.fixedDeltaTime;
            //Otherwise reset the timer
            else stopwatch = stopwatchMax;

            //Make sure position doesn't fcuk up
            indicator.transform.position = base.transform.position;
            //Set scale based on curve generator
            indicator.transform.localScale = Vector3.one * GenerateLogScaleCurve();
        }

        private float GenerateLogScaleCurve()
        {
            //Makes counter move lo -> hi instead of hi -> low
            float invert = stopwatchMax - stopwatch;
            //Then convert that to a 1 -> max range instead
            float convertedScale = EggsUtils.Helpers.Math.ConvertToRange(0f, stopwatchMax, 1f, radiusMax, invert);
            //Generate the value
            float lnFactor = -radiusMax / Mathf.Log(1f / radiusMax);
            //Return the log of the scale times the lnfactor
            return Mathf.Log(convertedScale) * lnFactor;
        }
    }
}
