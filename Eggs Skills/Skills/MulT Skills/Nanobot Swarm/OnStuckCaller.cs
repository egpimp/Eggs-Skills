using UnityEngine;
using RoR2.Projectile;
using EntityStates;
using EntityStates.Huntress;

namespace EggsSkills
{
    public class OnStuckCaller : MonoBehaviour
    {
        private bool monoTrigger;
        private bool monoTrigger2;
        private float countDown;
        private GameObject areaIndicator;
        private GameObject owner;
        private ProjectileStickOnImpact stick;
        private ProjectileController controller;
        private float radiusMax = 20;
        private void Start()
        {
            monoTrigger = true;
            monoTrigger2 = true;
            countDown = 2f;
            areaIndicator = Object.Instantiate(ArrowRain.areaIndicatorPrefab);
            owner = GetComponent<ProjectileController>().owner;
            stick = GetComponent<ProjectileStickOnImpact>();
            controller = GetComponent<ProjectileController>();
            areaIndicator.transform.localScale = Vector3.zero;
        }
        private void FixedUpdate()
        {
            if(stick.stuck)
            {
                if (monoTrigger)
                {
                    areaIndicator.SetActive(true);
                    monoTrigger = false;
                }
            }
            if(monoTrigger2 && !monoTrigger)
            {
                if (countDown > 0)
                {
                    countDown -= Time.fixedDeltaTime;
                    areaIndicator.SetActive(true);
                    areaIndicator.transform.localScale = new Vector3(radiusMax, radiusMax, radiusMax) * (1 - (countDown / 2f));
                    areaIndicator.transform.position = controller.transform.position;
                }
                else
                {
                    owner.GetComponent<SwarmComponent>().GetTargets(controller.transform.position);
                    monoTrigger2 = false;
                    areaIndicator.SetActive(false);
                    EntityState.Destroy(areaIndicator.gameObject);
                }
            }
        }
    }
}
