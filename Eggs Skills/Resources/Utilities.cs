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
    }
}
