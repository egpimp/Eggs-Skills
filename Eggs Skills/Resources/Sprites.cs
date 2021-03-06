using UnityEngine;
using EggsSkills.Properties;

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

        internal static Sprite magicBulletIconS;

        internal static Sprite dashIconS;

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
            EggsUtils.EggsUtils.LogToConsole("Sprites loaded");
        }
        internal static void CommandoIcons()
        {
            //Shotgun
            Texture2D shotgunIcon = Assets.assetBundle.LoadAsset<Texture2D>("texcommandoshotgun");
            shotgunIconS = EggsUtils.Properties.Assets.TexToSprite(shotgunIcon);
            //Dash
            Texture2D dashIcon = Assets.assetBundle.LoadAsset<Texture2D>("texdash");
            dashIconS = EggsUtils.Properties.Assets.TexToSprite(dashIcon);
        }
        internal static void ArtificerIcons()
        {
            //Zapport
            Texture2D zapportIcon = Assets.assetBundle.LoadAsset<Texture2D>("texsurgeteleport");
            zapportIconS = EggsUtils.Properties.Assets.TexToSprite(zapportIcon);
        }
        internal static void BanditIcons()
        {
            //InvisSprint
            Texture2D invisSprintIcon = Assets.assetBundle.LoadAsset<Texture2D>("texinvissprint");
            invisSprintIconS = EggsUtils.Properties.Assets.TexToSprite(invisSprintIcon);
            //MagicBullet
            Texture2D magicBulletIcon = Assets.assetBundle.LoadAsset<Texture2D>("texrichochet");
            magicBulletIconS = EggsUtils.Properties.Assets.TexToSprite(magicBulletIcon);
        }
        internal static void RexIcons()
        {
            //Respire
            Texture2D rexrootIcon = Assets.assetBundle.LoadAsset<Texture2D>("texrexroot");
            rexrootIconS = EggsUtils.Properties.Assets.TexToSprite(rexrootIcon);
        }
        internal static void AcridIcons()
        {
            //Expunge
            Texture2D acridpurgeIcon = Assets.assetBundle.LoadAsset<Texture2D>("texexpunge");
            acridpurgeIconS = EggsUtils.Properties.Assets.TexToSprite(acridpurgeIcon);
        }
        internal static void LoaderIcons()
        {
            //Shieldsplosion
            Texture2D shieldSplosionIcon = Assets.assetBundle.LoadAsset<Texture2D>("texshieldsplosion");
            shieldsplosionIconS = EggsUtils.Properties.Assets.TexToSprite(shieldSplosionIcon);
        }
        internal static void CaptainIcons()
        {
            //DebuffGrenade
            Texture2D debuffNadeIcon = Assets.assetBundle.LoadAsset<Texture2D>("texdebuffnade");
            debuffNadeIconS = EggsUtils.Properties.Assets.TexToSprite(debuffNadeIcon);
        }
        internal static void HuntressIcons()
        {
            //ClusterArrow
            Texture2D clusterArrowIcon = Assets.assetBundle.LoadAsset<Texture2D>("texbombarrow");
            clusterArrowIconS = EggsUtils.Properties.Assets.TexToSprite(clusterArrowIcon);
        }
        internal static void MercIcons()
        {
            //Slashport
            Texture2D slashportIcon = Assets.assetBundle.LoadAsset<Texture2D>("texfatalteleport");
            slashportIconS = EggsUtils.Properties.Assets.TexToSprite(slashportIcon);
        }
        internal static void EngiIcons()
        {
            //TeslaMines
            Texture2D teslaMineIcon = Assets.assetBundle.LoadAsset<Texture2D>("texteslamine");
            teslaMineIconS = EggsUtils.Properties.Assets.TexToSprite(teslaMineIcon);
        }
        internal static void MulTIcons()
        {
            //Nanobots
            Texture2D nanoBotsIcon = Assets.assetBundle.LoadAsset<Texture2D>("texnanobots");
            nanoBotsIconS = EggsUtils.Properties.Assets.TexToSprite(nanoBotsIcon);
        }
    }
}
