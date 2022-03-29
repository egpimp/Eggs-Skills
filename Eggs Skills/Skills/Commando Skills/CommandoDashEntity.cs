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
        private readonly float baseDashSpeed = 4f;
        //Duration buff
        private readonly float buffDuration = Configuration.GetConfigValue(Configuration.CommandoDashBuffTimer);
        //How long dash last
        private readonly float dashDuration = 0.3f;
        //Calculated dash speed
        private float dashSpeed;

        //Vector3 holder for forward direction
        private Vector3 forwardDirection;

        public override void OnEnter()
        {
            //Execute enter procedure
            base.OnEnter();
            //Play sound
            Util.PlaySound(DodgeState.dodgeSoundString, base.gameObject);
            //Handle animation
            base.PlayAnimation("Body", "DodgeForward", "Dodge.playbackRate", this.dashDuration);
            //If moving, get move direction.  Otherwise, get character direction.
            base.characterDirection.forward = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            //Get the invulnerability buff
            if(NetworkServer.active) base.characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, this.dashDuration + this.buffDuration);
            //Determine dash speed with base speed and movespeed
            this.dashSpeed = this.baseDashSpeed * base.moveSpeedStat;
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
            this.forwardDirection = base.characterDirection.forward;
            //Execute motion per tick based on speed in forward direction
            base.characterMotor.rootMotion += this.forwardDirection * Time.fixedDeltaTime * this.dashSpeed;
            //If dash over, and network check, next state to main
            if (base.fixedAge >= this.dashDuration && base.isAuthority) this.outer.SetNextStateToMain();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //Can only be interrupted by freezing
            return InterruptPriority.Frozen;
        }
    }
}
