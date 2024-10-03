using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates.Toolbot;
using EntityStates.GolemMonster;
using EggsUtils.Helpers;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;

namespace EggsSkills.EntityStates
{
    class NanobotEntity : BaseSkillState
    {
        //Muzzle locator for MUL-T
        private ChildLocator muzzleLocator;

        //Base duration for firing, pre attack speed
        private static readonly float baseDuration = 0.5f;
        //Duration post-attack speed
        private float duration;
        //Maximum laser-sight distance
        private static readonly float maxDist = 1000f;

        //Prefab for laser sight
        private GameObject lineFXPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Golem/LaserGolem.prefab").WaitForCompletion();
        //FX for the laser sight
        private GameObject lineEffect;

        //Handles the generation of the line
        private LineRenderer lineComponent;

        //Aimray for aiming
        private Ray aimRay;

        //String for the mumzzle it's fired from
        private static readonly string muzzleString = "MuzzleNailgun";
        //Sound strings
        private static readonly string startSoundString = "Play_MULT_m2_aim";
        private static readonly string endSoundString = "Play_MULT_m2_throw";

        //Transform for the muzzle (Helps locate position)
        private Transform muzzleTransform;

        public override void OnEnter()
        {
            //Do base enter stuff
            base.OnEnter();
            //Get the aimray
            aimRay = base.GetAimRay();
            //Locate the muzzle
            muzzleLocator = base.modelLocator.modelTransform.GetComponent<ChildLocator>();
            //Find the exact muzzle transform we need
            muzzleTransform = muzzleLocator.FindChild(muzzleString);
            //Generate the laser effect for our use
            lineEffect = Object.Instantiate(lineFXPrefab);
            //Set it as active
            lineEffect.SetActive(true);
            //Grab the linerenderer component
            lineComponent = lineEffect.GetComponent<LineRenderer>();
            //Replace the origin for the aimray
            aimRay.origin = muzzleTransform.position;
            //Set start and end width
            lineComponent.startWidth = 0.1f;
            lineComponent.endWidth = 0.1f;
            //Find the duration with AS and baseduration
            duration = baseDuration / base.attackSpeedStat;
            //Play the animation for aiming
            base.PlayAnimation("Gesture, Additive", "PrepBomb", "PrepBomb.playbackRate", duration);
            base.PlayAnimation("Stance, Override", "PutAwayGun");
            //Play the sound for aiming
            Util.PlaySound(startSoundString, base.gameObject);
        }
        public override void OnExit()
        {
            //Standard exit procedure
            base.OnExit();
            //Fire
            if (base.isAuthority) Fire();
            //If the fx still exists, unexist it
            if(lineEffect)
            {
                lineEffect.SetActive(false);
                Destroy(lineEffect);
            }
        }
        public override void FixedUpdate()
        {
            //Standard fixedupdate procedure
            base.FixedUpdate();
            base.StartAimMode();
            UpdateFx();
            //If the button is no longer held down and minimum time has passed
            if (!base.IsKeyDownAuthority() && base.fixedAge >= duration && base.isAuthority)
            {
                //fix the state
                outer.SetNextStateToMain();
                return;
            }
        }

        private void UpdateFx()
        {
            //Otherwise keep updating the aimray
            aimRay = base.GetAimRay();
            //Grab the hit component
            RaycastHit hit;
            //Try to find the hit position, otherwise get the point at the max distance
            Vector3 origHitPos = base.inputBank.GetAimRaycast(maxDist, out hit) ? hit.point : aimRay.GetPoint(maxDist);
            //Set the aimray origin to the muzzle position
            aimRay.origin = muzzleTransform.position;
            //Set the parent of the line effect to the muzzle
            lineEffect.transform.parent = muzzleTransform;
            //Get the direction from the muzzle to the position hit by the aimray
            aimRay.direction = Math.GetDirection(aimRay.origin, origHitPos);
            //This gets us a new hitposition based off of the aimray coming from the muzzle
            Vector3 newHitPos = Physics.Raycast(aimRay.origin, aimRay.direction, out hit, maxDist) ? hit.point : aimRay.GetPoint(maxDist);
            //Set the positions of the line component based off the muzzle position and the determine hit position
            lineComponent.SetPositions(new Vector3[] { aimRay.origin, newHitPos });
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //Interrupted by any more important skills
            return InterruptPriority.PrioritySkill;
        }
        private void Fire()
        {
            //Network check, then fire the projectile
            ProjectileManager.instance.FireProjectile(Resources.Projectiles.nanoBeaconPrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, base.damageStat, 50f, base.RollCrit());
            //Handle the fire animations
            base.PlayAnimation("Gesture, Additive", "FireBomb", "FireBomb.playbackRate", duration);
            base.PlayCrossfade("Stance, Override", "Empty", 0.1f);
            //Execute the fire sound
            Util.PlaySound(endSoundString, base.gameObject);
        }
    }
}
