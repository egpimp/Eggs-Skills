using EggsSkills.EntityStates;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using EggsUtils.Buffs;
using EggsSkills.EntityStates.TeslaMine.MineStates.MainStates;
using UnityEngine.Networking;

namespace EggsSkills.SkillModifiers
{
    [SkillLevelModifier("ESPurge", typeof(AcridPurgeEntity))]
    class AcridPurgeModifier : SimpleSkillModifier<AcridPurgeEntity>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            //+20% radius per level
            AcridPurgeEntity.spp_radiusMult = 1 + level * 0.2f;
            //+10% damage per level
            AcridPurgeEntity.spp_damageMult = 1 + level * 0.1f;
        }
    }

    [SkillLevelModifier("ESZapport", typeof(ZapportChargeEntity))]
    class ArtificerZapportModifier : SimpleSkillModifier<ZapportChargeEntity>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            //+20% bonus damage per level
            ZapportChargeEntity.spp_damageMult = 1f + level * 0.2f;
            //+10% bonus distance per level
            ZapportChargeEntity.spp_distanceMult = 1f + level * 0.1f;
        }
    }

    [SkillLevelModifier("ESInvisSprint", typeof(InvisDummyState))]
    class BanditInvisSprintModifier : SimpleSkillModifier<InvisDummyState>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            InvisHandler component = characterBody.GetComponent<InvisHandler>();
            if (component != null)
            {
                //+10% move speed per level
                component.spp_moveSpeedBonus = 1 + level * 0.1f;
                //+4 radius stun on exit
                component.spp_stunRadius = 4 * level;
            }
        }
    }

    [SkillLevelModifier("ESMagicBullet", typeof(MagicBulletEntity))]
    class BanditMagicBulletModifier: SimpleSkillModifier<MagicBulletEntity>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            //+7.5% bounce modifier per level
            MagicBulletEntity.spp_bounceMod = level * 0.05f;
            //+1 bounce per 2 levels
            MagicBulletEntity.spp_richochetMod = level / 2;
        }
    }

    [SkillLevelModifier("ESDebuffNade", typeof(DebuffGrenadeEntity))]
    class CaptainDebuffNadeModifier : SimpleSkillModifier<DebuffGrenadeEntity>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            //+20% Damage
            DebuffGrenadeEntity.spp_damageMult = 1 + level * 0.2f;
            //+1 stock
            skillDef.baseMaxStock = 1 + level / 2;
        }
    }

    [SkillLevelModifier("ESCombatShotgun", typeof(CombatShotgunEntity))]
    class CommandoCombatShotgunModifier : SimpleSkillModifier<CombatShotgunEntity>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            //+0.1 proc chance per level
            CombatShotgunEntity.spp_procMod = level * 0.1f;
            //+1 Bullet per 2 levels
            CombatShotgunEntity.spp_bulletMod = (uint)level / 2;
        }
    }

    [SkillLevelModifier("ESDash", typeof(CommandoDashEntity))]
    class CommandoDashModifier : SimpleSkillModifier<CommandoDashEntity>
    {
        public override void OnSkillExit(CommandoDashEntity skillState, int level)
        {
            base.OnSkillExit(skillState, level);
            //Apply -level- stacks of 10% damage buff
            for(int i = 0; i < level; i++)
            {
                if(NetworkServer.active) skillState.characterBody.AddTimedBuff(BuffsLoading.buffDefStackingDamage, 2);
            }
        }
    }

    [SkillLevelModifier("ESTeslaMine", typeof(TeslaMineFireState), typeof(TeslaWaitForTargetState) ,typeof(TeslaDetonateState))]
    class EngiTeslaMineModifier : SimpleSkillModifier<TeslaDetonateState>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            //+10% radius
            TeslaDetonateState.spp_radiusMult = TeslaWaitForTargetState.spp_radiusMult = 1 + level * 0.1f;
            //+10% damage
            TeslaDetonateState.spp_damageMult = 1 + level * 0.1f;
            //+1 pulse
            TeslaDetonateState.spp_pulseBonus = level / 2;
        }
    }

    [SkillLevelModifier("ESClusterArrow", typeof(ClusterBombArrow))]
    class HuntressClusterBombArrowModifier : SimpleSkillModifier<ClusterBombArrow>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            //+10% damage
            ClusterBombArrow.spp_damageMult = 1 + level * 0.1f;
            //+1 bomblet
            ClusterBombArrow.spp_bombletBonus = level;
        }
    }

    [SkillLevelModifier("ESShieldSplosion", typeof(ShieldSplosionEntity))]
    class LoaderShieldSplosionModifier : SimpleSkillModifier<ShieldSplosionEntity>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            //+10% radius
            ShieldSplosionEntity.spp_radiusMult = 1 + level * 0.1f;
            //+5% barrier refund
            ShieldSplosionEntity.spp_refund = 0.05f * level;
        }
    }

    [SkillLevelModifier("ESSlashport", typeof(SlashportEntity))]
    class MercSlashportModifier : SimpleSkillModifier<SlashportEntity>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            //+20% damage
            SlashportEntity.spp_damagemult = 1 + level * 0.2f;
            //+0.1 proc coeficcient
            SlashportEntity.spp_procbonus = level * 0.1f;
        }
    }

    [SkillLevelModifier("ESNanobots", typeof(NanobotEntity))]
    class MultNanobotsModifier : SimpleSkillModifier<NanobotEntity>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            //+10% nanobot damage
            SwarmComponent.spp_damageMult = 1 + level * 0.1f;
            //+20% nanobot healing
            SwarmComponent.spp_healMult = 1 + level * 0.2f;
            //+1 nanobot every other level
            SwarmComponent.spp_swarmBonus = level / 2;

        }
    }

    [SkillLevelModifier("ESRoot", typeof(DirectiveRoot))]
    class RexRootModifier : SimpleSkillModifier<DirectiveRoot>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            //+4m radius
            DirectiveRoot.spp_radiusBonus = level * 4f;
            //+10% barrier
            DirectiveRoot.spp_healMult = 1 + level * 0.1f;

        }
    }
}
