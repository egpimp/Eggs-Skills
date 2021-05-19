using RoR2.Orbs;
using RoR2;
using UnityEngine;

namespace EggsSkills.Orbs
{
    class NanobotOrb : GenericDamageOrb
    {
        public override void Begin()
        {
            speed = Random.Range(500f,600f) / 10f;
            scale /= 6f;
            base.Begin();
        }
        public override void OnArrival()
        {
            base.OnArrival();
            HealthComponent health = attacker.GetComponent<HealthComponent>();
            if (target)
            {
                health.HealFraction(0.015f, default);
            }
        }
        public override GameObject GetOrbEffect()
        {
            return UnityEngine.Resources.Load<GameObject>("prefabs/effects/orbeffects/SquidOrbEffect");
        }
    }
}
