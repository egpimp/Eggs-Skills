using RoR2;
using UnityEngine;
using EntityStates.Captain.Weapon;
using RoR2.Stats;
using EggsSkills.Config;
using RoR2.Achievements;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    class InversionAchievement : BaseAchievement
    {
        internal const string ACHNAME = "VoidsurvivorManyFormChange";
        internal const string REWARDNAME = "EggsSkills.Inversion";
        internal const uint TOKENS = 10;

        private static readonly int reqFormChanges = 6;

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.voidFiendRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            On.EntityStates.VoidSurvivor.CorruptionTransitionBase.OnEnter += DetectFormChange;
            TeleporterInteraction.onTeleporterBeginChargingGlobal += TeleporterStart;
            TeleporterInteraction.onTeleporterChargedGlobal += TeleporterEnd;
            if (Configuration.UnlockAll.Value) base.Grant();
        }


        public override void OnUninstall()
        {
            base.OnUninstall();
            On.EntityStates.VoidSurvivor.CorruptionTransitionBase.OnEnter -= DetectFormChange;
            TeleporterInteraction.onTeleporterBeginChargingGlobal -= TeleporterStart;
            TeleporterInteraction.onTeleporterChargedGlobal -= TeleporterEnd;
        }

        int currentFormChanges = 0;

        private void DetectFormChange(On.EntityStates.VoidSurvivor.CorruptionTransitionBase.orig_OnEnter orig, global::EntityStates.VoidSurvivor.CorruptionTransitionBase self)
        {
            orig(self);
            if (base.isUserAlive && base.localUser.cachedBody.master == self.characterBody.master) currentFormChanges++;
        }

        private void TeleporterStart(TeleporterInteraction interaction)
        {
            currentFormChanges = 0;
        }

        private void TeleporterEnd(TeleporterInteraction interaction)
        {
            if (currentFormChanges >= 6 && base.meetsBodyRequirement) base.Grant();
        }
    }
}