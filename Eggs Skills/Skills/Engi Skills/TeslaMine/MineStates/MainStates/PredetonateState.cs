using EntityStates.Engi.Mine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using UnityEngine.Networking;
using EggsSkills.EntityStates.TeslaMine.MineStates.MainStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.MainStates
{
    public class TeslaPreDetState : BaseMineState
    {
        public override bool shouldStick => true;
        public override bool shouldRevertToWaitForStickOnSurfaceLost => false;
        public override void OnEnter()
        {
            GameObject owner = GetComponent<ProjectileController>().owner;
            CharacterBody body = owner.GetComponent<CharacterBody>();
            if(body)
            {
                CharacterMaster master = body.master;
                if(master)
                {
                    master.RemoveDeployable(GetComponent<Deployable>());
                }
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
