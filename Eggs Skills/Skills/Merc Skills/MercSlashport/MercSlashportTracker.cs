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
        //The bullseye search we use
        public BullseyeSearch search = new BullseyeSearch();

        //Characterbody of the player
        public CharacterBody characterBody;

        //How many times / second to update the tracker
        public readonly float trackerUpdateFrequency = 10f;
        //Helps handle the tracker updating
        public float trackerUpdateStopwatch;
        //Max angle to lock onto with
        public readonly float maxTrackingAngle = 15f;
        //Max distance to track from
        public readonly float maxTrackingDistance = 60f;

        //Target the tracker spots
        public HurtBox trackingTarget;

        //Targeting indicator
        public Indicator indicator;

        //Player inputbank
        public InputBankTest inputBank;

        //Team component of the player
        public TeamComponent teamComponent;

        private void Start()
        {
            //Setup the indicator
            this.indicator = new Indicator(base.gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
            //Grab characterbody
            this.characterBody = base.GetComponent<CharacterBody>();
            //Grab inputbank
            this.inputBank = base.GetComponent<InputBankTest>();
            //Grab teamcomponent
            this.teamComponent = base.GetComponent<TeamComponent>();
        }

        public HurtBox GetTrackingTarget()
        {
            //Return the tracking target
            return this.trackingTarget;
        }

        private void FixedUpdate()
        {
            //Update the tracker
            this.trackerUpdateStopwatch += Time.fixedDeltaTime;
            //If tracker hits the max
            if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
            {
                //Reset the tracker
                this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
                //Grab the target hurtbox
                HurtBox hurtBox = this.trackingTarget;
                //Grab the aimray
                Ray aimRay = new Ray(this.inputBank.aimOrigin, this.inputBank.aimDirection);
                //Search for a target with the aimray
                this.SearchForTarget(aimRay);
                //Get transform of the target if they exist
                this.indicator.targetTransform = (this.trackingTarget ? this.trackingTarget.transform : null);
            }
            //If not on cooldown indicator allowed to display
            if(this.characterBody.skillLocator.utility.cooldownRemaining <= 0) this.indicator.active = true;
            //If on cooldown no indicator
            else this.indicator.active = false;
        }
        private void SearchForTarget(Ray aimRay)
        {
            //Let it target non-allies
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(this.teamComponent.teamIndex);
            //Must have LoS to target
            this.search.filterByLoS = true;
            //Start search at player aimray
            this.search.searchOrigin = aimRay.origin;
            //Search where player facing
            this.search.searchDirection = aimRay.direction;
            //Sort by distance
            this.search.sortMode = BullseyeSearch.SortMode.Distance;
            //Max dist and angle, dist based on speed
            this.search.maxDistanceFilter = this.maxTrackingDistance * (this.characterBody.moveSpeed / 7);
            this.search.maxAngleFilter = this.maxTrackingAngle;
            //Refresh the targets
            this.search.RefreshCandidates();
            //Don't target self (lol)
            this.search.FilterOutGameObject(base.gameObject);
            //Get the target
            this.trackingTarget = this.search.GetResults().FirstOrDefault();
        }
    }
}
