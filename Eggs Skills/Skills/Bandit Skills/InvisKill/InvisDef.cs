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
            //Setup the invishandler
            InvisHandler invisHandler;
            //If it exists nab it, otherwise make it
            if(skillSlot.characterBody.gameObject.TryGetComponent(out invisHandler)) invisHandler = skillSlot.characterBody.gameObject.AddComponent<InvisHandler>();
            
            //Return instance data with the invishandler
            return new InvisOnSprintSkillDef.InstanceData
            {
                banditInvisHandler = skillSlot.GetComponent<InvisHandler>()
            };
        }
        private static bool IsInvis([NotNull] GenericSkill skillSlot)
        {
            //Grab the handler
            InvisHandler invisHandler = ((InvisOnSprintSkillDef.InstanceData)skillSlot.skillInstanceData).banditInvisHandler;
            //If it exists, check if invis, otherwise return false
            return (invisHandler != null) ? invisHandler.IsInvis() : false;
        }
        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            //Can never execute
            return false;
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            //This makes it highlight only when player is invis
            return base.IsReady(skillSlot) && InvisOnSprintSkillDef.IsInvis(skillSlot);
        }
        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            //Instancedata
            public InvisHandler banditInvisHandler;
        }
    }
}
