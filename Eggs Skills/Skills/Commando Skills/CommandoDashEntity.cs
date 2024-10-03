using EggsSkills.Config;
using EntityStates;
using RoR2;
using UnityEngine;
using EntityStates.Commando;
using UnityEngine.Networking;

namespace EggsSkills.EntityStates
{
    class CommandoDashEntity : BaseSkillState
    {
        //Base speed
        private static readonly float baseDashSpeed = 4f;
        //Duration buff
        private static readonly float buffDuration = Configuration.GetConfigValue(Configuration.CommandoDashBuffTimer);
        //How long dash last
        private static readonly float dashDuration = 0.3f;
        //Calculated dash speed
        private float dashSpeed;

        //Sound string
        private static readonly string soundString = "Play_commando_shift";

        //Vector3 holder for forward direction
        private Vector3 forwardDirection;

        public override void OnEnter()
        {
            //Execute enter procedure
            base.OnEnter();
            //Play sound
            Util.PlaySound(soundString, base.gameObject);
            //Handle animation
            base.PlayAnimation("Body", "DodgeForward", "Dodge.playbackRate", dashDuration);
            //If moving, get move direction.  Otherwise, get character direction.
            base.characterDirection.forward = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            //Get the invulnerability buff
            if(NetworkServer.active) base.characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, dashDuration + buffDuration);
            //Determine dash speed with base speed and movespeed
            dashSpeed = baseDashSpeed * base.moveSpeedStat;
            //If secondary stock not at max, add a stock
            if (base.skillLocator.secondary.stock < base.skillLocator.secondary.maxStock) base.skillLocator.secondary.AddOneStock();
        }

        public override void FixedUpdate()
        {
            //Standard update procedure
            base.FixedUpdate();
            //Determine the player move vector based on input
            base.characterBody.characterDirection.moveVector = base.inputBank.moveVector;
            //Get forward direction
            forwardDirection = base.characterDirection.forward;
            //Gives us a 2 -> 0.5 scale based on dash duration, used to increase dash speed early and slow it down near end
            float speedMult = 2 - EggsUtils.Helpers.Math.ConvertToRange(0f, dashDuration, 0f, 1.5f, base.fixedAge);
            //Execute motion per tick based on speed in forward direction
            base.characterMotor.rootMotion += forwardDirection * Time.fixedDeltaTime * dashSpeed * speedMult;
            //If dash over, and network check, next state to main
            if (base.fixedAge >= dashDuration && base.isAuthority) outer.SetNextStateToMain();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //Can only be interrupted by freezing
            return InterruptPriority.Frozen;
        }
    }
}
