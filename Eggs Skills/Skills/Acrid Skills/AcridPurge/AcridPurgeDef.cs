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
            //This lets us track everyone that would be affected by this ability
            AcridPurgeTracker charTracker;
            //Try to get it, add it if not exist
            if (!skillSlot.characterBody.gameObject.TryGetComponent<AcridPurgeTracker>(out charTracker)) charTracker = skillSlot.characterBody.gameObject.AddComponent<AcridPurgeTracker>();
            //No idea what this does but the devs do it so I think it works
            return new AcridPurgeDef.InstanceData
            {
                acridTracker = skillSlot.GetComponent<AcridPurgeTracker>()
            };
        }
        private static bool HasPoisoned([NotNull] GenericSkill skillSlot)
        {
            //Get the tracker
            AcridPurgeTracker acridTracker = ((AcridPurgeDef.InstanceData)skillSlot.skillInstanceData).acridTracker;
            //Returns whether or not there are any poisoned units, if none skill no worky
            return (acridTracker != null) ? acridTracker.GetPoisonedCount() >= 1 : false;
        }
        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            //Canexecute handles skill being disabled if no targets
            return base.CanExecute(skillSlot) && AcridPurgeDef.HasPoisoned(skillSlot);
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            //Same as above basically
            return base.IsReady(skillSlot) && AcridPurgeDef.HasPoisoned(skillSlot);
        }
        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            //It's a thing for handling the tracker
            public AcridPurgeTracker acridTracker;
        }
    }
}
