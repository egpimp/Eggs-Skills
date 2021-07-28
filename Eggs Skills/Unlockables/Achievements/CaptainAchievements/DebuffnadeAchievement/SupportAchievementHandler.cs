using UnityEngine;

namespace EggsSkills
{
    class SupportAchievementHandler : MonoBehaviour
    {
        private bool isValid;
        private void Start()
        {
            isValid = true;
        }

        internal void Revalidate()
        {
            isValid = true;
        }

        internal void Invalidate()
        {
            isValid = false;
        }

        internal bool IsValid()
        {
            return isValid;
        }
    }
}
