using RoR2.Skills;
using RoR2;
using UnityEngine;

namespace EggsSkills
{
    class BulletAchievementHandler : MonoBehaviour
    {
        private InputBankTest input;
        private bool isFiring;
        private int killCount;
        private bool reqMet;
        private void Start()
        {
            input = GetComponent<InputBankTest>();
            isFiring = false;
            killCount = 0;
            reqMet = false;
        }
        private void FixedUpdate()
        {
            isFiring = input.skill1.down && !(input.skill2.down || input.skill4.down);
            if(!isFiring)
            {
                killCount = 0;
                reqMet = false;
            }
        }
        internal void AddKill()
        {
            if (isFiring)
            {
                killCount += 1;
            }
        }
        internal bool IsReqMet()
        {
            if (killCount >= 20 && isFiring)
            {
                reqMet = true;
            }
            else
            {
                reqMet = false;
            }
            return reqMet;
        }
    }
}
