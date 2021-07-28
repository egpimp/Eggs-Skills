using System;
using RoR2;
using UnityEngine;
using EggsSkills.Resources;
using EggsSkills.Config;
using R2API;

namespace EggsSkills.Achievements
{
    class SlashportAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "MERC_CULLUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "MERC_CULLUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "MERC_CULLUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "MERC_CULLUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "MERC_CULLUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.slashportIconS;

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("MERC_CULLUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("MERC_CULLUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("MERC_CULLUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("MERC_CULLUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.mercenaryRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            RoR2Application.onUpdate += SlashComponentCheck;
            On.RoR2.HealthComponent.TakeDamage += DamageChecker;
            if (Configuration.UnlockAll.Value)
            {
                base.Grant();
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            RoR2Application.onUpdate -= SlashComponentCheck;
            On.RoR2.HealthComponent.TakeDamage -= DamageChecker;
        }

        private void SlashComponentCheck()
        {
            if(base.localUser != null && base.localUser.cachedBody != null && base.meetsBodyRequirement && base.isUserAlive)
            {
                CullAchievementHandler component = base.localUser.cachedBody.gameObject.GetComponent<CullAchievementHandler>();
                if(!component)
                {
                    component = base.localUser.cachedBody.gameObject.AddComponent<CullAchievementHandler>();
                }
            }
        }

        private void DamageChecker(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (damageInfo.attacker)
            {
                if (base.isUserAlive && base.meetsBodyRequirement)
                {
                    if (damageInfo.attacker.GetComponent<CharacterBody>().master.netId != null && damageInfo.attacker.GetComponent<CharacterBody>().master.netId == base.localUser.cachedMasterController.master.netId)
                    {
                        if (self)
                        {
                            if (self.body.HasBuff(RoR2Content.Buffs.MercExpose))
                            {
                                CullAchievementHandler component = damageInfo.attacker.GetComponent<CullAchievementHandler>();
                                if (component)
                                {
                                    component.AddVictim(self);
                                    if (component.ReqMet())
                                    {
                                        base.Grant();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            orig(self, damageInfo);
        }
    }
}