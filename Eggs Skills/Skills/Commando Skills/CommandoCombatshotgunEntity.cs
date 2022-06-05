using RoR2;
using EntityStates;
using UnityEngine;
using EntityStates.Commando.CommandoWeapon;
using RoR2.Skills;
using EggsSkills.Config;

namespace EggsSkills.EntityStates
{

    public class CombatShotgunEntity : BaseState, SteppedSkillDef.IStepSetter
    {
        //Skills++ modifiers
        public static float spp_procMod = 0f;
        public static uint spp_bulletMod = 0;

        //Cast time pre-attack speed
        private readonly float baseDuration = 0.4f;
        //Force
        private readonly float baseForce = 10f;
        //Recoil factor
        private readonly float baseRecoil = 0.6f;
        //Damage coefficient
        private readonly float damageCoefficient = 0.6f;
        //Cast time post-attack speed
        private float duration;
        //Max firing range
        private readonly float maxDist = 200f;
        //Proc coefficient
        private readonly float procCoefficient = 0.6f + spp_procMod;

        //Hit fx
        private GameObject hitEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/impacteffects/Hitspark1");
        //Muzzle fx
        private GameObject muzzleEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/muzzleflashes/Muzzleflash1");
        //Tracer fx
        private GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/TracerCommandoDefault");

        //Modifier for crit spread shrink
        private int critMod;
        //Helps determine which gun to fire from
        private int step;

        //How many bullet go pew
        private uint bulletCount = Configuration.GetConfigValue(Configuration.CommandoShotgunPellets) + spp_bulletMod;

        void SteppedSkillDef.IStepSetter.SetStep(int i)
        {
            //Handles determining step
            step = i;
        }
        public override void OnEnter()
        {
            //Standard enter procedure
            base.OnEnter();
            //Did we crit?
            bool isCrit;
            //Determine the cast-time
            duration = baseDuration / base.attackSpeedStat;
            //Muzzlestring establisher
            string muzzleString;
            //Determine if crit
            isCrit = base.RollCrit();
            //If we crit grab a 1, if we didn't grab a 0
            critMod = isCrit ? 1 : 0;
            //If 1, use right muzzle and anims
            if (step % 2 == 1)
            {
                muzzleString = "MuzzleRight";
                base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
            }
            //Otherwise, use left muzzle and anims
            else
            {
                muzzleString = "MuzzleLeft";
                base.PlayAnimation("Gesture Additive, Left", "FirePistol, Left");
            }
            //Fire bullet with given muzzle string and determined crit
            BulletFire(muzzleString, isCrit);
        }

        //Bullet firing
        private void BulletFire(string muzzleName, bool isCrit)
        {
            //Get aimray
            Ray aimRay = base.GetAimRay();
            //Set aim mode
            base.StartAimMode(aimRay, duration * 2f, false);
            //Network check
            if (base.isAuthority)
            {
                //Establish and fire bullet
                new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    //Min spread is 1 on non-crit, 0 on crit
                    minSpread = 1f - critMod,
                    //Max spread is 5 on non-crit, 3 on crit
                    maxSpread = 5f - (critMod * 2f),
                    bulletCount = bulletCount,
                    procCoefficient = procCoefficient,
                    damage = base.characterBody.damage * damageCoefficient,
                    force = baseForce,
                    muzzleName = muzzleName,
                    falloffModel = default,
                    tracerEffectPrefab = tracerEffectPrefab,
                    hitEffectPrefab = hitEffectPrefab,
                    isCrit = isCrit,
                    HitEffectNormal = false,
                    smartCollision = true,
                    maxDistance = maxDist,
                    damageType = DamageType.Generic,
                }.Fire();
            }
            //Execute muzzle flash
            EffectManager.SimpleMuzzleFlash(muzzleEffectPrefab, base.gameObject, muzzleName, false);
            //Play the sound
            Util.PlaySound(FireShotgun.attackSoundString, base.gameObject);
            //Apply recoil, shifted towards side the bullet shot from
            base.AddRecoil(-baseRecoil, baseRecoil, -2 * baseRecoil * (1 - (step % 2)), 2 * baseRecoil * (step % 2));
        }
        public override void FixedUpdate()
        {
            //Standard update procedure
            base.FixedUpdate();
            //If cast time over, and network check
            if (base.fixedAge >= duration && base.isAuthority)
            {
                //Set the next state
                outer.SetNextStateToMain();
                return;
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //Interrupted by anything
            return InterruptPriority.Skill;
        }
    }
}
