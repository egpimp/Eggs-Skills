using EntityStates;
using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace EggsSkills.SkillDefs
{
    internal class VoidsurvivorSkilldefFix : SkillDef
    {
        public SkillDef alt;

        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            On.EntityStates.VoidSurvivor.CorruptMode.CorruptMode.OnEnter += (orig, self) =>
            {
                if (self.characterBody.skillLocator.special.skillDef == this) self.specialOverrideSkillDef = alt;
                orig(self);
            };
            return base.OnAssigned(skillSlot);
        }
    }
}
