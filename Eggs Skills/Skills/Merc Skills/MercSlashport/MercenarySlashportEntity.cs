using EntityStates;
using RoR2;
using UnityEngine;
using System.Linq;
using EntityStates.Merc;
using EggsSkills.Config;

namespace EggsSkills.EntityStates
{
    class SlashportEntity : BaseState
    {
        //Skill damage coeffficient
        private readonly float damageCoefficient = 7f;
        //For handling cast time
        private float[] findMax;
        //Missing health% fraction
        private readonly float healthFraction = Configuration.GetConfigValue(Configuration.MercSlashHealthfraction);
        //Proc coeff of ability
        private readonly float procCoefficient = 1f;

        //Target hurtbox
        private HurtBox targetBox;

        //Tracker
        private MercSlashportTracker mercTracker;

        //String for slash fx to play
        private readonly string slashChildName = "GroundLight3Slash";

        public override void OnEnter()
        {            
            base.OnEnter();
            {
                //This keeps the duration being too low and messing shit up
                this.findMax = new float[] { 0.4f / base.attackSpeedStat, 0.1f };
                //Grab the tracker
                this.mercTracker = base.GetComponent<MercSlashportTracker>();
                //Get the target hitbox
                this.targetBox = this.mercTracker.trackingTarget;
                //If the target still exists
                if (this.targetBox.healthComponent.alive && targetBox)
                {
                    //Handle overlay shit
                    HandleOverlay();
                    //Setup fx data
                    EffectData effectData = new EffectData
                    {
                        rotation = Util.QuaternionSafeLookRotation(this.inputBank.aimDirection),
                        origin = base.characterBody.corePosition
                    };
                    //Play the blink fx
                    EffectManager.SpawnEffect(EvisDash.blinkPrefab, effectData, false);
                    //Play the sound
                    Util.PlaySound(EvisDash.beginSoundString, base.gameObject);
                    //Stop the movement during the cast
                    base.characterMotor.walkSpeedPenaltyCoefficient = 0f;
                    base.characterMotor.velocity = Vector3.zero;
                    //Pop the player in the air
                    base.SmallHop(base.characterMotor, 5f);
                    //Handle the calculations
                    HandleTeleport();
                    //Check network
                    if (base.isAuthority)
                    {
                        //Fire off first micro attack, no damage but applies expose and stuns them for the damage
                        new BlastAttack
                        {
                            position = this.targetBox.transform.position,
                            baseDamage = 0f,
                            baseForce = 0f,
                            radius = 0.5f,
                            attacker = base.gameObject,
                            inflictor = base.gameObject,
                            teamIndex = base.teamComponent.teamIndex,
                            crit = false,
                            procChainMask = default,
                            procCoefficient = 0f,
                            falloffModel = BlastAttack.FalloffModel.None,
                            damageColorIndex = default,
                            damageType = DamageType.ApplyMercExpose | DamageType.Stun1s,
                            attackerFiltering = default
                        }.Fire();
                    }
                }
            }
        }

        public override void OnExit()
        {
            //Play the animation
            base.PlayAnimation("Gesture, Additive", "GroundLight3", "GroundLight.playbackRate", findMax.Max());
            base.PlayAnimation("Gesture, Override", "GroundLight3", "GroundLight.playbackRate", findMax.Max());
            //If the target actually exists
            if (this.targetBox)
            {
                //Check network
                if (base.isAuthority)
                {
                    //Fire off blastattack centered on target (Cleaner than overlapattack)
                    new BlastAttack
                    {
                        position = this.targetBox.transform.position,
                        //Actual damage + % missing hp
                        baseDamage = base.damageStat * this.damageCoefficient + this.healthFraction * this.targetBox.healthComponent.missingCombinedHealth,
                        baseForce = 0,
                        radius = 0.5f,
                        attacker = base.gameObject,
                        inflictor = base.gameObject,
                        teamIndex = base.teamComponent.teamIndex,
                        crit = base.RollCrit(),
                        procChainMask = default,
                        procCoefficient = this.procCoefficient,
                        falloffModel = BlastAttack.FalloffModel.None,
                        damageColorIndex = default,
                        damageType = DamageType.Stun1s,
                        attackerFiltering = default
                    }.Fire();
                }
                //Fire off the swing fx
                EffectManager.SimpleMuzzleFlash(GroundLight.comboSwingEffectPrefab, base.gameObject, this.slashChildName, true);
                //Fire off sounds
                Util.PlaySound(GroundLight.finisherAttackSoundString, base.gameObject);
                Util.PlaySound(EvisDash.endSoundString, base.gameObject);
            }
            //Fix the walkspeed no matter what
            base.characterMotor.walkSpeedPenaltyCoefficient = 1;
            base.OnExit();
        }

        private void HandleTeleport()
        {
            //Get the distance to place merc at
            float collisionDistance = base.characterBody.radius + this.targetBox.collider.contactOffset;
            //Get the direction to teleport in
            Vector3 teleDir = EggsUtils.Helpers.Math.GetDirection(base.characterBody.corePosition, this.targetBox.transform.position);
            //Cut the Y component
            teleDir -= new Vector3(0, teleDir.y, 0);
            //Find the position to place the player at at; target position, - collision distance towards player original position
            Vector3 telePos = this.targetBox.transform.position + (-teleDir * collisionDistance);
            //Set the position
            base.characterMotor.Motor.SetPosition(telePos);
            //Set look direction towards enemy for effect
            base.characterDirection.forward = teleDir;
        }

        private void HandleOverlay()
        {
            //Setup the overlay
            TemporaryOverlay temporaryOverlay = base.GetModelTransform().gameObject.AddComponent<TemporaryOverlay>();
            //Set the duration
            temporaryOverlay.duration = 0.1f;
            //Animate the curve
            temporaryOverlay.animateShaderAlpha = true;
            temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
            //Destroy it on end
            temporaryOverlay.destroyComponentOnEnd = true;
            //Set the material
            temporaryOverlay.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
            //Add to the character model
            temporaryOverlay.AddToCharacerModel(base.GetModelTransform().GetComponent<CharacterModel>());
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //Check the duration and network
            if (base.fixedAge >= findMax.Max() && base.isAuthority)
            {
                //Set next state
                this.outer.SetNextStateToMain();
                return;
            };
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //Can only be interrupted if frozen
            return InterruptPriority.Frozen;
        }
    }
}
