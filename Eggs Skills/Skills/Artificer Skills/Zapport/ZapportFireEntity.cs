using EntityStates;
using UnityEngine;
using RoR2;
using EntityStates.VagrantMonster;

namespace EggsSkills.EntityStates
{
    class ZapportFireEntity : BaseSkillState
    {
        private float baseForce = 100f;
        internal float damageMult;
        internal float radius;

        private GameObject explosionPrefab = UnityEngine.Resources.Load<GameObject>("Prefabs/effects/MageLightningBombExplosion");
        private GameObject muzzlePrefab = UnityEngine.Resources.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");

        private string rMuzzleString = "MuzzleRight";
        private string lMuzzleString = "MuzzleLeft";

        internal Vector3 moveVec;
        public override void OnEnter()
        {
            base.OnEnter();
            base.characterMotor.velocity = Vector3.zero;
            base.PlayAnimation("Gesture, Additive", "FireWall");
            Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
            EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, lMuzzleString, false);
            EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, rMuzzleString, false);
            base.characterMotor.rootMotion += moveVec;
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public override void OnExit()
        {
            base.OnExit();
            EffectData endEffectData = new EffectData
            {
                scale = this.radius * 2f,
                origin = this.characterBody.corePosition
            };
            EffectManager.SpawnEffect(this.explosionPrefab, endEffectData, true);
            if (base.isAuthority)
            {
                new BlastAttack
                {
                    position = this.characterBody.corePosition,
                    baseDamage = base.damageStat * this.damageMult,
                    baseForce = this.baseForce * this.damageMult,
                    radius = radius,
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    teamIndex = base.teamComponent.teamIndex,
                    crit = RollCrit(),
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1,
                    falloffModel = BlastAttack.FalloffModel.None,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Stun1s,
                    attackerFiltering = AttackerFiltering.Default
                }.Fire();
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= 0.1f && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
    }
}
