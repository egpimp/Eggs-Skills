using UnityEngine;
using EggsSkills.Resources;

namespace EggsSkills.Properties
{
    internal static class Assets
    {
        //Reference for our assetbundle
        internal static AssetBundle assetBundle;
        internal static void LoadResources()
        {
            //Loads the main asset bundle just like it says
            LoadMainAssetbundle();
            //Loads our icons (U the best SOM)
            Sprites.LoadIcons();
            //Registers our custom projectiles
            Projectiles.RegisterProjectiles();
        }
        internal static void LoadMainAssetbundle()
        {
            //Assign the assetbundle
            assetBundle = EggsUtils.Properties.Assets.LoadAssetBundle(Resources.eggsskillsbundle);
            //If it exists it loaded just fine
            if (assetBundle) Log.LogMessage("Assetbundle successfully loaded");
            //Otherwise, it didn't load just fine
            else Log.LogMessage("Assetbundle failed to load");
        }
    }
}
