using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;

namespace EggsSkills.Properties
{
    public static class Assets
    {
        public static Texture2D LoadTexture2D(Byte[] resourceBytes)
        {
            if (resourceBytes == null) throw new ArgumentNullException(nameof(resourceBytes));

            var tempTex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
            tempTex.LoadImage(resourceBytes, false);
            return tempTex;
        }
        public static Sprite TexToSprite(Texture2D tex)
        {
            return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
    }
}
