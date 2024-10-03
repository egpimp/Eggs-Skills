using EntityStates;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;

using UnityEngine;

namespace EggsSkills.SkillDefs
{
    [CreateAssetMenu(menuName = "RoR2/SkillDef/AutoshotgunDef")]
    class CaptainAutoshotgunSkilldef : SkillDef
    {
        //% ramp gained per fire
        private static readonly float rampGainPerFire = 0.15f;
        //% of lost ramp per second of not firing as a float
        private static readonly float rampLossPerSecond = 0.35f;
        //How long before the de-ramping starts after not firing
        private static readonly float gracePeriod = 0.2f;

        private float graceTimer;

        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new InstanceData();
        }

        public override EntityState InstantiateNextState([NotNull] GenericSkill skillSlot)
        {
            EntityState state = base.InstantiateNextState(skillSlot);
            InstanceData instanceData = (InstanceData)skillSlot.skillInstanceData;
            IRampSetter setter;
            if ((setter = (state as IRampSetter)) != null) setter.SetRamp(instanceData.ramp);
            return state;
        }

        public override void OnExecute([NotNull] GenericSkill skillSlot)
        {
            base.OnExecute(skillSlot);
            InstanceData instanceData = (InstanceData) skillSlot.skillInstanceData;
            //Ramp up per ability execution, restrict it to a max of 1 (100%)
            if (instanceData.ramp < 1f) instanceData.ramp += rampGainPerFire;
            instanceData.ramp = Mathf.Clamp(instanceData.ramp, 0f, 1f);
            //Reset timer of grace period
            graceTimer = gracePeriod;
        }

        public override void OnFixedUpdate([NotNull] GenericSkill skillSlot, float deltaTime)
        {
            base.OnFixedUpdate(skillSlot, deltaTime);
            InstanceData instanceData = (InstanceData) skillSlot.skillInstanceData;
            //If the skill can execute and the grace period hasn't ended, tickdown the grace period
            if (skillSlot.CanExecute())
            {
                if (graceTimer > 0f) graceTimer -= Time.fixedDeltaTime;
            }
            //If skill can't execute, don't tick down the graceperiod yet
            else graceTimer = gracePeriod;
            //Ramp down per tick based on the percent rate and clamp
            if (graceTimer <= 0f && instanceData.ramp > 0f)
            {
                instanceData.ramp -= rampLossPerSecond * Time.fixedDeltaTime;
                
            }
            instanceData.ramp = Mathf.Clamp(instanceData.ramp, 0f, 1f);
        }

        public class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public float ramp;
        }

        public interface IRampSetter
        {
            void SetRamp(float ramp);
        }
    }
}
