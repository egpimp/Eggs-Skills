using EntityStates.Engi.Mine;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using EggsSkills.EntityStates.MineStates.MainStates;
using RoR2;
using RoR2.Projectile;

namespace EggsSkills.EntityStates.MineStates
{
    public class TeslaArmState : BaseMineState
    {
        protected override bool shouldStick => true;
        protected override bool shouldRevertToWaitForStickOnSurfaceLost => false;
        public override void OnEnter()
        {
            var goodState = new Arm();
            if(string.IsNullOrEmpty(enterSoundString))
            {
                enterSoundString = goodState.enterSoundString;
            }
            base.OnEnter();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(NetworkServer.active && Arm.duration <= fixedAge)
            {
                outer.SetNextState(new TeslaWaitForTargetState());
            }
        }
    }
}
