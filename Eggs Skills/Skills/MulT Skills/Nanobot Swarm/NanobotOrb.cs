using RoR2.Orbs;
using RoR2;
using UnityEngine;

namespace EggsSkills.Orbs
{
    class NanobotOrb : GenericDamageOrb
    {
        //Amount of health to heal
        private readonly float healthFraction = 0.015f;

        public override void Begin()
        {
            //Assign it a random speed from 50-60
            speed = Random.Range(500f,600f) / 10f;
            //Make it smaller
            scale /= 6f;
            //Then enter standard orb procedure
            base.Begin();
        }

        public override void OnArrival()
        {
            //Handle normal arrival shit like damage and destruction of the orb
            base.OnArrival();
            //Grab the owner's health component
            HealthComponent health = attacker.GetComponent<HealthComponent>();
            //If the target actually exists (so if orb hit something) heal the owner for the amount
            if (target) health.HealFraction(this.healthFraction, default);
        }

        public override GameObject GetOrbEffect()
        {
            //We use the squid orb effect for this, it looks cool and does what we want
            return LegacyResourcesAPI.Load<GameObject>("prefabs/effects/orbeffects/SquidOrbEffect");
        }
    }
}
