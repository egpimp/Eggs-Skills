using EntityStates.Engi.Mine;
using RoR2;
using UnityEngine;
using RoR2.Projectile;
using EntityStates.JellyfishMonster;
using UnityEngine.Networking;
using EggsSkills.Config;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.MainStates
{
    public class TeslaDetonateState : BaseMineState
    {
        //Skill++
        public static float spp_damageMult = 1f;
        public static float spp_radiusMult = 1f;
        public static int spp_pulseBonus = 0;

        //Should keep sticking at this point
        public override bool shouldStick => true;
        //And still should never revert
        public override bool shouldRevertToWaitForStickOnSurfaceLost => false;

        //Time between pulses
        private readonly float fixedPulseTime = 1f;
        //Proc coefficient
        private readonly float procCoeff = 1f;
        //timer for handling pulses
        private float pulseTimer;
        //Ouchies radius
        private readonly float radius = 8f * spp_radiusMult;

        //Prefab for the explode fx
        private GameObject bodyPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/JellyfishNova");

        //Max amount of electric pulses
        private int maxPulseCount = Configuration.GetConfigValue(Configuration.EngiTeslaminePulses) + spp_pulseBonus;
        //Counts pulses
        private int pulseCounter;

        //Projectile damage component
        private ProjectileDamage projectileDamage;

        public override void OnEnter()
        {
            base.OnEnter();
            //Get the component
            projectileDamage = GetComponent<ProjectileDamage>();
            //Start timer at 0 so it pops once at start
            pulseTimer = 0f;
            //It has yet to pulse
            pulseCounter = 0;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            //If it has pulsed max amount of times, kill the mine
            if (pulseCounter >= maxPulseCount) Explode();
            //If the timer is still above zero count it down
            if (pulseTimer > 0) pulseTimer -= Time.fixedDeltaTime;
            //Otherwise...
            else
            {
                //Reset the pulse timer
                pulseTimer = fixedPulseTime;
                //Fire off a pulse
                Pulse();
            }
        }



        private void Pulse()
        {
            //Network check, then prep and fire blast attack
            if (NetworkServer.active)
            {
                new BlastAttack()
                {
                    attacker = base.projectileController.owner,
                    inflictor = base.gameObject,
                    procCoefficient = procCoeff,
                    teamIndex = base.projectileController.teamFilter.teamIndex,
                    baseDamage = projectileDamage.damage * spp_damageMult,
                    baseForce = 0f,
                    falloffModel = BlastAttack.FalloffModel.None,
                    crit = projectileDamage.crit,
                    radius = radius,
                    position = base.transform.position,
                    damageType = DamageType.Stun1s
                }.Fire();
            }
            //Setup fx data
            EffectData effectData = new EffectData
            {
                origin = base.transform.position,
                color = Color.blue,
                scale = radius
            };
            //Spawn the vfx
            EffectManager.SpawnEffect(bodyPrefab, effectData, true);
            //Play the zappy sound
            Util.PlaySound(JellyNova.novaSoundString, gameObject);
            //Track that as a pulse
            pulseCounter += 1;
        }
        private void Explode()
        {
            //YOU HAVE BEEN DESTROYED
            Destroy(base.gameObject);
        }
    }
}
