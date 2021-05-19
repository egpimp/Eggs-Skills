using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using JetBrains.Annotations;

namespace EggsSkills.SkillDefs
{
    [CreateAssetMenu(menuName = "RoR2/SkillDef/InvisKillDef")]
    class InvisOnSprintSkillDef : SkillDef
    {
        public override BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            InvisHandler invisHandler = skillSlot.characterBody.gameObject.GetComponent<InvisHandler>();
            if(!invisHandler)
            {
                invisHandler = skillSlot.characterBody.gameObject.AddComponent<InvisHandler>();
            };
            return new InvisOnSprintSkillDef.InstanceData
            {
                banditInvisHandler = skillSlot.GetComponent<InvisHandler>()
            };
        }
        private static bool IsInvis([NotNull] GenericSkill skillSlot)
        {
            InvisHandler invisHandler = ((InvisOnSprintSkillDef.InstanceData)skillSlot.skillInstanceData).banditInvisHandler;
            return (invisHandler != null) ? invisHandler.IsInvis() : false;
        }
        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return false;
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && InvisOnSprintSkillDef.IsInvis(skillSlot);
        }
        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public InvisHandler banditInvisHandler;
        }
    }
}
