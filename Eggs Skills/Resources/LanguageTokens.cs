using R2API;
using EggsSkills.Config;
using System;
using static EggsSkills.SkillsLoader;
using EggsSkills.Achievements;

namespace EggsSkills.Resources
{
    internal class LanguageTokens
    {
        public static readonly string prefix = "ES_";
        public static readonly string ach_prefix = "ACHIEVEMENT_" + prefix;
        public static readonly string nSuffix = "_NAME";
        public static readonly string dSuffix = "_DESCRIPTION";

        internal static void RegisterLanguageTokens()
        {
            //Artificer language tokens
            ArtificerTokens();
            //Huntress language tokens
            HuntressTokens();
            //Bandit language tokens
            BanditTokens();
            //Captain language tokens
            CaptainTokens();
            //Acrid language tokens
            AcridTokens();
            //Loader language tokens
            LoaderTokens();
            //REX language tokens
            RexTokens();
            //Commando language tokens
            CommandoTokens();
            //Mercenary language tokens
            MercTokens();
            //Engineer language tokens
            EngiTokens();
            //MUL-T language tokens
            MulTTokens();
            //Announce if all goes through as planned
            EggsUtils.EggsUtils.LogToConsole("Language tokens loaded");
        }

        internal static void AcridTokens()
        {
            //Expunge
            LanguageAPI.Add(prefix + acridName.ToUpper() + "_" + "SPECIAL_PURGE" + nSuffix, "Expunge");
            LanguageAPI.Add(prefix + acridName.ToUpper() + "_" + "SPECIAL_PURGE" + dSuffix, "Inflict damage on all <style=cIsHealing>poisoned</style> or <style=cIsDamage>blighted</style> enemies in range, dealing either <style=cIsHealing>200% + 10% of their max health</style> or <style=cIsDamage>300% per stack</style> in an area around them, depending on <style=cIsUtility>passive selection.</style>");

            LanguageAPI.Add(ach_prefix + PurgeAchievement.ACHNAME.ToUpper() + nSuffix, "Acrid: The cure");
            LanguageAPI.Add(ach_prefix + PurgeAchievement.ACHNAME.ToUpper() + dSuffix, "As Acrid, have 20 enemies poisoned or blighted at once");
        }

        internal static void ArtificerTokens()
        {
            //Zapport
            LanguageAPI.Add(prefix + artificerName.ToUpper() + "_" + "UTILITY_ZAPPORT" + nSuffix, "Quantum Transposition");
            LanguageAPI.Add(prefix + artificerName.ToUpper() + "_" + "UTILITY_ZAPPORT" + dSuffix, "<style=cIsDamage>Stunning. Overloading.</style> Charge to <style=cIsUtility>teleport</style> forward, dealing <style=cIsDamage>250%-1000% damage</style> to enemies near target location.");

            LanguageAPI.Add(ach_prefix + ZapportAchievement.ACHNAME.ToUpper() + nSuffix, "Artificer: FTL");
            LanguageAPI.Add(ach_prefix + ZapportAchievement.ACHNAME.ToUpper() + dSuffix, "As Artificer, reach +500% movespeed.");
        }

        internal static void BanditTokens()
        {
            //Sprint Invis
            LanguageAPI.Add(prefix + banditName.ToUpper() + "_" + "UTILITY_INVISSPRINT" + nSuffix, "Kinetic Refractor");
            LanguageAPI.Add(prefix + banditName.ToUpper() + "_" + "UTILITY_INVISSPRINT" + dSuffix, "Passively become <style=cIsUtility>invisible while sprinting</style>.  Gain a burst of <style=cIsUtility>movement speed and increased damage</style> upon leaving <style=cIsUtility>invisibility</style>.");

            LanguageAPI.Add(ach_prefix + InvisSprintAchievement.ACHNAME.ToUpper() + nSuffix, "Bandit: Flanked");
            LanguageAPI.Add(ach_prefix + InvisSprintAchievement.ACHNAME.ToUpper() + dSuffix, "As Bandit, remain invisible for a total of 3 minutes in a run");

            //Magic Bullet
            LanguageAPI.Add(prefix + banditName.ToUpper() + "_" + "PRIMARY_MAGICBULLET" + nSuffix, "Bounce");
            LanguageAPI.Add(prefix + banditName.ToUpper() + "_" + "PRIMARY_MAGICBULLET" + dSuffix, "Fire a bullet that deals <style=cIsDamage>200% damage</style>, and <style=cIsUtility>ricochets</style> to " + Configuration.GetConfigValue(Configuration.BanditMagicbulletRicochets) + " nearby enem" + ((Configuration.GetConfigValue(Configuration.BanditMagicbulletRicochets) > 1) ? "ies" : "y") + " for <style=cIsDamage>60% of the previous shot's damage</style>.");

            LanguageAPI.Add(ach_prefix + MagicBulletAchievement.ACHNAME.ToUpper() + nSuffix, "Bandit: Hat trick");
            LanguageAPI.Add(ach_prefix + MagicBulletAchievement.ACHNAME.ToUpper() + dSuffix, "As Bandit, kill 3 enemies in under 1 second");
        }

