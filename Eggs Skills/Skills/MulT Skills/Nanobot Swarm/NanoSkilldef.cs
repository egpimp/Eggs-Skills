using RoR2;
using RoR2.Skills;
using UnityEngine;
using JetBrains.Annotations;

namespace EggsSkills.SkillDefs
{
    class NanoSkilldef : SkillDef
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            skillSlot.characterBody.gameObject.AddComponent<SwarmComponent>();
            return base.OnAssigned(skillSlot);
        }
    }
}
