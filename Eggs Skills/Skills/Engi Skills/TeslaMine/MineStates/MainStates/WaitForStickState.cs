using EntityStates.Engi.Mine;
using UnityEngine.Networking;
using EggsSkills.EntityStates.TeslaMine.MineStates.ArmingStates;
using RoR2.Projectile;


namespace EggsSkills.EntityStates.TeslaMine.MineStates.MainStates
{
    public class TeslaWaitForStick : BaseMineState
    {
        //Duh yes should stick
        public override bool shouldStick => true;
        //No reverting please
        public override bool shouldRevertToWaitForStickOnSurfaceLost => false;
        //Helps find targets
        private ProjectileSphereTargetFinder targetFinder;

        public override void OnEnter()
        {
            base.OnEnter();
            //Network check
            if(NetworkServer.active)
            {
                //Set the arming machine state
                armingStateMachine.SetNextState(new TeslaArmingUnarmedState());
                //Establish the targetfnider
                targetFinder = GetComponent<ProjectileSphereTargetFinder>();
                //It should exist, and if it does disable it cause we ain't searching yet
                if(targetFinder) targetFinder.enabled = false;
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Network check and only if object is stuck set it to the arming state
            if(NetworkServer.active && projectileStickOnImpact.stuck) outer.SetNextState(new TeslaArmState());
        }
    }
}