        internal static void CaptainTokens()
        {
            //Captain
            LanguageAPI.Add(prefix + captainName.ToUpper() + "_" + "SECONDARY_DEBUFFNADE" + nSuffix, "MK-4 Tracking Grenade");
            LanguageAPI.Add(prefix + captainName.ToUpper() + "_" + "SECONDARY_DEBUFFNADE" + dSuffix, "Launch a grenade that deals <style=cIsDamage>250% damage</style> and inflicts <style=cArtifact>tracking</style> on enemies in a wide area for <style=cIsDamage>5 seconds</style>");

            LanguageAPI.Add(ach_prefix + DebuffnadeAchievement.ACHNAME.ToUpper() + nSuffix, "Captain: My own backup");
            LanguageAPI.Add(ach_prefix + DebuffnadeAchievement.ACHNAME.ToUpper() + dSuffix, "As Captain, complete a stage past stage 3 without having deployed any orbital beacons");
        }

        internal static void CommandoTokens()
        {
            //Shotgun
            LanguageAPI.Add(prefix + commandoName.ToUpper() + "_" + "PRIMARY_COMBATSHOTGUN" + nSuffix, "Flechette Rounds");
            LanguageAPI.Add(prefix + commandoName.ToUpper() + "_" + "PRIMARY_COMBATSHOTGUN" + dSuffix, "Fire flechette rounds, dealing <style=cIsDamage>" + Configuration.GetConfigValue(Configuration.CommandoShotgunPellets) + "x60% damage</style> in a wider but shorter range.  <style=cIsUtility>Spread</style> decreases on <style=cIsDamage>critical strikes</style>.");

            LanguageAPI.Add(ach_prefix + ShotgunAchievement.ACHNAME.ToUpper() + nSuffix, "Commando: 65% More Bullet");
            LanguageAPI.Add(ach_prefix + ShotgunAchievement.ACHNAME.ToUpper() + dSuffix, "As Commando, kill 20 enemies in a row without releasing primary fire");

            //Dash
            LanguageAPI.Add(prefix + commandoName.ToUpper() + "_" + "UTILITY_DASH" + nSuffix, "Tactical Pursuit");
            LanguageAPI.Add(prefix + commandoName.ToUpper() + "_" + "UTILITY_DASH" + dSuffix, "<style=cIsUtility>Prepare: Secondary. Agile. Dash</style> a short distance and become <style=cIsDamage>invulnerable</style> during and for a short period afterwards");

            LanguageAPI.Add(ach_prefix + DashAchievement.ACHNAME.ToUpper() + nSuffix, "Commando: Perseverence");
            LanguageAPI.Add(ach_prefix + DashAchievement.ACHNAME.ToUpper() + dSuffix, "As Commando, finish a teleporter event under 20% health");
        }

        internal static void EngiTokens()
        {
            //Tesla Mine
            LanguageAPI.Add(prefix + engineerName.ToUpper() + "_" + "SECONDARY_TESLAMINE" + nSuffix, "T-3514 Shock Mines");
            LanguageAPI.Add(prefix + engineerName.ToUpper() + "_" + "SECONDARY_TESLAMINE" + dSuffix, "<style=cIsDamage>Stunning.</style> Place a shock mine, that upon detonation deals <style=cIsDamage>200% damage</style> and leaves a lingering zone for <style=cIsDamage>" + (Configuration.GetConfigValue(Configuration.EngiTeslaminePulses) - 1) + " seconds</style> that deals <style=cIsDamage>200% damage each second</style>.  Can place up to <style=cIsDamage>4</style>.");

            LanguageAPI.Add(ach_prefix + TeslaMineAchievement.ACHNAME.ToUpper() + nSuffix, "Engineer: Electric Boogaloo");
            LanguageAPI.Add(ach_prefix + TeslaMineAchievement.ACHNAME.ToUpper() + dSuffix, "As Engineer, have 4 different electric items at once.");
        }

        internal static void HuntressTokens()
        {
            //Explosive Arrow
            LanguageAPI.Add(prefix + huntressName.ToUpper() + "_" + "SECONDARY_CLUSTERARROW" + nSuffix, "Explosive Arrow");
            LanguageAPI.Add(prefix + huntressName.ToUpper() + "_" + "SECONDARY_CLUSTERARROW" + dSuffix, "<style=cIsUtility>Agile.</style> Fire an arrow that <style=cIsDamage>explodes</style> on impact, dealing <style=cIsDamage>500% damage</style> and releasing <style=cIsDamage>" + Configuration.GetConfigValue(Configuration.HuntressArrowBomblets) + " bomblets</style> that deal <style=cIsDamage>80% damage each</style>.  Critical strikes instead release <style=cIsDamage>" + (Math.Floor(Configuration.GetConfigValue(Configuration.HuntressArrowBomblets) * 1.5f)) + "</style>");

            LanguageAPI.Add(ach_prefix + BombArrowAchievement.ACHNAME.ToUpper() + nSuffix, "Huntress: Traditional");
            LanguageAPI.Add(ach_prefix + BombArrowAchievement.ACHNAME.ToUpper() + dSuffix, "As Huntress, complete a teleporter event without using your secondary or special skills");
        }

