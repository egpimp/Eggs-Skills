
using EntityStates;
using EntityStates.Huntress;
using RoR2;
using UnityEngine;
using System.Linq;
using EntityStates.VagrantMonster;

namespace EggsSkills.EntityStates
{
    class ZapportChargeEntity : BaseSkillState
    {
        private float chargePercent;
        private float maxCharge;
        private float baseDistance = 8f;
        private float maxDistance;
        private Vector3 maxMoveVec;
        private float baseDamageMult = 2.5f;
        private float damageMult;
        private float hitDis;
        private RaycastHit raycastHit;
        private float baseRadius = 3;
        private GameObject areaIndicator;
        private float radius;
        private Vector3 randRelPos;
        private GameObject effectPrefab = UnityEngine.Resources.Load<GameObject>("Prefabs/effects/LightningStakeNova");
        private bool reducerFlipFlop;
        private int randFreq;
        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority)
            {
                var findMinSpeed = new float[] { base.attackSpeedStat, 4 };
                this.maxCharge = 2f/findMinSpeed.Min();
                base.PlayAnimation("Gesture, Additive", "PrepWall", "PrepWall.playbackRate", 1);
                areaIndicator = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
                areaIndicator.SetActive(true);
                Util.PlaySound(ChargeTrackingBomb.chargingSoundString, base.gameObject);
            }
        }
        public void IndicatorUpdator()
        {
            var aimRay = GetAimRay();
            var aimDirection = aimRay.direction;
            var oldOrigin = aimRay.origin;
            aimRay.origin = characterBody.footPosition;
            maxDistance = (1 + (4*chargePercent)) * baseDistance * (base.moveSpeedStat / 7);
            base.inputBank.GetAimRaycast(maxDistance, out raycastHit);
            hitDis = raycastHit.distance;
            if (hitDis < maxDistance && hitDis > 0)
            {
                maxDistance = hitDis;
            }
            damageMult = baseDamageMult+(chargePercent*7.5f);
            radius = (baseRadius * damageMult + 20)/4;
            maxMoveVec = maxDistance * aimDirection;
            areaIndicator.transform.localScale = new Vector3(radius, radius, radius);
            areaIndicator.transform.localPosition = aimRay.origin + maxMoveVec;
            aimRay.origin = oldOrigin;
        }


        public override void OnExit()
        {
            base.characterMotor.walkSpeedPenaltyCoefficient = 1;
            areaIndicator.SetActive(false);
            EntityState.Destroy(areaIndicator);
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge < maxCharge && base.IsKeyDownAuthority() && base.isAuthority)
            {
                chargePercent = base.fixedAge / maxCharge;
                this.randRelPos = new Vector3(Random.Range(-12, 12) / 4f, Random.Range(-12, 12) / 4f, Random.Range(-12, 12) / 4f);
                randFreq = Random.Range(1, 200) / 100;
                if (reducerFlipFlop == true)
                {
                    if (randFreq <= chargePercent)
                    {
                        EffectData chargeEffectData = new EffectData
                        {
                            scale = 1,
                            origin = base.characterBody.corePosition + randRelPos

                        };
                        EffectManager.SpawnEffect(effectPrefab, chargeEffectData, true);
                    }
                    reducerFlipFlop = false;
                }
                else
                {
                    reducerFlipFlop = true;
                }
                base.characterMotor.walkSpeedPenaltyCoefficient = 1 - (chargePercent / 3) ;
                IndicatorUpdator();
            }
            else
            {
                ZapportFireEntity nextState = new ZapportFireEntity();
                nextState.damageMult = damageMult;
                nextState.radius = radius;
                nextState.moveVec = maxMoveVec;
                this.outer.SetNextState(nextState);
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}