using UnityEngine;

namespace EggsSkills.Utility
{
    class Utilities
    {
        internal static float ConvertToRange(float oldMin, float oldMax, float newMin, float newMax, float valueToConvert)
        {
            float oldRange = oldMax - oldMin;
            float newRange = newMax - newMin;
            return (((valueToConvert - oldMin) * newRange) / oldRange) + newMin;
        }
        internal static Vector3 GetDirection(Vector3 startPos, Vector3 endPos)
        {
            return (endPos - startPos).normalized;
        }
        internal static void LogToConsole(string logText)
        {
            Debug.Log("EggsSkills : " + logText);
        }
    }
}
