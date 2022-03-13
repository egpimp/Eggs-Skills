using EntityStates.Engi.Mine;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.ArmingStates
{
    class TeslaArmingWeakState : BaseMineArmingState
    {
        public override void OnEnter()
        {
            //Steal variables again
            var goodState = new MineArmingWeak();
            if(string.IsNullOrEmpty(pathToChildToEnable))
            {
                pathToChildToEnable = goodState.pathToChildToEnable;
                onEnterSfxPlaybackRate = goodState.onEnterSfxPlaybackRate;
                onEnterSfx = goodState.onEnterSfx;
                triggerRadius = goodState.triggerRadius;
                blastRadiusScale = goodState.blastRadiusScale;
                forceScale = goodState.forceScale;
                damageScale = goodState.damageScale;
            }
            //No trigger radius at this stage
            triggerRadius = 0f;
            base.OnEnter();
        }
        public override void FixedUpdate()
        {
            //Set it to having been fully armed after 0.5 seconds
            base.FixedUpdate();
            if(NetworkServer.active && 0.5 <= fixedAge)
            {
                outer.SetNextState(new TeslaArmingFullState());
            }
        }
    }
}
