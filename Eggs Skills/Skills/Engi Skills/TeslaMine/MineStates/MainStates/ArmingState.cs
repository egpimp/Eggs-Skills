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
        //It should stick in this stage
        public override bool shouldStick => true;
        //We don't want it trying to stick again after falling off of something
        public override bool shouldRevertToWaitForStickOnSurfaceLost => false;

        public override void OnEnter()
        {
            //Steal the functioning arm state, take it's soun string
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
            //If network server active and the tesla has officially armed, it looks for targets
            if(NetworkServer.active && Arm.duration <= fixedAge)
            {
                outer.SetNextState(new TeslaWaitForTargetState());
            }
        }
    }
}
