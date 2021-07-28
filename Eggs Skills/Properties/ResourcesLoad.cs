using UnityEngine;
using EggsSkills.Resources;

namespace EggsSkills.Properties
{
    internal static class Assets
    {
        internal static AssetBundle assetBundle;
        internal static void LoadResources()
        {
            LoadMainAssetbundle();
            LanguageTokens.RegisterLanguageTokens();
            Sprites.LoadIcons();
            Projectiles.RegisterProjectiles();
        }
        internal static void LoadMainAssetbundle()
        {
            assetBundle = (EggsUtils.Properties.Assets.LoadAssetBundle(Resources.eggsskillsbundle));
            if (assetBundle)
            {
                EggsUtils.EggsUtils.LogToConsole("Assetbundle successfully loaded");
            }
            else
            {
                EggsUtils.EggsUtils.LogToConsole("Assetbundle failed to load");
            }
        }
    }
}
