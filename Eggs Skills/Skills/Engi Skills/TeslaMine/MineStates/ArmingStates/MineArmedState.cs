using EntityStates.Engi.Mine;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.ArmingStates
{
    class TeslaArmingFullState : BaseMineArmingState
    {
        public override void OnEnter()
        {
            //Imma be honest, I don't understand most of this shit, it's just standard mine arming stuff tho
            var goodState = new MineArmingFull();
            //Actually maybe, we're taking an existing mine state that works and stealing it's variables, I think
            if(string.IsNullOrEmpty(base.pathToChildToEnable))
            {
                base.pathToChildToEnable = goodState.pathToChildToEnable;
                base.onEnterSfxPlaybackRate = goodState.onEnterSfxPlaybackRate;
                base.onEnterSfx = goodState.onEnterSfx;
                base.triggerRadius = goodState.triggerRadius;
                base.blastRadiusScale = goodState.blastRadiusScale;
                base.forceScale = goodState.forceScale;
                base.damageScale = goodState.damageScale;
            }
            base.OnEnter();
        }
    }
}
