using EggsSkills.EntityStates;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;

namespace EggsSkills.SkillModifiers
{
    [SkillLevelModifier("CombatShotgun", typeof(CombatShotgunEntity))]
    class CommandoCombatShotgunModifier : SimpleSkillModifier<CombatShotgunEntity>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            //+0.1f proc chance per level
            CombatShotgunEntity.spp_procMod = level * 0.1f;
            CombatShotgunEntity.spp_bulletMod = (uint)level;
        }
    }
}
