using UnityEngine;
using RoR2.Projectile;
using EntityStates;
using EntityStates.Huntress;
using UnityEngine.Networking;

namespace EggsSkills
{
    public class OnStuckCaller : MonoBehaviour
    {
        private bool monoTrigger;
        private bool monoTrigger2;

        private float baseCountDown = 2f;
        private float countDown;
        private float radiusMax = 20f;

        private GameObject areaIndicator;
        private GameObject owner;

        private ProjectileController controller;
        private ProjectileStickOnImpact stick;
        private void Start()
        {
            this.monoTrigger = true;
            this.monoTrigger2 = true;
            this.countDown = this.baseCountDown;
            this.areaIndicator = Object.Instantiate(ArrowRain.areaIndicatorPrefab);
            this.owner = GetComponent<ProjectileController>().owner;
            this.stick = GetComponent<ProjectileStickOnImpact>();
            this.controller = GetComponent<ProjectileController>();
            this.areaIndicator.transform.localScale = Vector3.zero;
        }
        private void FixedUpdate()
        {
            if(stick.stuck)
            {
                if (monoTrigger)
                {
                    this.areaIndicator.SetActive(true);
                    this.monoTrigger = false;
                }
            }
            if(this.monoTrigger2 && !this.monoTrigger)
            {
                if (this.countDown > 0)
                {
                    this.countDown -= Time.fixedDeltaTime;
                    this.areaIndicator.transform.localScale = Vector3.one * this.radiusMax * (1 - (this.countDown / this.baseCountDown));
                    this.areaIndicator.transform.position = this.controller.transform.position;
                }
                else
                {
                
                    this.owner.GetComponent<SwarmComponent>()?.GetTargets(this.controller.transform.position);
                    this.monoTrigger2 = false;
                    if (this.areaIndicator)
                    {
                        this.areaIndicator.SetActive(false);
                        Destroy(this.areaIndicator);
                    }
                }
            }
        }
    }
}
