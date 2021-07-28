using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace EggsSkills
{
    class BreathingroomAchievementHandler : MonoBehaviour
    {
        private int kills;
        private List<HurtBox> nearbyEnemies = new List<HurtBox>();
        private CharacterBody characterBody;
        private TeamComponent teamComponent;
        private void Start()
        {
            characterBody = base.GetComponent<CharacterBody>();
            teamComponent = base.GetComponent<TeamComponent>();
            kills = 0;
        }

        internal bool HasReqKills()
        {
            return kills >= 100;
        }

        internal void AddKill(CharacterBody body)
        {
            foreach (HurtBox hurtBox in new SphereSearch
            {
                origin = characterBody.corePosition,
                radius = 16,
                mask = LayerIndex.entityPrecise.mask
            }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
            {
                nearbyEnemies.Add(hurtBox.hurtBoxGroup.mainHurtBox);
            }
            if (nearbyEnemies.Contains(body.mainHurtBox))
            {
                kills += 1;
            }
        }
    }
}
