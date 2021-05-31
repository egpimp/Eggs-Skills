
using EntityStates;
using EntityStates.Huntress;
using RoR2;
using UnityEngine;
using System.Linq;
using EntityStates.VagrantMonster;
using EggsSkills.Config;

namespace EggsSkills.EntityStates
{
    class ZapportChargeEntity : BaseSkillState
    {
        private bool reducerFlipFlop;

        private float baseDamageMult = 2.5f;
        private float baseDistance = 8f;
        private float baseRadius = Configuration.GetConfigValue<float>(Configuration.MageZapportBaseradius);
        private float chargePercent;
        private float damageMult;
        private float hitDis;
        private float maxCharge;
        private float maxDistance;
        private float radius;

        private GameObject areaIndicator;
        private GameObject effectPrefab = UnityEngine.Resources.Load<GameObject>("Prefabs/effects/LightningStakeNova");

        private int baseMaxCharge = 2;
        private int randFreq;

        private RaycastHit raycastHit;

        private Vector3 maxMoveVec;
        private Vector3 randRelPos;
        public override void OnEnter()
        {
            base.OnEnter();
            float[] findMinSpeed = new float[] { base.attackSpeedStat, 4 };
            this.maxCharge = this.baseMaxCharge/findMinSpeed.Min();
            base.PlayAnimation("Gesture, Additive", "PrepWall", "PrepWall.playbackRate", 1);
            this.areaIndicator = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.areaIndicator.SetActive(true);
            Util.PlaySound(ChargeTrackingBomb.chargingSoundString, base.gameObject);
        }
        public void IndicatorUpdator()
        {
            Ray aimRay = GetAimRay();
            Vector3 aimDirection = aimRay.direction;
            aimRay.origin = characterBody.footPosition;
            this.maxDistance = (1 + (4 * this.chargePercent)) * this.baseDistance * (base.moveSpeedStat / 7);
            Physics.Raycast(aimRay.origin, aimRay.direction, out raycastHit, this.maxDistance);
            this.hitDis = raycastHit.distance;
            if (this.hitDis < this.maxDistance && this.hitDis > 0)
            {
                this.maxDistance = this.hitDis;
            }
            this.damageMult = this.baseDamageMult + 3 * (this.chargePercent * this.baseDamageMult);
            this.radius = (this.baseRadius * this.damageMult + 20) / 4;
            this.maxMoveVec = this.maxDistance * aimDirection;
            this.areaIndicator.transform.localScale = Vector3.one * this.radius;
            this.areaIndicator.transform.localPosition = aimRay.origin + this.maxMoveVec;
        }


        public override void OnExit()
        {
            base.characterMotor.walkSpeedPenaltyCoefficient = 1f;
            if (this.areaIndicator)
            {
                this.areaIndicator.SetActive(false);
                Destroy(areaIndicator);
            }
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            {
                if (base.fixedAge < maxCharge && base.IsKeyDownAuthority())
                {
                    this.chargePercent = base.fixedAge / this.maxCharge;
                    this.randRelPos = new Vector3(Random.Range(-12, 12) / 4f, Random.Range(-12, 12) / 4f, Random.Range(-12, 12) / 4f);
                    this.randFreq = Random.Range(1, this.baseMaxCharge * 100) / 100;
                    if (this.reducerFlipFlop == true)
                    {
                        if (this.randFreq <= this.chargePercent)
                        {
                            EffectData chargeEffectData = new EffectData
                            {
                                scale = 1f,
                                origin = base.characterBody.corePosition + this.randRelPos

                            };
                            EffectManager.SpawnEffect(effectPrefab, chargeEffectData, true);
                        }
                        this.reducerFlipFlop = false;
                    }
                    else
                    {
                        this.reducerFlipFlop = true;
                    }
                    base.characterMotor.walkSpeedPenaltyCoefficient = 1 - (this.chargePercent / 3);
                    IndicatorUpdator();
                }
                else if (base.isAuthority)
                {
                    ZapportFireEntity nextState = new ZapportFireEntity();
                    nextState.damageMult = damageMult;
                    nextState.radius = radius;
                    nextState.moveVec = maxMoveVec;
                    this.outer.SetNextState(nextState);
                    return;
                }
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}