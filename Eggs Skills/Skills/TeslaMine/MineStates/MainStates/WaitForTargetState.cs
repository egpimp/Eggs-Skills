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
        private ProjectileSphereTargetFinder targetFinder;
        private ProjectileTargetComponent projectileTargetComponent;
        public override bool shouldStick => true;
        public override bool shouldRevertToWaitForStickOnSurfaceLost => false;
        public override void OnEnter()
        {
            base.OnEnter();
            projectileTargetComponent = base.GetComponent<ProjectileTargetComponent>();
            targetFinder = base.GetComponent<ProjectileSphereTargetFinder>();
            if(NetworkServer.active)
            {
                targetFinder.enabled = true;
                armingStateMachine.SetNextState(new TeslaArmingWeakState()) ;
            }
        }
        public override void OnExit()
        {
            if(targetFinder)
            {
                targetFinder.enabled = false;
            }
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(NetworkServer.active && targetFinder)
            {
                if(projectileTargetComponent.target)
                {
                    outer.SetNextState(new TeslaPreDetState());
                }
                BaseMineArmingState baseMineArmingState;
                if ((baseMineArmingState = (armingStateMachine?.state) as BaseMineArmingState) != null)
                {
                    targetFinder.enabled = baseMineArmingState.triggerRadius != 0f;
                    targetFinder.lookRange = baseMineArmingState.triggerRadius;
                };
            }
        }
    }
}
