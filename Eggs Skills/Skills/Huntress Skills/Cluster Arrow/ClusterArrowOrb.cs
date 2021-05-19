﻿using RoR2.Orbs;
using UnityEngine;
using RoR2;
using RoR2.Projectile;
namespace EggsSkills.Orbs
{
    public class HuntressBombArrowOrb : GenericDamageOrb
    {
        private float radius = 12f;
        private GameObject effectPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFXScavCannonImpactExplosion");
        private float critMod;
        public override void Begin()
        {
            speed = 80f;
            if(isCrit)
            {
                critMod = 1f;
            }
            else
            {
                critMod = 0f;
            }
            base.Begin();
        }
        public override void OnArrival()
        {
            Explode();
            FireBomblets();
        }
        public override GameObject GetOrbEffect()
        {
            return UnityEngine.Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/ArrowOrbEffect");
        }
        private void FireBomblets()
        {
            for (int i = 0; i < 8 + (4 * critMod); i += 1)
            {             
                Quaternion angle = Quaternion.LookRotation((Vector3.up + new Vector3(UnityEngine.Random.Range(-5f,5f)/10f,UnityEngine.Random.Range(-5f,5f)/10f,UnityEngine.Random.Range(-5f,5f)/10f)) * (1 + critMod / 4));
                var transform = target.transform;
                var pos = transform.position;
                ProjectileManager.instance.FireProjectile(EggsSkills.Resources.Projectiles.bombletPrefab, new Vector3(pos.x, pos.y, pos.z), angle, attacker, damageValue * 0.8f, 50f, isCrit);
            }
        }
        private void Explode()
        {
            new BlastAttack
            {
                baseDamage = damageValue * 5f,
                baseForce = 100f,
                attacker = attacker,
                inflictor = attacker,
                position = target.transform.position,
                radius = radius,
                teamIndex = teamIndex,
                procChainMask = default,
                procCoefficient = 1f,
                falloffModel = BlastAttack.FalloffModel.SweetSpot,
                damageColorIndex = default,
                crit = base.isCrit,
                damageType = DamageType.Stun1s,
                losType = BlastAttack.LoSType.None,
            }.Fire();
            EffectData effectData = new EffectData
            {
                color = Color.blue,
                origin = target.transform.position,
                scale = radius
            };
            EffectManager.SpawnEffect(effectPrefab, effectData,false);
        }
    }
}