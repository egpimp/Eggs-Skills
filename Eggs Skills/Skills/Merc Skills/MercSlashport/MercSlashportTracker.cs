using System.Linq;
using RoR2;
using UnityEngine;

namespace EggsSkills
{
    [RequireComponent(typeof(CharacterBody))]
    [RequireComponent(typeof(InputBankTest))]
    [RequireComponent(typeof(TeamComponent))]
    public class MercSlashportTracker : MonoBehaviour
    {
        public Indicator indicator;
        public CharacterBody characterBody;
        public TeamComponent teamComponent;
        public InputBankTest inputBank;
        public HurtBox trackingTarget;
        public float trackerUpdateStopwatch;
        public float trackerUpdateFrequency = 10f;
        public BullseyeSearch search = new BullseyeSearch();
        public float maxTrackingDistance = 60f;
        public float maxTrackingAngle = 15f;

        private void Awake()
        {
            this.indicator = new Indicator(base.gameObject, UnityEngine.Resources.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
        }
        private void Start()
        {
            this.characterBody = base.GetComponent<CharacterBody>();
            this.inputBank = base.GetComponent<InputBankTest>();
            this.teamComponent = base.GetComponent<TeamComponent>();
        }
        public HurtBox GetTrackingTarget()
        {
            return this.trackingTarget;
        }
        private void FixedUpdate()
        {
            this.trackerUpdateStopwatch += Time.fixedDeltaTime;
            if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
            {
                    this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
                    HurtBox hurtBox = this.trackingTarget;
                    Ray aimRay = new Ray(this.inputBank.aimOrigin, this.inputBank.aimDirection);
                    this.SearchForTarget(aimRay);
                    this.indicator.targetTransform = (this.trackingTarget ? this.trackingTarget.transform : null);
            }
            if(this.characterBody.skillLocator.utility.cooldownRemaining <= 0)
            {
                this.indicator.active = true;
            }
            else
            {
                this.indicator.active = false;
            };
        }
        private void SearchForTarget(Ray aimRay)
        {
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(this.teamComponent.teamIndex);
            this.search.filterByLoS = true;
            this.search.searchOrigin = aimRay.origin;
            this.search.searchDirection = aimRay.direction;
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            this.search.maxDistanceFilter = this.maxTrackingDistance;
            this.search.maxAngleFilter = this.maxTrackingAngle;
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(base.gameObject);
            this.trackingTarget = this.search.GetResults().FirstOrDefault<HurtBox>();
        }
    }
}
