using EntityStates.Captain.Weapon;
using EntityStates.Engi.Mine;
using RoR2;
using System.Linq.Expressions;
using UnityEngine;
using RoR2.Projectile;
using EntityStates.Huntress;
using EntityStates;
using EntityStates.JellyfishMonster;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.MainStates
{
    public class TeslaDetonateState : BaseMineState
    {
        protected override bool shouldStick => true;
        protected override bool shouldRevertToWaitForStickOnSurfaceLost => false;
        public float pulseTimer;
        private GameObject bodyPrefab = Resources.Load<GameObject>("prefabs/effects/JellyfishNova");
        private GameObject areaIndicator;
        private float radius = 8f;
        private ProjectileDamage projectileDamage;
        private float pulseCounter;
        public override void OnEnter()
        {
            base.OnEnter();
            projectileDamage = GetComponent<ProjectileDamage>();
            areaIndicator = UnityEngine.Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            areaIndicator.SetActive(true);
            areaIndicator.transform.localScale = new Vector3(radius, radius, radius);
            areaIndicator.transform.position = transform.position;
            pulseTimer = 0f;
            pulseCounter = 0;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (pulseCounter >= 5)
            {
                EntityState.Destroy(areaIndicator);
                Explode();
            }
            if (this.pulseTimer > 0)
            {
                this.pulseTimer -= Time.fixedDeltaTime;
            }
            else
            {
                this.pulseTimer = 1f;
                this.Pulse();
            }
            areaIndicator.transform.localScale = new Vector3(radius,radius,radius) * (1 - (pulseTimer/1));
        }
        public void Pulse()
        {
            new BlastAttack()
            {
                attacker = projectileController.owner,
                inflictor = gameObject,
                procChainMask = default,
                procCoefficient = 1f,
                teamIndex = projectileController.teamFilter.teamIndex,
                baseDamage = projectileDamage.damage,
                baseForce = 0f,
                falloffModel = BlastAttack.FalloffModel.None,
                crit = projectileDamage.crit,
                radius = radius,
                position = transform.position,
                damageColorIndex = default,
                attackerFiltering = AttackerFiltering.Default,
                damageType = DamageType.Stun1s
            }.Fire();
            EffectData effectData = new EffectData
            {
                origin = transform.position,
                color = Color.blue,
                scale = radius
            };
            EffectManager.SpawnEffect(bodyPrefab, effectData, true);
            Util.PlaySound(JellyNova.novaSoundString,gameObject);
            pulseCounter += 1;
        }
        private void Explode()
        {
            Destroy(gameObject);
        }
    }
}
