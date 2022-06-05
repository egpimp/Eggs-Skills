using EggsSkills.EntityStates.TeslaMine.MineStates.ArmingStates;
using EggsSkills.EntityStates.TeslaMine.MineStates.MainStates;
using EntityStates.Engi.Mine;
using RoR2;
using RoR2.Projectile;
using UnityEngine.Networking;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.MainStates
{
    public class TeslaWaitForTargetState : BaseMineState
    {
        //Skills++
        public static float spp_radiusMult = 1f;

        //Target sphere finder component
        private ProjectileSphereTargetFinder targetFinder;

        //Target component
        private ProjectileTargetComponent projectileTargetComponent;

        //Same things as usual
        public override bool shouldStick => true;
        //Yes
        public override bool shouldRevertToWaitForStickOnSurfaceLost => false;

        public override void OnEnter()
        {
            base.OnEnter();
            //Get target component
            projectileTargetComponent = base.GetComponent<ProjectileTargetComponent>();
            //Get target finder
            targetFinder = base.GetComponent<ProjectileSphereTargetFinder>();

            //Network check
            if(NetworkServer.active)
            {
                //Enable the target finder
                targetFinder.enabled = true;
                //Set the arming state to the last state
                armingStateMachine.SetNextState(new TeslaArmingWeakState()) ;
            }
        }

        public override void OnExit()
        {
            //If it still exists, disable it
            if(targetFinder) targetFinder.enabled = false;
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Network check and making sure finder exists
            if(NetworkServer.active && targetFinder)
            {
                //If we found something set it to the pre-det state
                if(projectileTargetComponent.target) outer.SetNextState(new TeslaPreDetState());
                //Grab arming state
                BaseMineArmingState baseMineArmingState;
                //If the baseminearmingstate isn't fucked up
                if ((baseMineArmingState = (armingStateMachine?.state) as BaseMineArmingState) != null)
                {
                    //Keep it enabled
                    targetFinder.enabled = baseMineArmingState.triggerRadius != 0f;
                    //Determine the range it searches
                    targetFinder.lookRange = baseMineArmingState.triggerRadius;
                };
            }
        }
    }
}
