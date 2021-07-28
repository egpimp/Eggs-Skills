using UnityEngine;
using RoR2;

namespace EggsSkills
{
    class FlankedAchievmentHandler : MonoBehaviour
    {
        private CharacterBody body;
        private float countDown;
        private void Start()
        {
            body = GetComponent<CharacterBody>();
            countDown = 180f;
        }
        private void FixedUpdate()
        {
            if (body.HasBuff(RoR2Content.Buffs.Cloak))
            {
                if (countDown > 0)
                {
                    countDown -= Time.fixedDeltaTime;
                }
                else if(countDown < 0)
                {
                    countDown = 0;
                }
            }
        }
        internal bool MeetsRequirement()
        {
            if(countDown <= 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
