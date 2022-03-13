using EntityStates.Engi.Mine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.MainStates
{
    public class TeslaPreDetState : BaseMineState
    {
        //Shoulds tick yes
        public override bool shouldStick => true;
        //No revert pls
        public override bool shouldRevertToWaitForStickOnSurfaceLost => false;

        public override void OnEnter()
        {
            //Get owner
            GameObject owner = GetComponent<ProjectileController>().owner;
            //Get owner body
            CharacterBody body = owner.GetComponent<CharacterBody>();
            //If it exist
            if(body)
            {
                //Set master
                CharacterMaster master = body.master;
                //If master exist, free up mine slot
                if(master) master.RemoveDeployable(GetComponent<Deployable>());
            }
            base.OnEnter();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //If predet is done, move to full det
            if(NetworkServer.active && PreDetonate.duration <= fixedAge)
            {
                outer.SetNextState(new TeslaDetonateState());
            };
        }
    }
}
