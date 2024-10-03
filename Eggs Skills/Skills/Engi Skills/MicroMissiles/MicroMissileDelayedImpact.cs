using RoR2.Projectile;
using UnityEngine;

namespace EggsSkills
{
    internal class MicroMissileDelayedImpact : MonoBehaviour
    {
        //Spp radius
        public static float spp_radiusMult = 1f;

        private ProjectileImpactExplosion explosion;

        private ProjectileTargetComponent homing;

        private Transform tracker;

        float delay = 0.2f;

        void Start()
        {
            explosion = GetComponent<ProjectileImpactExplosion>();
            homing = GetComponent<ProjectileTargetComponent>();
            tracker = homing.target;
            GetComponent<MissileController>().maxVelocity = Random.Range(20f, 25f);
            explosion.blastRadius = 5f * spp_radiusMult;
        }

        void FixedUpdate()
        {
            homing.target = tracker;
            if (homing.target == null || Vector3.Distance(homing.target.position, transform.position) < 3f)
            {
                Destroy(gameObject);
                explosion.Detonate();
            }
            if(delay > 0f) delay -= Time.deltaTime;
            else explosion.destroyOnEnemy = explosion.destroyOnWorld = true;
        }
    }
}
