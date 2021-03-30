using EggsSkills.EntityStates.MineStates;
using EntityStates.Engi.Mine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using UnityEngine.Networking;
using EggsSkills.EntityStates.MineStates.MainStates;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.MainStates
{
    public class TeslaPreDetState : BaseMineState
    {
        protected override bool shouldStick => true;
        protected override bool shouldRevertToWaitForStickOnSurfaceLost => false;
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(NetworkServer.active && PreDetonate.duration <= fixedAge)
            {
                outer.SetNextState(new TeslaDetonateState());
            };
        }
    }
}
