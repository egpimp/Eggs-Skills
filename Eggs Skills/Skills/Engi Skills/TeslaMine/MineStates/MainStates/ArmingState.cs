using EntityStates.Engi.Mine;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using EggsSkills.EntityStates.TeslaMine.MineStates.MainStates;
using RoR2;
using RoR2.Projectile;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.MainStates
{
    public class TeslaArmState : BaseMineState
    {
        public override bool shouldStick => true;
        public override bool shouldRevertToWaitForStickOnSurfaceLost => false;
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
