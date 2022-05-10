using System.Linq;
using RoR2;
using UnityEngine;

namespace EggsSkills
{
    class AcridPurgeTracker : MonoBehaviour
    {
        //Characterbody of acrid
        public CharacterBody characterBody;

        //How far away can enemies be tracked by this
        private float maxTrackingDistance = 50000f;
        //How many poisoned enemies
        private float poisonCounter;
        //Poison counter but more stable and safe for access
        private float totalPoisoned;
        //How many times per second to update the tracker
        private float trackerUpdateFrequency = 10f;
        //Handles the tracker updating
        private float trackerUpdateStopwatch;

        //Inputbank of the acrid
        public InputBankTest inputBank;

        //Team of the acrid
        public TeamComponent teamComponent;

        private void Start()
        {
            //Get these three components
            characterBody = base.GetComponent<CharacterBody>();
            inputBank = base.GetComponent<InputBankTest>();
            teamComponent = base.GetComponent<TeamComponent>();
        }
        private void FixedUpdate()
        {
            //Tick up the stopwatch
            trackerUpdateStopwatch += Time.fixedDeltaTime;
            //If the stopwatch has gone on for .1 seconds...
            if (trackerUpdateStopwatch >= 1f / trackerUpdateFrequency)
            {
                //Reset the timer
                trackerUpdateStopwatch -= 1f / trackerUpdateFrequency;
                //Poisoncounter to 0 cause we haven't found any poisoned creatures yet
                poisonCounter = 0;
                //Spheresearch it
                foreach (HurtBox hurtBox in new SphereSearch
                {
                    origin = characterBody.footPosition,
                    radius = maxTrackingDistance,
                    mask = LayerIndex.entityPrecise.mask
                }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
                {
                    //Get the targets body component
                    CharacterBody body = hurtBox.healthComponent.body;
                    //Make sure they stats ain't brokeded
                    body.RecalculateStats();
                    //If they have either buff, tick up the counter
                    if (body.HasBuff(RoR2Content.Buffs.Poisoned) || body.HasBuff(RoR2Content.Buffs.Blight)) poisonCounter += 1;
                }
                //Set the totalpoisoned count to what we found
                totalPoisoned = poisonCounter;
            }
        }
        
        //Get the amount of poisoned enemies
        public float GetPoisonedCount()
        {
            //Just return how many poisoned targets we got
            return totalPoisoned;
        }
    }
}
