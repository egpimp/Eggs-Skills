using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace EggsSkills
{
    class CullAchievementHandler : MonoBehaviour
    {
        private List<CharacterBody> victimsList = new List<CharacterBody>();
        private float resetTimer;
        private void Start()
        {
            resetTimer = 0f;
        }

        private void FixedUpdate()
        {
            if(resetTimer > 0)
            {
                resetTimer -= Time.fixedDeltaTime;
            }
            else
            {
                while (victimsList.Count > 0)
                {
                    victimsList.RemoveAt(0);
                }
                resetTimer = 0;
            }
        }

        internal bool ReqMet()
        {
            return victimsList.Count >= 10f;
        }

        internal void AddVictim(HealthComponent victim)
        {
            if (victimsList.Contains(victim.body) == false)
            {
                victimsList.Add(victim.body);
            }
            resetTimer = 5f;
        }
    }
}
