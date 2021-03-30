using EntityStates.Engi.Mine;
using UnityEngine.Networking;
using EggsSkills.EntityStates.TeslaMine.MineStates.ArmingStates;
using RoR2;
using RoR2.Projectile;
using System.EnterpriseServices;
using UnityEngine.Events;
using EntityStates;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.MainStates
{
    public class TeslaWaitForStick : BaseMineState
    {
        protected override bool shouldStick => true;
        protected override bool shouldRevertToWaitForStickOnSurfaceLost => false;
        private ProjectileSphereTargetFinder targetFinder;
        public override void OnEnter()
        {
            base.OnEnter();
            if(NetworkServer.active)
            {
                armingStateMachine.SetNextState(new TeslaArmingUnarmedState());
                targetFinder = GetComponent<ProjectileSphereTargetFinder>();
                if(targetFinder)
                {
                    targetFinder.enabled = false;
                };
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(NetworkServer.active && projectileStickOnImpact.stuck)
            {
                outer.SetNextState(new TeslaArmState());
            };
        }
    }
}
