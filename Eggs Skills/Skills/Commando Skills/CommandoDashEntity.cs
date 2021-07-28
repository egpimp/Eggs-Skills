using EggsSkills.Config;
using EntityStates;
using RoR2;
using UnityEngine;

namespace EggsSkills.EntityStates
{
    class CommandoDashEntity : BaseSkillState
    {
        private float baseDashSpeed = 8f;
        private float buffDuration = Configuration.GetConfigValue<float>(Configuration.CommandoDashBuffTimer);
        private float dashDuration = 0.1f;
        private float dashSpeed;

        private Vector3 forwardDirection;
        public override void OnEnter()
        {
            base.OnEnter();
            base.characterDirection.forward = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, this.dashDuration + this.buffDuration);
            this.dashSpeed = this.baseDashSpeed * base.moveSpeedStat;
            if (base.skillLocator.secondary.stock < base.skillLocator.secondary.maxStock)
            {
                base.skillLocator.secondary.AddOneStock();
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
            {
                base.characterBody.characterDirection.moveVector = base.inputBank.moveVector;
                this.forwardDirection = base.characterDirection.forward;
                base.characterMotor.rootMotion += this.forwardDirection * Time.fixedDeltaTime * this.dashSpeed;
            }
            if (base.fixedAge >= this.dashDuration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
