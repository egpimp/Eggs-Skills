using RoR2;
using EntityStates;
using UnityEngine;
using System.Linq;
using EggsUtils.Buffs;
using EggsSkills.Config;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using EntityStates.Bison;
using EntityStates.VoidSurvivor.Weapon;

namespace EggsSkills.EntityStates
{
    class InversionChargeBase : BaseSkillState
    {
        private uint soundID;

        //Max duration of skill
        private static readonly float baseDuration = 1f;
        private float duration;

        //Sound string
        private static readonly string soundString = "Play_voidman_R_activate";

        //Muzzle flash
        protected GameObject muzzleFlash;
        private GameObject effectInstance;

        private string animation = "ChargeCrushCorruption";
        private string playbackRate = "CrushCorruption.playbackRate";

        protected EntityState nextState;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / base.attackSpeedStat;
            base.StartAimMode(duration);
            soundID = Util.PlayAttackSpeedSound(soundString, base.gameObject, base.attackSpeedStat);
            base.PlayAnimation("LeftArm, Override", animation, playbackRate, duration);
            Transform transform = base.FindModelChild("MuzzleHandBeam");
            if (transform && muzzleFlash)
            {
                effectInstance = Object.Instantiate(muzzleFlash, transform.position, transform.rotation);
                effectInstance.transform.parent = transform;
                ScaleParticleSystemDuration particleDuration = effectInstance.GetComponent<ScaleParticleSystemDuration>();
                ObjectScaleCurve curve = effectInstance.GetComponent<ObjectScaleCurve>();
                if (particleDuration) particleDuration.newDuration = duration;
                if (curve) curve.timeMax = duration;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if(isAuthority && fixedAge >= duration)
            {
                outer.SetNextState(nextState);
                return;
            }
        }

        public override void OnExit()
        {
            if (effectInstance) Destroy(effectInstance);
            AkSoundEngine.StopPlayingID(soundID);
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
