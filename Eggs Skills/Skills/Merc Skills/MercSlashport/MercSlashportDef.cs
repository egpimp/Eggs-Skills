using RoR2;
using RoR2.Skills;
using JetBrains.Annotations;
using UnityEngine;

namespace EggsSkills.SkillDefs
{
    [CreateAssetMenu(menuName = "RoR2/SkillDef/MercSlashportDef")]
    public class MercSlashportDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            var charTracker = skillSlot.characterBody.gameObject.GetComponent<MercSlashportTracker>();
            if (!charTracker)
            {
                skillSlot.characterBody.gameObject.AddComponent<MercSlashportTracker>();
            };
            return new MercSlashportDef.InstanceData
            {
                mercTracker = skillSlot.GetComponent<MercSlashportTracker>()
            };
        }
        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            MercSlashportTracker mercTracker = ((MercSlashportDef.InstanceData) skillSlot.skillInstanceData).mercTracker;
            return mercTracker?.GetTrackingTarget();
        }
        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return base.CanExecute(skillSlot) && MercSlashportDef.HasTarget(skillSlot);
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && MercSlashportDef.HasTarget(skillSlot);
        }
        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public MercSlashportTracker mercTracker;
        }
    }
}
