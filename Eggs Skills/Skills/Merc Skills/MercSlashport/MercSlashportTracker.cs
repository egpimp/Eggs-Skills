using System.Linq;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
        public readonly float maxTrackingDistance = 45f;

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
            indicator = new Indicator(base.gameObject, Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Huntress/HuntressTrackingIndicator.prefab").WaitForCompletion());
            //Grab characterbody
            characterBody = base.GetComponent<CharacterBody>();
            //Grab inputbank
            inputBank = base.GetComponent<InputBankTest>();
            //Grab teamcomponent
            teamComponent = base.GetComponent<TeamComponent>();
        }

        public HurtBox GetTrackingTarget()
        {
            //Return the tracking target
            return trackingTarget;
        }

        private void FixedUpdate()
        {
            //Update the tracker
            trackerUpdateStopwatch += Time.fixedDeltaTime;
            //If tracker hits the max
            if (trackerUpdateStopwatch >= 1f / trackerUpdateFrequency)
            {
                //Reset the tracker
                trackerUpdateStopwatch -= 1f / trackerUpdateFrequency;
                //Grab the target hurtbox
                HurtBox hurtBox = trackingTarget;
                //Grab the aimray
                Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
                //Search for a target with the aimray
                SearchForTarget(aimRay);
                //Get transform of the target if they exist
                indicator.targetTransform = (trackingTarget ? trackingTarget.transform : null);
            }
            //If not on cooldown indicator allowed to display
            if(characterBody.skillLocator.special.cooldownRemaining <= 0) indicator.active = true;
            //If on cooldown no indicator
            else indicator.active = false;
        }
        private void SearchForTarget(Ray aimRay)
        {
            //Let it target non-allies
            search.teamMaskFilter = TeamMask.GetUnprotectedTeams(teamComponent.teamIndex);
            //Must have LoS to target
            search.filterByLoS = true;
            //Start search at player aimray
            search.searchOrigin = aimRay.origin;
            //Search where player facing
            search.searchDirection = aimRay.direction;
            //Sort by distance
            search.sortMode = BullseyeSearch.SortMode.Distance;
            //Max dist and angle, dist based on speed
            search.maxDistanceFilter = this.maxTrackingDistance;
            search.maxAngleFilter = this.maxTrackingAngle;
            //Refresh the targets
            search.RefreshCandidates();
            //Don't target self (lol)
            search.FilterOutGameObject(base.gameObject);
            //Get the target
            trackingTarget = this.search.GetResults().FirstOrDefault();
        }
    }
}
