using RoR2.Skills;
using RoR2;
using JetBrains.Annotations;
using UnityEngine;

namespace EggsSkills.SkillDefs
{
    [CreateAssetMenu(menuName = "RoR2/SkillDef/ShieldsplosionDef")]
    class ShieldsplosionDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            //Get the health component as instance data
            return new ShieldsplosionDef.InstanceData
            {
                characterHealth = skillSlot.GetComponent<HealthComponent>()
            };
        }

        public bool HasShield([NotNull] GenericSkill skillSlot)
        {
            //Nab the instance data
            ShieldsplosionDef.InstanceData instanceData = (ShieldsplosionDef.InstanceData) skillSlot.skillInstanceData;
            //Use the instance data to check if barrier is over 10% barrier
            return instanceData.characterHealth && (instanceData.characterHealth.barrier >= (instanceData.characterHealth.fullHealth * 0.1f));
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            //Check if they have stock and meet the above req
            return base.HasRequiredStockAndDelay(skillSlot) && this.HasShield(skillSlot);
        }

        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            //Just gets healthcomponent
            public HealthComponent characterHealth;
        }
    }
}
