using System.Linq;
using RoR2;
using UnityEngine;

namespace EggsSkills
{
    [RequireComponent(typeof(CharacterBody))]
    [RequireComponent(typeof(InputBankTest))]
    [RequireComponent(typeof(TeamComponent))]
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
            this.characterBody = GetComponent<CharacterBody>();
            this.inputBank = GetComponent<InputBankTest>();
            this.teamComponent = GetComponent<TeamComponent>();
        }
        private void FixedUpdate()
        {
            //Tick up the stopwatch
            this.trackerUpdateStopwatch += Time.fixedDeltaTime;
            //If the stopwatch has gone on for .1 seconds...
            if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
            {
                //Reset the timer
                this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
                //Poisoncounter to 0 cause we haven't found any poisoned creatures yet
                this.poisonCounter = 0;
                //Spheresearch it
                foreach (HurtBox hurtBox in new SphereSearch
                {
                    origin = characterBody.footPosition,
                    radius = this.maxTrackingDistance,
                    mask = LayerIndex.entityPrecise.mask
                }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(this.teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
                {
                    //Get the targets body component
                    CharacterBody body = hurtBox.healthComponent.body;
                    //Make sure they stats ain't brokeded
                    body.RecalculateStats();
                    //If they have either buff, tick up the counter
                    if (body.HasBuff(RoR2Content.Buffs.Poisoned) || body.HasBuff(RoR2Content.Buffs.Blight)) this.poisonCounter += 1;
                }
                //Set the totalpoisoned count to what we found
                this.totalPoisoned = this.poisonCounter;
            }
        }
        
        //Get the amount of poisoned enemies
        public float GetPoisonedCount()
        {
            //Just return how many poisoned targets we got
            return this.totalPoisoned;
        }
    }
}
