using RoR2;
using UnityEngine;

namespace EggsSkills
{
    class TraditionalAchievementHandler : MonoBehaviour
    {
        private bool canGrant;
        private InputBankTest input;
        private void Start()
        {
            input = GetComponent<InputBankTest>();
            canGrant = true;
        }

        private void FixedUpdate()
        {
            if(input.skill2.down || input.skill4.down)
            {
                canGrant = false;
            }
        }

        internal void ResetEligibility()
        {
            canGrant = true;
        }

        internal bool CheckEligibility()
        {
            return canGrant;
        }
    }
}
