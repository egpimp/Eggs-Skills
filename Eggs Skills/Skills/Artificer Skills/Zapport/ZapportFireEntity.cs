using EntityStates;
using UnityEngine;
using RoR2;
using EntityStates.VagrantMonster;
using UnityEngine.AddressableAssets;

namespace EggsSkills.EntityStates
{
    class ZapportFireEntity : BaseSkillState
    {
        //Force of the explosion
        private static readonly float baseForce = 100f;
        //Damage multiplier
        internal float damageMult;
        //Proc coefficient of the skill
        private static readonly float procCoef = 1f;
        //Radius of the explosion
        internal float radius;

        //Explosion fx
        private GameObject explosionPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Junk/Mage/MageLightningBombExplosion.prefab").WaitForCompletion();
        //Muzzle fx
        private GameObject muzzlePrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MuzzleflashMageLightningLarge.prefab").WaitForCompletion();

        //Strings for the hand 'muzzle' positions
        private static readonly string rMuzzleString = "MuzzleRight";
        private static readonly string lMuzzleString = "MuzzleLeft";
        private static readonly string soundString = "Play_vagrant_R_explode";

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
            Util.PlaySound(soundString, base.gameObject);
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
                    damageType = DamageType.Stun1s | DamageTypeCombo.GenericUtility,
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
