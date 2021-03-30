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
            triggerRadius = 0f;
            base.OnEnter();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(NetworkServer.active && 0.5 <= fixedAge)
            {
                outer.SetNextState(new TeslaArmingFullState());
            }
        }
    }
}
