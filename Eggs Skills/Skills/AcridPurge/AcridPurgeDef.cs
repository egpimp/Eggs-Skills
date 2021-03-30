using RoR2;
using RoR2.Skills;
using JetBrains.Annotations;
using UnityEngine;
using EggsSkills.EntityStates;

namespace EggsSkills.SkillDefs
{
    [CreateAssetMenu(menuName = "RoR2/SkillDef/AcridPurgeDef")]

    class AcridPurgeDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            var charTracker = skillSlot.characterBody.gameObject.GetComponent<AcridPurgeTracker>();
            if (!charTracker)
            {
                skillSlot.characterBody.gameObject.AddComponent<AcridPurgeTracker>();
            };
            return new AcridPurgeDef.InstanceData
            {
                acridTracker = skillSlot.GetComponent<AcridPurgeTracker>()
            };
        }
        private static bool HasPoisoned([NotNull] GenericSkill skillSlot)
        {
            AcridPurgeTracker acridTracker = ((AcridPurgeDef.InstanceData)skillSlot.skillInstanceData).acridTracker;
            return (acridTracker != null) ? acridTracker.GetPoisonedCount() >= 1 : false;
        }
        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return base.CanExecute(skillSlot) && AcridPurgeDef.HasPoisoned(skillSlot);
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && AcridPurgeDef.HasPoisoned(skillSlot);
        }
        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public AcridPurgeTracker acridTracker;
        }
    }
}
