using UnityEngine;
using EggsSkills.Properties;
using EggsSkills.Utility;

namespace EggsSkills.Resources
{
    class Sprites
    {
        internal static Sprite shotgunIconS;

        internal static Sprite zapportIconS;

        internal static Sprite slashportIconS;

        internal static Sprite rexrootIconS;

        internal static Sprite acridpurgeIconS;

        internal static Sprite shieldsplosionIconS;

        internal static Sprite teslaMineIconS;

        internal static Sprite debuffNadeIconS;

        internal static Sprite invisSprintIconS;

        internal static Sprite nanoBotsIconS;

        internal static Sprite clusterArrowIconS;

        internal static void LoadIcons()
        {
            CommandoIcons();
            ArtificerIcons();
            BanditIcons();
            RexIcons();
            AcridIcons();
            LoaderIcons();
            CaptainIcons();
            HuntressIcons();
            EngiIcons();
            MercIcons();
            MulTIcons();
            Utilities.LogToConsole("Sprites loaded");
        }
        internal static void CommandoIcons()
        {
            //Shotgun
            Texture2D shotgunIcon = Assets.assetBundle.LoadAsset<Texture2D>("texcommandoshotgun");
            shotgunIconS = Assets.TexToSprite(shotgunIcon);
        }
        internal static void ArtificerIcons()
        {
            //Zapport
            Texture2D zapportIcon = Assets.assetBundle.LoadAsset<Texture2D>("texsurgeteleport");
            zapportIconS = Assets.TexToSprite(zapportIcon);
        }
        internal static void BanditIcons()
        {
            //InvisSprint
            Texture2D invisSprintIcon = Assets.assetBundle.LoadAsset<Texture2D>("texinvissprint");
            invisSprintIconS = Assets.TexToSprite(invisSprintIcon);
        }
        internal static void RexIcons()
        {
            //Respire
            Texture2D rexrootIcon = Assets.assetBundle.LoadAsset<Texture2D>("texrexroot");
            rexrootIconS = Assets.TexToSprite(rexrootIcon);
        }
        internal static void AcridIcons()
        {
            //Expunge
            Texture2D acridpurgeIcon = Assets.assetBundle.LoadAsset<Texture2D>("texexpunge");
            acridpurgeIconS = Assets.TexToSprite(acridpurgeIcon);
        }
        internal static void LoaderIcons()
        {
            //Shieldsplosion
            Texture2D shieldSplosionIcon = Assets.assetBundle.LoadAsset<Texture2D>("texshieldsplosion");
            shieldsplosionIconS = Assets.TexToSprite(shieldSplosionIcon);
        }
        internal static void CaptainIcons()
        {
            //DebuffGrenade
            Texture2D debuffNadeIcon = Assets.assetBundle.LoadAsset<Texture2D>("texdebuffnade");
            debuffNadeIconS = Assets.TexToSprite(debuffNadeIcon);
        }
        internal static void HuntressIcons()
        {
            //ClusterArrow
            Texture2D clusterArrowIcon = Assets.assetBundle.LoadAsset<Texture2D>("texbombarrow");
            clusterArrowIconS = Assets.TexToSprite(clusterArrowIcon);
        }
        internal static void MercIcons()
        {
            //Slashport
            Texture2D slashportIcon = Assets.assetBundle.LoadAsset<Texture2D>("texfatalteleport");
            slashportIconS = Assets.TexToSprite(slashportIcon);
        }
        internal static void EngiIcons()
        {
            //TeslaMines
            Texture2D teslaMineIcon = Assets.assetBundle.LoadAsset<Texture2D>("texteslamine");
            teslaMineIconS = Assets.TexToSprite(teslaMineIcon);
        }
        internal static void MulTIcons()
        {
            //Nanobots
            Texture2D nanoBotsIcon = Assets.assetBundle.LoadAsset<Texture2D>("texnanobots");
            nanoBotsIconS = Assets.TexToSprite(nanoBotsIcon);
        }
    }
}
