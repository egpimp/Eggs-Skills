using UnityEngine;
using EggsSkills.Properties;
using static EggsSkills.EggsSkills;

namespace EggsSkills.Resources
{
    class Sprites
    {
        //Placeholder
        internal static Sprite placeholderIconS;

        //Normal icons
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

        //Scepter icons
        internal static Sprite slashportUpgradedIconS;

        internal static Sprite rexrootUpgradedIconS;

        internal static Sprite acridpurgeUpgradedIconS;

        internal static void LoadIcons()
        {
            Texture2D placeholderIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_placeholder");
            placeholderIconS = EggsUtils.Properties.Assets.TexToSprite(placeholderIcon);
            try { CommandoIcons(); } catch { Log.LogError("Failed to load Commando icons"); }
            try { ArtificerIcons(); } catch { Log.LogError("Failed to load Artificer icons"); }
            try { BanditIcons(); } catch { Log.LogError("Failed to load Bandit icons"); }
            try { RexIcons(); } catch { Log.LogError("Failed to load Rex icons"); }
            try { AcridIcons(); } catch { Log.LogError("Failed to load Acrid icons"); }
            try { LoaderIcons(); } catch { Log.LogError("Failed to load Loader icons"); }
            try { CaptainIcons(); } catch { Log.LogError("Failed to load Captain icons"); }
            try { HuntressIcons(); } catch { Log.LogError("Failed to load Huntress icons"); }
            try { EngiIcons(); } catch { Log.LogError("Failed to load Engineer icons"); }
            try { MercIcons(); } catch { Log.LogError("Failed to load Mercenary icons"); }
            try { MulTIcons(); } catch { Log.LogError("Failed to load MUL-T icons"); } 
            try { if (classicItemsLoaded) ScepterIcons(); } catch { Log.LogError("Failed to load Scepter icons"); }
            Log.LogMessage("Sprites loaded");
        }
        internal static void CommandoIcons()
        {
            //Shotgun
            Texture2D shotgunIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_commandoshotgun");
            shotgunIconS = EggsUtils.Properties.Assets.TexToSprite(shotgunIcon);
            //Dash
            Texture2D dashIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_dash");
            dashIconS = EggsUtils.Properties.Assets.TexToSprite(dashIcon);
        }
        internal static void ArtificerIcons()
        {
            //Zapport
            Texture2D zapportIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_surgeteleport");
            zapportIconS = EggsUtils.Properties.Assets.TexToSprite(zapportIcon);
        }
        internal static void BanditIcons()
        {
            //InvisSprint
            Texture2D invisSprintIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_invissprint");
            invisSprintIconS = EggsUtils.Properties.Assets.TexToSprite(invisSprintIcon);
            //MagicBullet
            Texture2D magicBulletIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_richochet");
            magicBulletIconS = EggsUtils.Properties.Assets.TexToSprite(magicBulletIcon);
        }
        internal static void RexIcons()
        {
            //Respire
            Texture2D rexrootIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_rexroot");
            rexrootIconS = EggsUtils.Properties.Assets.TexToSprite(rexrootIcon);
        }
        internal static void AcridIcons()
        {
            //Expunge
            Texture2D acridpurgeIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_expunge");
            acridpurgeIconS = EggsUtils.Properties.Assets.TexToSprite(acridpurgeIcon);
        }
        internal static void LoaderIcons()
        {
            //Shieldsplosion
            Texture2D shieldSplosionIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_shieldsplosion");
            shieldsplosionIconS = EggsUtils.Properties.Assets.TexToSprite(shieldSplosionIcon);
        }
        internal static void CaptainIcons()
        {
            //DebuffGrenade
            Texture2D debuffNadeIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_debuffnade");
            debuffNadeIconS = EggsUtils.Properties.Assets.TexToSprite(debuffNadeIcon);
        }
        internal static void HuntressIcons()
        {
            //ClusterArrow
            Texture2D clusterArrowIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_bombarrow");
            clusterArrowIconS = EggsUtils.Properties.Assets.TexToSprite(clusterArrowIcon);
        }
        internal static void MercIcons()
        {
            //Slashport
            Texture2D slashportIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_fatalteleport");
            slashportIconS = EggsUtils.Properties.Assets.TexToSprite(slashportIcon);
        }
        internal static void EngiIcons()
        {
            //TeslaMines
            Texture2D teslaMineIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_teslamine");
            teslaMineIconS = EggsUtils.Properties.Assets.TexToSprite(teslaMineIcon);
        }
        internal static void MulTIcons()
        {
            //Nanobots
            Texture2D nanoBotsIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_nanobots");
            nanoBotsIconS = EggsUtils.Properties.Assets.TexToSprite(nanoBotsIcon);
        }

        internal static void ScepterIcons()
        {
            //Slashport Upgrade
            Texture2D slashportUpgradedIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_fatalteleport_upgraded");
            slashportUpgradedIconS = EggsUtils.Properties.Assets.TexToSprite(slashportUpgradedIcon);

            //Rexroot Upgrade
            Texture2D rexrootUpgradedIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_rexroot_upgraded");
            rexrootUpgradedIconS = EggsUtils.Properties.Assets.TexToSprite(rexrootUpgradedIcon);

            //Acridpurge Upgrade
            Texture2D acridpurgeIcon = Assets.assetBundle.LoadAsset<Texture2D>("icon_expunge_upgraded");
            acridpurgeUpgradedIconS = EggsUtils.Properties.Assets.TexToSprite(acridpurgeIcon);
        }
    }
}
