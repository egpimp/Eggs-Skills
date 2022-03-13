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
            //Grab the tracker
            var charTracker = skillSlot.characterBody.gameObject.GetComponent<MercSlashportTracker>();
            //If it doesn't exist add it
            if (!charTracker) skillSlot.characterBody.gameObject.AddComponent<MercSlashportTracker>();
            //Return instance data
            return new MercSlashportDef.InstanceData
            {
                //Return the thing
                mercTracker = skillSlot.GetComponent<MercSlashportTracker>()
            };
        }
        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            //Grab the tracker
            MercSlashportTracker mercTracker = ((MercSlashportDef.InstanceData) skillSlot.skillInstanceData).mercTracker;
            //Return true of there is a target
            return mercTracker?.GetTrackingTarget();
        }
        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            //Can execute if has target
            return base.CanExecute(skillSlot) && MercSlashportDef.HasTarget(skillSlot);
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            //Is ready if has target
            return base.IsReady(skillSlot) && MercSlashportDef.HasTarget(skillSlot);
        }
        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            //Instance data
            public MercSlashportTracker mercTracker;
        }
    }
}
