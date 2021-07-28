using R2API;
using EggsSkills.Config;
using System;
using IL.RoR2;

namespace EggsSkills.Resources
{
    class LanguageTokens
    {
        internal static void RegisterLanguageTokens()
        {
            ArtificerTokens();
            HuntressTokens();
            BanditTokens();
            CaptainTokens();
            AcridTokens();
            LoaderTokens();
            RexTokens();
            CommandoTokens();
            MercTokens();
            EngiTokens();
            MulTTokens();
            EggsUtils.EggsUtils.LogToConsole("Language tokens loaded");
        }
        internal static void ArtificerTokens()
        {
            //Zapport
            LanguageAPI.Add("ARTIFICER_UTILITY_ZAPPORT_NAME", "Quantum Transposition");
            LanguageAPI.Add("ARTIFICER_UTILITY_ZAPPORT_DESC", "<style=cIsDamage>Stunning.</style> Charge to  <style=cIsUtility>teleport</style> forward, dealing <style=cIsDamage>250%-1000% damage</style> to enemies near target location.");

            LanguageAPI.Add("ARTIFICER_FTLUNLOCKABLE_ACHIEVEMENT_NAME", "Artificer: FTL");
            LanguageAPI.Add("ARTIFICER_FTLUNLOCKABLE_ACHIEVEMENT_DESC", "As Artificer, reach 500% movespeed.");
            LanguageAPI.Add("ARTIFICER_FTLUNLOCKABLE_UNLOCKABLE_NAME", "Artificer: FTL");
            LanguageAPI.Add("ARTIFICER_FTLUNLOCKABLE_REWARD_ID", "5XMoveSpeed");
            LanguageAPI.Add("ARTIFICER_FTLUNLOCKABLE_ACHIEVEMENT_ID", "5XMoveSpeed");
        }
        internal static void HuntressTokens()
        {
            //Explosive Arrow
            LanguageAPI.Add("HUNTRESS_SECONDARY_CLUSTERARROW_NAME", "Explosive Arrow");
            LanguageAPI.Add("HUNTRESS_SECONDARY_CLUSTERARROW_DESC", "<style=cIsUtility>Agile.</style> Fire an arrow that <style=cIsDamage>explodes</style> on impact, dealing <style=cIsDamage>500% Damage</style> and releasing <style=cIsDamage>" + Configuration.GetConfigValue<int>(Configuration.HuntressArrowBomblets) + " bomblets</style> that deal <style=cIsDamage>80% Damage each</style>.  Critical strikes instead release <style=cIsDamage>" + (Math.Floor(Configuration.GetConfigValue<int>(Configuration.HuntressArrowBomblets) * 1.5f)) + "</style>");

            LanguageAPI.Add("HUNTRESS_TRADITIONALUNLOCKABLE_ACHIEVEMENT_NAME", "Huntress: Traditional");
            LanguageAPI.Add("HUNTRESS_TRADITIONALUNLOCKABLE_ACHIEVEMENT_DESC", "As Huntress, complete a teleporter event without using your secondary or special skills");
            LanguageAPI.Add("HUNTRESS_TRADITIONALUNLOCKABLE_UNLOCKABLE_NAME", "Huntress: Traditional");
            LanguageAPI.Add("HUNTRESS_TRADITIONALUNLOCKABLE_REWARD_ID", "NoSecondarySpecial");
            LanguageAPI.Add("HUNTRESS_TRADITIONALUNLOCKABLE_ACHIEVEMENT_ID", "NoSecondarySpecial");
        }
        internal static void BanditTokens()
        {
            //Sprint Invis
            LanguageAPI.Add("BANDIT_UTILITY_INVISSPRINT_NAME", "Kinetic Refractor");
            LanguageAPI.Add("BANDIT_UTILITY_INVISSPRINT_DESC", "Passively become <style=cIsUtility>invisible while sprinting</style>.  Gain a burst of <style=cIsUtility>movement speed and increased damage</style> upon leaving <style=cIsUtility>invisibility</style>.");

            LanguageAPI.Add("BANDIT_FLANKEDUNLOCKABLE_ACHIEVEMENT_NAME", "Bandit: Flanked");
            LanguageAPI.Add("BANDIT_FLANKEDUNLOCKABLE_ACHIEVEMENT_DESC", "As Bandit, remain invisible for a total of 3 minutes in a run");
            LanguageAPI.Add("BANDIT_FLANKEDUNLOCKABLE_UNLOCKABLE_NAME", "Bandit: Flanked");
            LanguageAPI.Add("BANDIT_FLANKEDUNLOCKABLE_REWARD_ID", "Invis3Min");
            LanguageAPI.Add("BANDIT_FLANKEDUNLOCKABLE_ACHIEVEMENT_ID", "Invis3Min");

            //Magic Bullet
            LanguageAPI.Add("BANDIT_PRIMARY_MAGICBULLET_NAME", "Magic Bullet");
            LanguageAPI.Add("BANDIT_PRIMARY_MAGICBULLET_DESC", "Fire a bullet that deals <style=cIsDamage>200% damage</style>, and <style=cIsUtility>ricochets</style> to 1 nearby enemy for half damage.  Critical strikes are <style=cIsUtility>lucky: 1</style>");

            LanguageAPI.Add("BANDIT_HATUNLOCKABLE_ACHIEVEMENT_NAME", "Bandit: Hat trick");
            LanguageAPI.Add("BANDIT_HATUNLOCKABLE_ACHIEVEMENT_DESC", "As Bandit, kill 3 enemies in under 1 second");
            LanguageAPI.Add("BANDIT_HATUNLOCKABLE_UNLOCKABLE_NAME", "Bandit: Hat trick");
            LanguageAPI.Add("BANDIT_HATUNLOCKABLE_REWARD_ID", "3FastKill");
            LanguageAPI.Add("BANDIT_HATUNLOCKABLE_ACHIEVEMENT_ID", "3FastKill");
        }
        internal static void CaptainTokens()
        {
            //Captain
            LanguageAPI.Add("CAPTAIN_SECONDARY_DEBUFFNADE_NAME", "MK-4 Tracking Grenade");
            LanguageAPI.Add("CAPTAIN_SECONDARY_DEBUFFNADE_DESC", "Launch a grenade that deals <style=cIsDamage>250% Damage</style> and inflicts <style=cArtifact>tracking</style> on enemies in a wide area for <style=cIsDamage>5 seconds</style>");

            LanguageAPI.Add("CAPTAIN_SUPPORTUNLOCKABLE_ACHIEVEMENT_NAME", "Captain: My own backup");
            LanguageAPI.Add("CAPTAIN_SUPPORTUNLOCKABLE_ACHIEVEMENT_DESC", "As Captain, complete a stage past stage 3 without having deployed any orbital beacons");
            LanguageAPI.Add("CAPTAIN_SUPPORTUNLOCKABLE_UNLOCKABLE_NAME", "Captain: My own backup");
            LanguageAPI.Add("CAPTAIN_SUPPORTUNLOCKABLE_REWARD_ID", "NoBeacons");
            LanguageAPI.Add("CAPTAIN_SUPPORTUNLOCKABLE_ACHIEVEMENT_ID", "NoBeacons");
        }
        internal static void AcridTokens()
        {
            //Expunge
            LanguageAPI.Add("ACRID_SPECIAL_PURGE_NAME", "Expunge");
            LanguageAPI.Add("ACRID_SPECIAL_PURGE_DESC", "Inflict damage on all <style=cIsHealing>poisoned</style> or <style=cIsDamage>blighted</style> enemies in range, dealing either <style=cIsHealing>200% + 10% of their max health</style> or <style=cIsDamage>300% per stack</style> in an area around them, depending on <style=cIsUtility>passive selection.</style>");

            LanguageAPI.Add("ACRID_CUREUNLOCKABLE_ACHIEVEMENT_NAME", "Acrid: The cure");
            LanguageAPI.Add("ACRID_CUREUNLOCKABLE_ACHIEVEMENT_DESC", "As Acrid, have 20 enemies poisoned or blighted at once");
            LanguageAPI.Add("ACRID_CUREUNLOCKABLE_UNLOCKABLE_NAME", "Acrid: The cure");
            LanguageAPI.Add("ACRID_CUREUNLOCKABLE_REWARD_ID", "20Poisoned");
            LanguageAPI.Add("ACRID_CUREUNLOCKABLE_ACHIEVEMENT_ID", "20Poisoned");
        }
        internal static void LoaderTokens()
        {
            //Shieldsplosion
            LanguageAPI.Add("LOADER_SPECIAL_SHIELDSPLOSION_NAME", "Barrier Buster");
            LanguageAPI.Add("LOADER_SPECIAL_SHIELDSPLOSION_DESC", "Consume all of your current <style=cIsHealing>Barrier (Minimum 10%)</style> to gain a burst of <style=cIsUtility>Movement Speed</style> and deal <style=cIsDamage>600-6000% damage</style> around you based on <style=cIsHealing>Barrier</style> consumed.");

            LanguageAPI.Add("LOADER_BARRIERUNLOCKABLE_ACHIEVEMENT_NAME", "Loader: Overcharged");
            LanguageAPI.Add("LOADER_BARRIERUNLOCKABLE_ACHIEVEMENT_DESC", "As Loader, reach 95% barrier.");
            LanguageAPI.Add("LOADER_BARRIERUNLOCKABLE_UNLOCKABLE_NAME", "Loader: Overcharged");
            LanguageAPI.Add("LOADER_BARRIERUNLOCKABLE_REWARD_ID", "HighBarrier");
            LanguageAPI.Add("LOADER_BARRIERUNLOCKABLE_ACHIEVEMENT_ID", "HighBarrier");
        }
        internal static void RexTokens()
        {
            //Respire
            LanguageAPI.Add("REX_SPECIAL_ROOT_NAME", "DIRECTIVE: Respire");
            LanguageAPI.Add("REX_SPECIAL_ROOT_DESC", "<style=cIsDamage>Stunning.</style> <style=cIsUtility>Slow</style> yourself, but gain <style=cIsUtility>adaptive</style> for up to 8 seconds.  While active, deal <style=cIsDamage>250% damage</style> per second to nearby enemies, gaining <style=cIsHealing>barrier</style> per enemy hit and <style=cIsDamage>pulling them towards you</style>.");

            LanguageAPI.Add("REX_BREATHINGUNLOCKABLE_ACHIEVEMENT_NAME", "REX: Breathing room");
            LanguageAPI.Add("REX_BREATHINGUNLOCKABLE_ACHIEVEMENT_DESC", "As REX, kill 100 enemies in close range in a single run");
            LanguageAPI.Add("REX_BREATHINGUNLOCKABLE_UNLOCKABLE_NAME", "Rex: Breathing room");
            LanguageAPI.Add("REX_BREATHINGUNLOCKABLE_REWARD_ID", "Kill100Close");
            LanguageAPI.Add("REX_BREATHINGUNLOCKABLE_ACHIEVEMENT_ID", "Kill100Close");
        }
        internal static void EngiTokens()
        {
            //Tesla Mine
            LanguageAPI.Add("ENGI_SECONDARY_TESLAMINE_NAME", "T-3514 Shock Mines");
            LanguageAPI.Add("ENGI_SECONDARY_TESLAMINE_DESC", "<style=cIsDamage>Stunning.</style> Place a shock mine, that upon detonation deals <style=cIsDamage>200% damage</style> and leaves a lingering zone for <style=cIsDamage>" + (Configuration.GetConfigValue<int>(Configuration.EngiTeslaminePulses) - 1) + " seconds</style> that deals <style=cIsDamage>200% damage each second</style>.  Can place up to <style=cIsDamage>4</style>.");

            LanguageAPI.Add("ENGI_ELECTRICUNLOCKABLE_ACHIEVEMENT_NAME", "Engineer: Electric Boogaloo");
            LanguageAPI.Add("ENGI_ELECTRICUNLOCKABLE_ACHIEVEMENT_DESC", "As Engineer, have 4 different electric items at once.");
            LanguageAPI.Add("ENGI_ELECTRICUNLOCKABLE_UNLOCKABLE_NAME", "Engineer: Electric Boogaloo");
            LanguageAPI.Add("ENGI_ELECTRICUNLOCKABLE_REWARD_ID", "4ElectricItems");
            LanguageAPI.Add("ENGI_ELECTRICUNLOCKABLE_ACHIEVEMENT_ID", "4ElectricItems");
        }
        internal static void CommandoTokens()
        {
            //Shotgun
            LanguageAPI.Add("COMMANDO_PRIMARY_COMBATSHOTGUN_NAME", "Flechette Rounds");
            LanguageAPI.Add("COMMANDO_PRIMARY_COMBATSHOTGUN_DESC", "Fire flechette rounds, dealing <style=cIsDamage>" + Configuration.GetConfigValue<uint>(Configuration.CommandoShotgunPellets) + "x60% damage</style> in a wider but shorter range.  <style=cIsUtility>Spread</style> decreases on <style=cIsDamage>critical strikes</style>.");

            LanguageAPI.Add("COMMANDO_BULLETUNLOCKABLE_ACHIEVEMENT_NAME", "Commando: 65% More Bullet");
            LanguageAPI.Add("COMMANDO_BULLETUNLOCKABLE_ACHIEVEMENT_DESC", "As Commando, kill 20 enemies in a row without releasing primary fire");
            LanguageAPI.Add("COMMANDO_BULLETUNLOCKABLE_UNLOCKABLE_NAME", "Commando: 65% More Bullet");
            LanguageAPI.Add("COMMANDO_BULLETUNLOCKABLE_REWARD_ID", "20KillSpree");
            LanguageAPI.Add("COMMANDO_BULLETUNLOCKABLE_ACHIEVEMENT_ID", "20KillSpree");

            //Dash
            LanguageAPI.Add("COMMANDO_UTILITY_DASH_NAME", "Tactical Pursuit");
            LanguageAPI.Add("COMMANDO_UTILITY_DASH_DESC", "<style=cIsDamage>Prepare: Secondary.</style> <style=cIsUtility>Dash</style> a short distance and become <style=cIsDamage>invulnerable</style> during and for a short period afterwards");

            LanguageAPI.Add("COMMANDO_PERSEVEREUNLOCKABLE_ACHIEVEMENT_NAME", "Commando: Perseverence");
            LanguageAPI.Add("COMMANDO_PERSEVEREUNLOCKABLE_ACHIEVEMENT_DESC", "As Commando, finish a teleporter event under 20% health");
            LanguageAPI.Add("COMMANDO_PERSEVEREUNLOCKABLE_UNLOCKABLE_NAME", "Commando: Perseverence");
            LanguageAPI.Add("COMMANDO_PERSEVEREUNLOCKABLE_REWARD_ID", "CLowHealthTeleporter");
            LanguageAPI.Add("COMMANDO_PERSEVEREUNLOCKABLE_ACHIEVEMENT_ID", "CLowHealthTeleporter");
        }
        internal static void MercTokens()
        {
            //Slashport
            LanguageAPI.Add("MERCENARY_UTILITY_SLASHPORT_NAME", "Fatal Assault");
            LanguageAPI.Add("MERCENARY_UTILITY_SLASHPORT_DESC", "<style=cIsDamage>Stunning.</style> Target an enemy to <style=cIsUtility>expose</style>, <style=cIsUtility>teleport to</style> and strike them for <style=cIsDamage>600% damage, plus " + (Configuration.GetConfigValue<float>(Configuration.MercSlashHealthfraction) * 100) + "% of their missing health</style>.");

            LanguageAPI.Add("MERC_CULLUNLOCKABLE_ACHIEVEMENT_NAME", "Mercenary: Culled");
            LanguageAPI.Add("MERC_CULLUNLOCKABLE_ACHIEVEMENT_DESC", "As Mercenary, Strike 10 unique exposed enemies with no more than 5 seconds between each strike");
            LanguageAPI.Add("MERC_CULLUNLOCKABLE_UNLOCKABLE_NAME", "Mercenary: Culled");
            LanguageAPI.Add("MERC_CULLUNLOCKABLE_REWARD_ID", "20ExposedEnemieStrikes");
            LanguageAPI.Add("MERC_CULLUNLOCKABLE_ACHIEVEMENT_ID", "20ExposedEnemieStrikes");
        }
        internal static void MulTTokens()
        {
            //Nanobots
            LanguageAPI.Add("MULT_SECONDARY_NANOBOT_NAME", "Nanobot Swarm");
            LanguageAPI.Add("MULT_SECONDARY_NANOBOT_DESC", "Fire a <style=cIsUtility>beacon</style> that deals <style=cIsDamage>100% damage</style> and inflicts <style=cArtifact>tracking</style> on impact.  After a delay, release <style=cIsDamage>nanobot swarms</style> for each nearby enemy that deal <style=cIsDamage>" + Configuration.GetConfigValue<int>(Configuration.ToolbotNanobotCountperenemy) + "x80% damage</style> and <style=cIsHealing>heal for 1.5% max hp each</style>");

            LanguageAPI.Add("MULT_MOTHERSHIPUNLOCKABLE_ACHIEVEMENT_NAME", "MUL-T: Mothership");
            LanguageAPI.Add("MULT_MOTHERSHIPUNLOCKABLE_ACHIEVEMENT_DESC", "As MUL-T, have 8 drone followers at once");
            LanguageAPI.Add("MULT_MOTHERSHIPUNLOCKABLE_UNLOCKABLE_NAME", "MUL-T: Mothership");
            LanguageAPI.Add("MULT_MOTHERSHIPUNLOCKABLE_REWARD_ID", "8Drones");
            LanguageAPI.Add("MULT_MOTHERSHIPUNLOCKABLE_ACHIEVEMENT_ID", "8Drones");
        }
    }
}
