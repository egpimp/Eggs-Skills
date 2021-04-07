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
        public CharacterBody characterBody;
        public TeamComponent teamComponent;
        public InputBankTest inputBank;
        private float trackerUpdateStopwatch;
        public float trackerUpdateFrequency = 10f;
        public float maxTrackingDistance = 5000f;
        public float poisonCounter;
        public float totalPoisoned;

        private void Start()
        {
            this.characterBody = base.GetComponent<CharacterBody>();
            this.inputBank = base.GetComponent<InputBankTest>();
            this.teamComponent = base.GetComponent<TeamComponent>();
        }
        private void FixedUpdate()
        {
            this.trackerUpdateStopwatch += Time.fixedDeltaTime;
            if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
            {
                this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
                poisonCounter = 0;
                foreach (HurtBox hurtBox in new SphereSearch
                {
                    origin = characterBody.footPosition,
                    radius = this.maxTrackingDistance,
                    mask = LayerIndex.entityPrecise.mask
                }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(this.teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
                {
                    CharacterBody body = hurtBox.healthComponent.body;
                    body.RecalculateStats();
                    if (body.HasBuff(RoR2Content.Buffs.Poisoned) || body.HasBuff(RoR2Content.Buffs.Blight))
                    {                       
                        this.poisonCounter += 1;
                    };
                }
                this.totalPoisoned = poisonCounter;
                this.poisonCounter = 0;
            }
        }
        public float GetPoisonedCount()
        {
            return this.totalPoisoned;
        }
    }
}
