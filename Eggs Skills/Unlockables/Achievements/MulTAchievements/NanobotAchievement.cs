using RoR2;
using System.Collections.Generic;
using EggsSkills.Config;
using RoR2.Achievements;

namespace EggsSkills.Achievements
{
    [RegisterAchievement("ES_" + ACHNAME, REWARDNAME, null, TOKENS)]
    internal class NanoBotAchievement : BaseAchievement
    {
        internal const string ACHNAME = "ToolbotManyDrones";
        internal const string REWARDNAME = "EggsSkills.NanoBot";
        internal const uint TOKENS = 10;

        //How many drones to unlock
        private static readonly int droneReq = 8;

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
            //This looks really ugly, but bear in mind that while trying to get this to work there was bountiful errors and it works so
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
                                                validDrones++;
                                            }
                                        }
                                    }
                                    if (validDrones >= droneReq)
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