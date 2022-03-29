using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates.Toolbot;
using EntityStates.GolemMonster;
using EggsUtils.Helpers;
using UnityEngine.Networking;

namespace EggsSkills.EntityStates
{
    class NanobotEntity : BaseSkillState
    {
        //Muzzle locator for MUL-T
        private ChildLocator muzzleLocator;

        //Base duration for firing, pre attack speed
        private readonly float baseDuration = 0.5f;
        //Duration post-attack speed
        private float duration;
        //Maximum laser-sight distance
        private readonly float maxDist = 1000f;

        //FX for the laser sight
        private GameObject lineEffect;

        //Handles the generation of the line
        private LineRenderer lineComponent;

        //Aimray for aiming
        private Ray aimRay;

        //String for the mumzzle it's fired from
        private string muzzleString = "MuzzleNailgun";

        //Transform for the muzzle (Helps locate position)
        private Transform muzzleTransform;

        public override void OnEnter()
        {
            //Do base enter stuff
            base.OnEnter();
            //Get the aimray
            this.aimRay = base.GetAimRay();
            //Locate the muzzle
            this.muzzleLocator = base.modelLocator.modelTransform.GetComponent<ChildLocator>();
            //Find the exact muzzle transform we need
            this.muzzleTransform = this.muzzleLocator.FindChild(this.muzzleString);
            //Generate the laser effect for our use
            this.lineEffect = Object.Instantiate(ChargeLaser.laserPrefab);
            //Set it as active
            this.lineEffect.SetActive(true);
            //Grab the linerenderer component
            this.lineComponent = this.lineEffect.GetComponent<LineRenderer>();
            //Replace the origin for the aimray
            this.aimRay.origin = this.muzzleTransform.position;
            //Set start and end width
            this.lineComponent.startWidth = 0.1f;
            this.lineComponent.endWidth = 0.1f;
            //Find the duration with AS and baseduration
            this.duration = this.baseDuration / base.attackSpeedStat;
            //Play the animation for aiming
            base.PlayAnimation("Gesture, Additive", "PrepBomb", "PrepBomb.playbackRate", this.duration);
            base.PlayAnimation("Stance, Override", "PutAwayGun");
            //Play the sound for aiming
            Util.PlaySound(AimStunDrone.enterSoundString, base.gameObject);
        }
        public override void OnExit()
        {
            //Standard exit procedure
            base.OnExit();
            //Fire
            if (base.isAuthority) Fire();
            //If the fx still exists, unexist it
            if(this.lineEffect)
            {
                this.lineEffect.SetActive(false);
                Destroy(this.lineEffect);
            }
        }
        public override void FixedUpdate()
        {
            //Standard fixedupdate procedure
            base.FixedUpdate();
            base.StartAimMode();
            this.UpdateFx();
            //If the button is no longer held down and minimum time has passed
            if (!base.IsKeyDownAuthority() && base.fixedAge >= this.duration && base.isAuthority)
            {
                //fix the state
                this.outer.SetNextStateToMain();
                return;
            }
        }

        private void UpdateFx()
        {
            //Otherwise keep updating the aimray
            this.aimRay = base.GetAimRay();
            //Grab the hit component
            RaycastHit hit;
            //Try to find the hit position, otherwise get the point at the max distance
            Vector3 origHitPos = base.inputBank.GetAimRaycast(this.maxDist, out hit) ? hit.point : this.aimRay.GetPoint(this.maxDist);
            //Set the aimray origin to the muzzle position
            this.aimRay.origin = this.muzzleTransform.position;
            //Set the parent of the line effect to the muzzle
            this.lineEffect.transform.parent = this.muzzleTransform;
            //Get the direction from the muzzle to the position hit by the aimray
            this.aimRay.direction = Math.GetDirection(this.aimRay.origin, origHitPos);
            //This gets us a new hitposition based off of the aimray coming from the muzzle
            Vector3 newHitPos = Physics.Raycast(this.aimRay.origin, this.aimRay.direction, out hit, this.maxDist) ? hit.point : this.aimRay.GetPoint(this.maxDist);
            //Set the positions of the line component based off the muzzle position and the determine hit position
            this.lineComponent.SetPositions(new Vector3[] { this.aimRay.origin, newHitPos });
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            //Interrupted by any more important skills
            return InterruptPriority.PrioritySkill;
        }
        private void Fire()
        {
            //Network check, then fire the projectile
            ProjectileManager.instance.FireProjectile(Resources.Projectiles.nanoBeaconPrefab, this.aimRay.origin, Util.QuaternionSafeLookRotation(this.aimRay.direction), base.gameObject, base.damageStat, 50f, base.RollCrit());
            //Handle the fire animations
            base.PlayAnimation("Gesture, Additive", "FireBomb", "FireBomb.playbackRate", duration);
            base.PlayCrossfade("Stance, Override", "Empty", 0.1f);
            //Execute the fire sound
            Util.PlaySound(AimStunDrone.exitSoundString, base.gameObject);
        }
    }
}
