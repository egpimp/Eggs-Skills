using UnityEngine;

namespace EggsSkills
{
    class HatTrickAchievementHandler : MonoBehaviour
    {
        private int kills;
        private float killsTimer;
        private const float killsBaseTimer = 1f;

        private void Start()
        {
            this.kills = 0;
        }
        
        private void FixedUpdate()
        {
            if (this.killsTimer > 0)
            {
                this.killsTimer -= Time.fixedDeltaTime;
            }
            else
            {
                this.killsTimer = 0;
                this.kills = 0;
            }
        }

        internal void AddKill()
        {
            if(this.kills == 0)
            {
                this.killsTimer = killsBaseTimer;
            }
            this.kills += 1;
        }

        internal bool IsReqMet()
        {
            return this.kills >= 3 && this.killsTimer > 0;
        }
    }
}
