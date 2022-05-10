using EntityStates;
using UnityEngine;
using RoR2;
using EntityStates.VagrantMonster;

namespace EggsSkills.EntityStates
{
    class ZapportFireEntity : BaseSkillState
    {
        //Force of the explosion
        private readonly float baseForce = 100f;
        //Damage multiplier
        internal float damageMult;
        //Proc coefficient of the skill
        private readonly float procCoef = 1f;
        //Radius of the explosion
        internal float radius;

        //Explosion fx
        private GameObject explosionPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/MageLightningBombExplosion");
        //Muzzle fx
        private GameObject muzzlePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/effects/muzzleflashes/MuzzleflashMageLightningLarge");

        //Strings for the hand 'muzzle' positions
        private readonly string rMuzzleString = "MuzzleRight";
        private readonly string lMuzzleString = "MuzzleLeft";

        //Where the artificer go
        internal Vector3 movePos;

        public override void OnEnter()
        {
            //Base enter
            base.OnEnter();
            //Cancel all velocity
            base.characterMotor.velocity = Vector3.zero;
            //Play the animation
            base.PlayAnimation("Gesture, Additive", "FireWall");
            //Play the splodey sound
            Util.PlaySound(FireMegaNova.novaSoundString, base.gameObject);
            //Play both muzzle flashes
            EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, lMuzzleString, false);
            EffectManager.SimpleMuzzleFlash(muzzlePrefab, base.gameObject, rMuzzleString, false);
            //Set the position
            base.characterMotor.Motor.SetPosition(movePos);
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //This can only be interrupted if frozen or worse
            return InterruptPriority.Frozen;
        }

        public override void OnExit()
        {
            //Base exit
            base.OnExit();
            //Create fx data
            EffectData endEffectData = new EffectData
            {
                scale = radius,
                origin = characterBody.corePosition
            };
            //Spawn the fx
            EffectManager.SpawnEffect(explosionPrefab, endEffectData, true);
            //Network check
            if (base.isAuthority)
            {
                //Create and fire blast attack
                new BlastAttack
                {
                    position = characterBody.corePosition,
                    baseDamage = base.damageStat * damageMult,
                    baseForce = baseForce * damageMult,
                    radius = radius,
                    attacker = base.gameObject,
                    inflictor = base.gameObject,
                    teamIndex = base.teamComponent.teamIndex,
                    crit = base.RollCrit(),
                    procCoefficient = procCoef,
                    falloffModel = BlastAttack.FalloffModel.None,
                    damageType = DamageType.Stun1s,
                }.Fire();
            }
        }
        public override void FixedUpdate()
        {
            //Base fixedupdate
            base.FixedUpdate();
            //Set next state after short time
            if (base.fixedAge >= 0.1f && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }
    }
}
