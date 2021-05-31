using System;
using RoR2;
using UnityEngine;
using EggsSkills.Resources;
using System.Collections.Generic;
using EggsSkills.Config;

namespace EggsSkills.Achievements
{
    class NanoBotAchievement : ModdedUnlockable
    {
        public override string AchievementIdentifier { get; } = "MULT_MOTHERSHIPUNLOCKABLE_ACHIEVEMENT_ID";
        public override string UnlockableIdentifier { get; } = "MULT_MOTHERSHIPUNLOCKABLE_REWARD_ID";
        public override string AchievementNameToken { get; } = "MULT_MOTHERSHIPUNLOCKABLE_ACHIEVEMENT_NAME";
        public override string PrerequisiteUnlockableIdentifier { get; } = "";
        public override string UnlockableNameToken { get; } = "MULT_MOTHERSHIPUNLOCKABLE_UNLOCKABLE_NAME";
        public override string AchievementDescToken { get; } = "MULT_MOTHERSHIPUNLOCKABLE_ACHIEVEMENT_DESC";
        public override Sprite Sprite { get; } = Sprites.nanoBotsIconS;

        public override Func<string> GetHowToUnlock { get; } = (() => Language.GetStringFormatted("UNLOCK_VIA_ACHIEVEMENT_FORMAT", new object[]
        {
            Language.GetString("MULT_MOTHERSHIPUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("MULT_MOTHERSHIPUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override Func<string> GetUnlocked { get; } = (() => Language.GetStringFormatted("UNLOCKED_FORMAT", new object[]
        {
            Language.GetString("MULT_MOTHERSHIPUNLOCKABLE_ACHIEVEMENT_NAME"),
            Language.GetString("MULT_MOTHERSHIPUNLOCKABLE_ACHIEVEMENT_DESC")
        }));

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(SkillsLoader.multRef);
        }

        public override void OnInstall()
        {
            base.OnInstall();
            MinionOwnership.onMinionOwnerChangedGlobal += CheckMinions;
            if (Configuration.UnlockAll.Value)
            {
                base.Grant();
            }
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            MinionOwnership.onMinionOwnerChangedGlobal -= CheckMinions;
        }

        private string[] validDroneNames = new string[] {"Drone1", "Drone2", "DroneBackup", "DroneMissile", "EmergencyDrone", "FlameDrone", "EquipmentDrone"};
        private void CheckMinions(MinionOwnership minion)
        {
            if (base.isUserAlive && base.meetsBodyRequirement)
            {
                if (minion != null)
                {
                    MinionOwnership.MinionGroup group = minion.group;
                    if (group != null)
                    {
                        CharacterMaster playerMaster = base.localUser.cachedMasterController.master;
                        if (playerMaster != null)
                        {
                            if (group.ownerId == playerMaster.netId)
                            {
                                MinionOwnership[] minionArray = group.members;
                                if (minionArray != null)
                                {
                                    int validDrones = 0;
                                    foreach (MinionOwnership tempMinion in minionArray)
                                    {
                                        if (tempMinion != null)
                                        {
                                            List<string> nameList = new List<string>();
                                            foreach (string tempName in validDroneNames)
                                            {
                                                nameList.Add(tempName + "Master(Clone)");
                                            }
                                            string droneName = tempMinion.name;
                                            if (nameList.Contains(droneName))
                                            {
                                                validDrones += 1;
                                            }
                                        }
                                    }
                                    if (validDrones >= 8)
                                    {
                                        base.Grant();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}