        internal static void LoaderTokens()
        {
            //Shieldsplosion
            LanguageAPI.Add(prefix + loaderName.ToUpper() + "_" + "SPECIAL_SHIELDSPLOSION" + nSuffix, "Barrier Buster");
            LanguageAPI.Add(prefix + loaderName.ToUpper() + "_" + "SPECIAL_SHIELDSPLOSION" + dSuffix, "Consume all of your current <style=cIsHealing>Barrier (Minimum 10%)</style> to gain a burst of <style=cIsUtility>Movement Speed</style> and deal <style=cIsDamage>600-6000% damage</style> around you based on <style=cIsHealing>Barrier</style> consumed.");

            LanguageAPI.Add(ach_prefix + BarrierAchievement.ACHNAME.ToUpper() + nSuffix, "Loader: Overcharged");
            LanguageAPI.Add(ach_prefix + BarrierAchievement.ACHNAME.ToUpper() + dSuffix, "As Loader, reach 95% barrier.");
        }

        internal static void MercTokens()
        {
            //Slashport
            LanguageAPI.Add(prefix + mercenaryName.ToUpper() + "_" + "SPECIAL_SLASHPORT" + nSuffix, "Execute");
            LanguageAPI.Add(prefix + mercenaryName.ToUpper() + "_" + "SPECIAL_SLASHPORT" + dSuffix, "<style=cIsDamage>Slayer</style>. Target an enemy to <style=cIsUtility>expose, teleport to</style> and <style=cIsDamage>strike</style> them for <style=cIsDamage>700% damage</style>, <style=cIsUtility>stunning</style> nearby enemies.");

            LanguageAPI.Add(ach_prefix + SlashportAchievement.ACHNAME.ToUpper() + nSuffix, "Mercenary: Culled");
            LanguageAPI.Add(ach_prefix + SlashportAchievement.ACHNAME.ToUpper() + dSuffix, "As Mercenary, strike 10 unique exposed enemies with no more than 5 seconds between each strike");
        }

        internal static void MulTTokens()
        {
            //Nanobots
            LanguageAPI.Add(prefix + multName.ToUpper() + "_" + "SECONDARY_NANOBOT" + nSuffix, "Nanobot Swarm");
            LanguageAPI.Add(prefix + multName.ToUpper() + "_" + "SECONDARY_NANOBOT" + dSuffix, "Fire a <style=cIsUtility>beacon</style> that deals <style=cIsDamage>100% damage</style> and inflicts <style=cArtifact>tracking</style> on impact.  After a delay, release <style=cIsDamage>nanobot swarms</style> for each nearby enemy that deal <style=cIsDamage>" + Configuration.GetConfigValue(Configuration.ToolbotNanobotCountperenemy) + "x80% damage</style> and <style=cIsHealing>heal for 1.5% max hp each</style>");

            LanguageAPI.Add(ach_prefix + NanoBotAchievement.ACHNAME.ToUpper() + nSuffix, "MUL-T: Mothership");
            LanguageAPI.Add(ach_prefix + NanoBotAchievement.ACHNAME.ToUpper() + dSuffix, "As MUL-T, have 8 drone followers at once");
        }

        internal static void RexTokens()
        {
            //Respire
            LanguageAPI.Add(prefix + rexName.ToUpper() + "_" + "SPECIAL_ROOT" + nSuffix, "DIRECTIVE: Respire");
            LanguageAPI.Add(prefix + rexName.ToUpper() + "_" + "SPECIAL_ROOT" + dSuffix, "<style=cIsDamage>Stunning.</style> <style=cIsUtility>Slow</style> yourself, but gain <style=cIsUtility>adaptive</style> for up to 8 seconds.  While active, deal <style=cIsDamage>250% damage</style> per second to nearby enemies, gaining <style=cIsHealing>barrier</style> per enemy hit and <style=cIsDamage>pulling them towards you</style>.");

            LanguageAPI.Add(ach_prefix + RootAchievement.ACHNAME.ToUpper() + nSuffix, "REX: Breathing Room");
            LanguageAPI.Add(ach_prefix + RootAchievement.ACHNAME.ToUpper() + dSuffix, "As REX, kill 100 enemies in close range in a single run");
        }
    }
}
