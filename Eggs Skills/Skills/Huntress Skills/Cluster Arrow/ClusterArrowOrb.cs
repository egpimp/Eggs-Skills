using RoR2.Orbs;
using UnityEngine;
using RoR2;
using UnityEngine.Networking;
using RoR2.Projectile;
using EggsSkills.Config;
using System;

namespace EggsSkills.Orbs
{
    public class HuntressBombArrowOrb : GenericDamageOrb
    {
        //Bomblet damage
        private readonly float bombletDamageCoef = 0.8f;
        //Explosion Damage
        private readonly float damageCoef = 5f;
        //Proc coefficient
        private readonly float procCoeff = 1f;
        //Explosion radius
        private readonly float radius = 12f;

        //Explosion effect
        private GameObject effectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXScavCannonImpactExplosion");
        
        //Number of bomblets
        private readonly int bombletCount = Configuration.GetConfigValue(Configuration.HuntressArrowBomblets);

        public override void Begin()
        {
            //Set arrow speed
            base.speed = 80f;
            //Fire arrow
            base.Begin();
        }
        public override void OnArrival()
        {
            //Explode first
            Explode();
            //Then release minibombs
            FireBomblets();
        }
        public override GameObject GetOrbEffect()
        {
            //The orb effect is basically just existing arrow orb
            return LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/ArrowOrbEffect");
        }
        private void FireBomblets()
        {
                //Repeat this for every bomblet needing to be fired; if it crits 1.5x it then floor that to get an int
            for (int i = 0; i < (!base.isCrit ? this.bombletCount : Math.Floor(this.bombletCount * 1.5f)); i++)
            {
                //Get a random upward angle, spread slightly increased on crit to spread bombs out better
                Quaternion angle = Quaternion.LookRotation((Vector3.up + new Vector3(UnityEngine.Random.Range(-50f, 50f) / 100f, UnityEngine.Random.Range(-50f, 50f) / 100f, UnityEngine.Random.Range(-50f, 50f) / 100f)) * (!base.isCrit ? 1f : 1.25f));
                //Grab the target's transform
                Transform transform = target.transform;
                //Then their position
                Vector3 pos = transform.position;
                //Fire the bomblet
                ProjectileManager.instance.FireProjectile(Resources.Projectiles.bombletPrefab, pos, angle, base.attacker, base.damageValue * this.bombletDamageCoef, 50f, base.isCrit);
            }
        }
        private void Explode()
        {
            //Make explosion
            new BlastAttack
            {
                baseDamage = damageValue * this.damageCoef,
                baseForce = 100f,
                attacker = attacker,
                inflictor = attacker,
                position = target.transform.position,
                radius = radius,
                teamIndex = teamIndex,
                procChainMask = default,
                procCoefficient = this.procCoeff,
                falloffModel = BlastAttack.FalloffModel.SweetSpot,
                damageColorIndex = default,
                crit = isCrit,
                losType = BlastAttack.LoSType.None,
            }.Fire();
            //Setup fx info
            EffectData effectData = new EffectData
            {
                color = Color.blue,
                origin = target.transform.position,
                scale = radius
            };
            //Spawn the fx
            EffectManager.SpawnEffect(effectPrefab, effectData,false);
        }
    }
}
