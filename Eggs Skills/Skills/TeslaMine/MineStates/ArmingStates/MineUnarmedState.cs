using EntityStates.Engi.Mine;
using System;
using System.Collections.Generic;
using System.Text;

namespace EggsSkills.EntityStates.MineStates.ArmingStates
{
    public class TeslaArmingUnarmedState : BaseMineArmingState
    {
        public override void OnEnter()
        {
            var goodState = new MineArmingUnarmed();
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
