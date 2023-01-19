using RoR2;
using System.Runtime.CompilerServices;

namespace EggsSkills.ModCompats
{
    internal static class DeeprotCompat
    {
        internal static bool CheckHasDeeprot(CharacterBody body)
        {
            //If its not loaded return false
            if (!EggsSkills.plasmacoreSpikestripLoaded) return false;
            //If it is loaded, use the soft-dependancy-safe check
            return InternalDeeprotCheck(body);
        }

        internal static bool CheckHasSoulrot(CharacterBody body)
        {
            //If not loaded return false
            if (!EggsSkills.plasmacoreSpikestripLoaded) return false;
            //If loaded use the safe check
            return InternalSoulrotCheck(body);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool InternalDeeprotCheck(CharacterBody body)
        {
            return body.HasBuff(PlasmaCoreSpikestripContent.Content.Skills.DeepRot.scriptableObject.buffs[1]);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool InternalSoulrotCheck(CharacterBody body)
        {
            return body.HasBuff(PlasmaCoreSpikestripContent.Content.Skills.DeepRot.scriptableObject.buffs[0]);
        }
    }
}
