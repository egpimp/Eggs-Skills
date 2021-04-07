using EntityStates.Engi.Mine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using UnityEngine.Networking;
using EggsSkills.EntityStates.TeslaMine.MineStates.MainStates;
using RoR2;
using RoR2.Projectile;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.MainStates
{
    public class TeslaPreDetState : BaseMineState
    {
        public override bool shouldStick => true;
        public override bool shouldRevertToWaitForStickOnSurfaceLost => false;
        public override void OnEnter()
        {
            if(base.GetComponent<Deployable>())
            {
                Destroy(base.GetComponent<Deployable>());
            }
            if(base.GetComponent<ProjectileDeployToOwner>())
            {
                Destroy(base.GetComponent<ProjectileDeployToOwner>());
            }
            base.OnEnter();
        }
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
