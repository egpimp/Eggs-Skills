using UnityEngine;
using EggsSkills.Properties;
using static EggsSkills.EggsSkills;
using EggsUtils.Properties;

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

        internal static Sprite acridpoisonbreathIconS;

        internal static Sprite acridpurgeIconS;

        internal static Sprite shieldsplosionIconS;

        internal static Sprite teslaMineIconS;

        internal static Sprite debuffNadeIconS;

        internal static Sprite invisSprintIconS;

        internal static Sprite nanoBotsIconS;

        internal static Sprite clusterarrowIconS;

        internal static Sprite magicBulletIconS;

        internal static Sprite dashIconS;

        internal static Sprite inversionIconS;
        internal static Sprite inversionAltIconS;

        internal static Sprite micromissileIconS;

        internal static Sprite autoshotgunIconS;

        internal static Sprite lanceroundsIconS;

        //Scepter icons
        internal static Sprite slashportUpgradedIconS;

        internal static Sprite rexrootUpgradedIconS;

        internal static Sprite acridpurgeUpgradedIconS;

        internal static void LoadIcons()
        {
            Texture2D placeholderIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_placeholder");
            placeholderIconS = EggAssets.TexToSprite(placeholderIcon);
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
            try { VoidFiendIcons(); } catch { Log.LogError("Failed to load Void Fiend icons"); }
            try { RailgunnerIcons(); } catch { Log.LogError("Failed to load Railgunner icons"); }
            try { if (classicItemsLoaded) ScepterIcons(); } catch { Log.LogError("Failed to load Scepter icons"); }
            Log.LogMessage("Sprites loaded");
        }
        internal static void CommandoIcons()
        {
            //Shotgun
            Texture2D shotgunIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_commandoshotgun");
            shotgunIconS = EggAssets.TexToSprite(shotgunIcon);
            //Dash
            Texture2D dashIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_dash");
            dashIconS = EggAssets.TexToSprite(dashIcon);
        }
        internal static void ArtificerIcons()
        {
            //Zapport
            Texture2D zapportIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_surgeteleport");
            zapportIconS = EggAssets.TexToSprite(zapportIcon);
        }
        internal static void BanditIcons()
        {
            //InvisSprint
            Texture2D invisSprintIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_invissprint");
            invisSprintIconS = EggAssets.TexToSprite(invisSprintIcon);
            //MagicBullet
            Texture2D magicBulletIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_richochet");
            magicBulletIconS = EggAssets.TexToSprite(magicBulletIcon);
        }
        internal static void RexIcons()
        {
            //Respire
            Texture2D rexrootIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_rexroot");
            rexrootIconS = EggAssets.TexToSprite(rexrootIcon);
        }
        internal static void AcridIcons()
        {
            //Poison Breath
            Texture2D acridpoisonbreathIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_poisonbreath");
            acridpoisonbreathIconS = EggAssets.TexToSprite(acridpoisonbreathIcon);
            //Expunge
            Texture2D acridpurgeIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_expunge");
            acridpurgeIconS = EggAssets.TexToSprite(acridpurgeIcon);
        }
        internal static void LoaderIcons()
        {
            //Shieldsplosion
            Texture2D shieldSplosionIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_shieldsplosion");
            shieldsplosionIconS = EggAssets.TexToSprite(shieldSplosionIcon);
        }
        internal static void CaptainIcons()
        {
            //Auto shotgun
            Texture2D autoshotgunIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_minigun");
            autoshotgunIconS = EggAssets.TexToSprite(autoshotgunIcon);
            //DebuffGrenade
            Texture2D debuffNadeIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_debuffnade");
            debuffNadeIconS = EggAssets.TexToSprite(debuffNadeIcon);
        }
        internal static void HuntressIcons()
        {
            //ClusterArrow
            Texture2D clusterarrowIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_bombarrow");
            clusterarrowIconS = EggAssets.TexToSprite(clusterarrowIcon);
        }
        internal static void MercIcons()
        {
            //Slashport
            Texture2D slashportIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_fatalteleport");
            slashportIconS = EggAssets.TexToSprite(slashportIcon);
        }
        internal static void EngiIcons()
        {
            //Micromissiles
            Texture2D micromissileIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_micromissiles");
            micromissileIconS = EggAssets.TexToSprite(micromissileIcon);
            //TeslaMines
            Texture2D teslaMineIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_teslamine");
            teslaMineIconS = EggAssets.TexToSprite(teslaMineIcon);
        }
        internal static void MulTIcons()
        {
            //Nanobots
            Texture2D nanoBotsIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_nanobots");
            nanoBotsIconS = EggAssets.TexToSprite(nanoBotsIcon);
        }
        internal static void VoidFiendIcons()
        {
            //Inversion
            Texture2D inversionIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_inversionalt");
            inversionIconS = EggAssets.TexToSprite(inversionIcon);
            Texture2D inversionAltIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_inversion");
            inversionAltIconS = EggAssets.TexToSprite(inversionAltIcon);
        }

        internal static void RailgunnerIcons()
        {
            //Lance rounds
            Texture2D lanceroundsIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_lancerounds");
            lanceroundsIconS = EggAssets.TexToSprite(lanceroundsIcon);
        }

        internal static void ScepterIcons()
        {
            //Slashport Upgrade
            Texture2D slashportUpgradedIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_fatalteleport_upgraded");
            slashportUpgradedIconS = EggAssets.TexToSprite(slashportUpgradedIcon);

            //Rexroot Upgrade
            Texture2D rexrootUpgradedIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_rexroot_upgraded");
            rexrootUpgradedIconS = EggAssets.TexToSprite(rexrootUpgradedIcon);

            //Acridpurge Upgrade
            Texture2D acridpurgeIcon = SkillsAssets.assetBundle.LoadAsset<Texture2D>("icon_expunge_upgraded");
            acridpurgeUpgradedIconS = EggAssets.TexToSprite(acridpurgeIcon);
        }
    }
}
