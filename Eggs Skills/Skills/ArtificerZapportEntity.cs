
using EntityStates;
using EntityStates.Huntress;
using RoR2;
using UnityEngine;
using System.Linq;
using EntityStates.VagrantMonster;

namespace EggsSkills.EntityStates
{
    class ZapportEntity : BaseSkillState
    {
        public float chargePercent;
        public float maxCharge;
        public float baseDistance = 8f;
        public float maxDistance;
        public Vector3 intendedMoveVec;
        public Vector3 maxMoveVec;
        public float baseDamageMult = 2.5f;
        public float damageMult;
        public Vector3 currentPos;
        public float hitDis;
        private RaycastHit raycastHit;
        public float realDis;
        public bool isCrit;
        public float baseRadius = 3;
        private GameObject areaIndicator;
        public float radius;
        private System.Random rng;
        private Vector3 randRelPos;
        private GameObject effectPrefab = Resources.Load<GameObject>("Prefabs/effects/LightningStakeNova");
        private bool reducerFlipFlop;
        private int randFreq;
        private GameObject explosionPrefab = Resources.Load<GameObject>("Prefabs/effects/MageLightningBombExplosion");
        private string rMuzzleString = "MuzzleRight";
        private string lMuzzleString = "MuzzleLeft";
        private GameObject muzzlePrefab = Resources.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");
        //private Animator animator;
        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority)
            {
                rng = new System.Random();
                var findMinSpeed = new float[] { base.attackSpeedStat, 4 };
                this.maxCharge = 2f/findMinSpeed.Min();
                isCrit = base.characterBody.RollCrit();
                base.PlayAnimation("Gesture, Additive", "PrepWall", "PrepWall.playbackRate", 1);
                areaIndicator = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
                Util.PlaySound(ChargeTrackingBomb.chargingSoundString, base.gameObject);
            }
        }
        public void IndicatorUpdator()
        {
            var aimDirection = base.GetAimRay().direction;
            currentPos = base.GetAimRay().origin;
            maxDistance = (1 + (4*chargePercent)) * baseDistance * (base.moveSpeedStat / 7);
            base.inputBank.GetAimRaycast(maxDistance + 10, out raycastHit);
            hitDis = raycastHit.distance;
            if (hitDis < maxDistance && hitDis > 0)
            {
                maxDistance = hitDis;
                realDis = maxDistance - 8;
            }
            else
            {
                realDis = maxDistance;
            }
            damageMult = baseDamageMult+(chargePercent*7.5f);
            radius = (baseRadius * damageMult + 20)/4;
            maxMoveVec = (realDis) * aimDirection + new Vector3(0, 2, 0);
            intendedMoveVec = maxMoveVec + currentPos;
            areaIndicator.transform.localScale = new Vector3(radius, radius, radius);
            areaIndicator.transform.localPosition = intendedMoveVec;
        }


        public override void OnExit()
        {
            base.characterMotor.walkSpeedPenaltyCoefficient = 1;
            EntityState.Destroy(areaIndicator.gameObject);
            base.PlayAnimation("Gesture, Additive", "FireWall");
            Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
            EffectData startEffectData = new EffectData
            {
                scale = radius * 1.5f,
                origin = currentPos
            };
            EffectManager.SpawnEffect(explosionPrefab, startEffectData,true);
            EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, lMuzzleString, false);
            EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, rMuzzleString, false);
            new BlastAttack
            {
                position = currentPos,
                baseDamage = base.damageStat * damageMult,
                baseForce = 30 * damageMult,
                radius = radius * 0.75f,
                attacker = base.gameObject,
                inflictor = base.gameObject,
                teamIndex = base.teamComponent.teamIndex,
                crit = isCrit,
                procChainMask = default(ProcChainMask),
                procCoefficient = 1,
                bonusForce = new Vector3(0, 0, 0),
                falloffModel = BlastAttack.FalloffModel.None,
                damageColorIndex = RoR2.DamageColorIndex.Default,
                damageType = DamageType.Stun1s,
                attackerFiltering = AttackerFiltering.Default
            }.Fire();
            base.characterMotor.Motor.SetPosition(intendedMoveVec);
            EffectData endEffectData = new EffectData
            {
                scale = radius * 2f,
                origin = intendedMoveVec
            };
            EffectManager.SpawnEffect(explosionPrefab, endEffectData, true);
            new BlastAttack
            {
                position = intendedMoveVec,
                baseDamage = base.damageStat * damageMult,
                baseForce = 40 * damageMult,
                radius = radius,
                attacker = base.gameObject,
                inflictor = base.gameObject,
                teamIndex = base.teamComponent.teamIndex,
                crit = isCrit,
                procChainMask = default(ProcChainMask),
                procCoefficient = 1,
                bonusForce = new Vector3(0, 0, 0),
                falloffModel = BlastAttack.FalloffModel.None,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageType.Stun1s,
                attackerFiltering = AttackerFiltering.Default
            }.Fire();
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge < maxCharge && base.IsKeyDownAuthority() && base.isAuthority)
            {
                chargePercent = base.fixedAge / maxCharge;
                this.randRelPos = new Vector3(rng.Next(-12, 13) / 4f, rng.Next(-12, 13) / 4f, rng.Next(-12, 13) / 4f);
                randFreq = rng.Next(1, 200) / 100;
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
                this.outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}