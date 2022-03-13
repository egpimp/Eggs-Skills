using RoR2;
using RoR2.Skills;
using JetBrains.Annotations;

namespace EggsSkills.SkillDefs
{
    class NanoSkilldef : SkillDef
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            //This just assigns the components to MUL-T when you spawn in with the skill
            skillSlot.characterBody.gameObject.AddComponent<SwarmComponent>();
            return base.OnAssigned(skillSlot);
        }
    }
}
