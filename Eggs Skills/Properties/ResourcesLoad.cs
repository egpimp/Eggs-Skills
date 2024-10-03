using UnityEngine;
using EggsSkills.Resources;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using RoR2;

namespace EggsSkills.Properties
{
    internal static class SkillsAssets
    {
        //Path for our lang folder
        internal const string LangFolder = "egmods_languages";
        internal static string RootLangFolderPath => System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), LangFolder);

        //Reference for our assetbundle
        internal static AssetBundle assetBundle;
        internal static void LoadResources()
        {
            //Register language
            if (Directory.Exists(RootLangFolderPath)) Language.collectLanguageRootFolders += RegisterTokensFolder;
            else Log.LogError("Could not find EggSkills language folder");
            //Loads the main asset bundle just like it says
            LoadMainAssetbundle();
            //Loads our icons
            Sprites.LoadIcons();
            //Register custom effects

            //Registers our custom projectiles
            Projectiles.RegisterProjectiles();
        }

        private static void RegisterTokensFolder(List<string> list)
        {
            Log.LogError(RootLangFolderPath);
            foreach (string l in list) Log.LogError(l);
            //Add our folder full of language tokens to be loaded
            list.Add(RootLangFolderPath);
        }

        internal static void LoadMainAssetbundle()
        {
            //Assign the assetbundle
            assetBundle = EggsUtils.Properties.EggAssets.LoadAssetBundle(Resources.eggsskillsbundle);
            //If it exists it loaded just fine
            if (assetBundle) Log.LogMessage("Assetbundle successfully loaded");
            //Otherwise, it didn't load just fine
            else Log.LogMessage("Assetbundle failed to load");
        }
    }
}
