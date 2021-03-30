using EntityStates.Engi.Mine;
using System;
using System.Collections.Generic;
using System.Text;

namespace EggsSkills.Skills.TeslaMine.MineStates.ArmingStates
{
    class TeslaArmingFullState : BaseMineArmingState
    {
        public override void OnEnter()
        {
            var goodState = new MineArmingFull();
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
            base.OnEnter();
        }
    }
}
