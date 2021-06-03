using System;
using UnityEngine;
using Mono.Cecil;
using EggsSkills.Resources;
using EggsSkills.Utility;

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
            assetBundle = LoadAssetBundle(Resources.mainbundle);
            if (assetBundle)
            {
                Utilities.LogToConsole("Assetbundle successfully loaded");
            }
            else
            {
                Utilities.LogToConsole("Assetbundle failed to load");
            }
        }
        internal static Sprite TexToSprite(Texture2D tex)
        {
            return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

        internal static AssetBundle LoadAssetBundle(Byte[] resourceBytes)
        {
            if (resourceBytes == null) throw new ArgumentNullException(nameof(Resource));
            var bundle = AssetBundle.LoadFromMemory(resourceBytes);
            return bundle;
        }
    }
}
