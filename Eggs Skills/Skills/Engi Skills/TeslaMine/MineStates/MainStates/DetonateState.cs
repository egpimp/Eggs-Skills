using EntityStates.Engi.Mine;
using RoR2;
using UnityEngine;
using RoR2.Projectile;
using EntityStates.Huntress;
using EntityStates.JellyfishMonster;
using UnityEngine.Networking;

namespace EggsSkills.EntityStates.TeslaMine.MineStates.MainStates
{
    public class TeslaDetonateState : BaseMineState
    {
        public override bool shouldStick => true;
        public override bool shouldRevertToWaitForStickOnSurfaceLost => false;

        private float pulseCounter;
        private float pulseTimer;
        private float radius = 8f;

        private GameObject areaIndicator;
        private GameObject bodyPrefab = UnityEngine.Resources.Load<GameObject>("prefabs/effects/JellyfishNova");

        private ProjectileDamage projectileDamage;
        public override void OnEnter()
        {
            base.OnEnter();
            this.projectileDamage = GetComponent<ProjectileDamage>();
            this.areaIndicator = Object.Instantiate<GameObject>(ArrowRain.areaIndicatorPrefab);
            this.areaIndicator.SetActive(true);
            this.areaIndicator.transform.localScale = Vector3.one * this.radius;
            this.areaIndicator.transform.position = transform.position;
            this.pulseTimer = 0f;
            this.pulseCounter = 0;
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (this.pulseCounter >= 5)
            {
                this.Explode();
            }
            if (this.pulseTimer > 0)
            {
                this.pulseTimer -= Time.fixedDeltaTime;
            }
            else
            {
                this.pulseTimer = 1f;
                if (NetworkServer.active)
                {
                    this.Pulse();
                }
            }
            this.areaIndicator.transform.localScale = Vector3.one * this.radius * (1 - (pulseTimer/1));
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
            Util.PlaySound(JellyNova.novaSoundString, gameObject);
            this.pulseCounter += 1;
        }
        private void Explode()
        {
            if (NetworkServer.active)
            {
                Destroy(this.gameObject);
            }
            if (this.areaIndicator)
            {
                this.areaIndicator.SetActive(false);
                Destroy(this.areaIndicator);
            }
        }
    }
}
