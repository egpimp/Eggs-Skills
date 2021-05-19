using EntityStates;
using RoR2;
using UnityEngine;
using System.Linq;
using EntityStates.Merc;

namespace EggsSkills.EntityStates
{
    class SlashportEntity : BaseState
    {
        private new InputBankTest inputBank;
        private HurtBox targetBox;
        private readonly string slashChildName = "GroundLight3Slash";
        private readonly GameObject swingEffectPrefab = UnityEngine.Resources.Load<GameObject>("Prefabs/effects/MercSwordSlashWhirlwind");
        private Vector3 telePos;
        private Vector3 lookDir;
        private Vector3 teleDir;
        private float collisionDistance;
        private float[] findMax;
        private MercSlashportTracker mercTracker;
        public override void OnEnter()
        {            
            base.OnEnter();
            if (base.isAuthority)
            {
                this.findMax = new float[] { 0.4f / base.attackSpeedStat, 0.1f };
                this.mercTracker = base.GetComponent<MercSlashportTracker>();
                this.targetBox = this.mercTracker.trackingTarget;
                this.inputBank = base.GetComponent<InputBankTest>();
                if (this.targetBox.healthComponent.alive && targetBox)
                {
                    TemporaryOverlay temporaryOverlay = base.GetModelTransform().gameObject.AddComponent<TemporaryOverlay>();
                    temporaryOverlay.duration = 0.1f;
                    temporaryOverlay.animateShaderAlpha = true;
                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlay.destroyComponentOnEnd = true;
                    temporaryOverlay.originalMaterial = UnityEngine.Resources.Load<Material>("Materials/matHuntressFlashBright");
                    temporaryOverlay.AddToCharacerModel(base.GetModelTransform().GetComponent<CharacterModel>());
                    base.PlayAnimation("FullBody, Override", "EvisPrep", "EvisPrep.playbackRate", findMax.Max());
                    EffectData effectData = new EffectData
                    {
                        rotation = Util.QuaternionSafeLookRotation(this.inputBank.aimDirection),
                        origin = base.characterBody.corePosition
                    };
                    EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
                    Util.PlaySound(EvisDash.beginSoundString, base.gameObject);
                    base.characterMotor.walkSpeedPenaltyCoefficient = 0f;
                    base.characterMotor.velocity = new Vector3(0, 10, 0);
                    this.collisionDistance = base.characterBody.radius + this.targetBox.collider.contactOffset;
                    this.teleDir = (base.characterBody.corePosition - this.targetBox.transform.position).normalized;
                    this.teleDir -= new Vector3(0, this.teleDir.y, 0);
                    this.telePos = this.targetBox.transform.position + (this.teleDir * this.collisionDistance);
                    base.characterMotor.Motor.SetPosition(this.telePos);
                    this.lookDir = (telePos - this.targetBox.transform.position).normalized;
                    base.characterDirection.forward = lookDir * -1;
                    new BlastAttack
                    {
                        position = this.targetBox.transform.position,
                        baseDamage = 0,
                        baseForce = 0,
                        radius = 0.5f,
                        attacker = base.gameObject,
                        inflictor = base.gameObject,
                        teamIndex = base.teamComponent.teamIndex,
                        crit = false,
                        procChainMask = default,
                        procCoefficient = 0,
                        bonusForce = new Vector3(0, 0, 0),
                        falloffModel = BlastAttack.FalloffModel.None,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.ApplyMercExpose,
                        attackerFiltering = AttackerFiltering.Default
                    }.Fire();
                }
            }
        }
        public override void OnExit()
        {
            if (this.targetBox)
            {
                new BlastAttack
                {
                    position = this.targetBox.transform.position,
                    baseDamage = base.damageStat * 7f + 0.20f * targetBox.healthComponent.missingCombinedHealth,
                    baseForce = 0,
                    radius = 0.5f,
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    teamIndex = base.teamComponent.teamIndex,
                    crit = base.RollCrit(),
                    procChainMask = default,
                    procCoefficient = 1,
                    bonusForce = new Vector3(0, 0, 0),
                    falloffModel = BlastAttack.FalloffModel.None,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Stun1s,
                    attackerFiltering = AttackerFiltering.Default
                }.Fire();
                EffectManager.SimpleMuzzleFlash(this.swingEffectPrefab, base.gameObject, this.slashChildName, false);
                base.PlayAnimation("FullBody, Override", "GroundLight3", "GroundLight.playbackRate", 1f);
                Util.PlaySound(GroundLight.finisherAttackSoundString, base.gameObject);
                Util.PlaySound(EvisDash.endSoundString, base.gameObject);
            }
            base.characterMotor.walkSpeedPenaltyCoefficient = 1;
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= findMax.Max())
            {
                this.outer.SetNextStateToMain();
                return;
            };
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}
