using RoR2.Skills;
using RoR2;
using JetBrains.Annotations;
using UnityEngine;

namespace EggsSkills.SkillDefs
{
    [CreateAssetMenu(menuName = "RoR2/SkillDef/MercSlashportDef")]
    class ShieldsplosionDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new ShieldsplosionDef.InstanceData
            {
                characterHealth = skillSlot.GetComponent<HealthComponent>()
            };
        }
        public bool HasShield([NotNull] GenericSkill skillSlot)
        {
            ShieldsplosionDef.InstanceData instanceData = (ShieldsplosionDef.InstanceData)
                skillSlot.skillInstanceData;
            return instanceData.characterHealth && (instanceData.characterHealth.barrier >= (instanceData.characterHealth.fullHealth * 0.1f));
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.HasRequiredStockAndDelay(skillSlot) && this.HasShield(skillSlot);
        }
        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
        public HealthComponent characterHealth;
        }
    }
}
