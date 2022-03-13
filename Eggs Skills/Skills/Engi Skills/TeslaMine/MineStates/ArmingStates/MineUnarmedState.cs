using EntityStates.Engi.Mine;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.ArmingStates
{
    public class TeslaArmingUnarmedState : BaseMineArmingState
    {
        public override void OnEnter()
        {
            //We're doing the same thing here afaik, just take an existing unarmed state and use the variables from it since it works
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
