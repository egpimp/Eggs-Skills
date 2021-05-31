using UnityEngine;
using RoR2;
using RoR2.Orbs;
using EggsSkills.Orbs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;
using EggsSkills.Config;

namespace EggsSkills
{
    [RequireComponent(typeof(TeamComponent))]
    [RequireComponent(typeof(CharacterBody))]
    class SwarmComponent : MonoBehaviour
    {
        private CharacterBody ownerBody;

        private float queueTimer;

        private int nanoBotCount = Configuration.GetConfigValue<int>(Configuration.ToolbotNanobotCountperenemy);

        private List<HurtBox> hurtBoxesList;
        private void Start()
        {
            this.ownerBody = GetComponent<CharacterBody>();
            this.hurtBoxesList = new List<HurtBox>();
            this.queueTimer = 0.05f;
        }
        private void CallSwarm(HurtBox hurtBox)
        {
            GenericDamageOrb nanobotOrb = new NanobotOrb();
            nanobotOrb.attacker = gameObject;
            nanobotOrb.damageValue = ownerBody.damage * 0.8f;
            nanobotOrb.isCrit = ownerBody.RollCrit();
            nanobotOrb.origin = ownerBody.corePosition;
            nanobotOrb.target = hurtBox.hurtBoxGroup.mainHurtBox;
            nanobotOrb.procCoefficient = 0.4f;
            if (NetworkServer.active)
            {
                OrbManager.instance.AddOrb(nanobotOrb);
            }
        }
        internal void GetTargets(Vector3 impactPos)
        {
            foreach (HurtBox hurtBox in new SphereSearch
            {
                origin = impactPos,
                radius = 25,
                mask = LayerIndex.entityPrecise.mask
            }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.GetComponent<TeamComponent>().teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
            {
                this.AddToTargets(hurtBox);
            }
        }
        private void AddToTargets(HurtBox hurtBox)
        {
            for (int i = 0; i < this.nanoBotCount; i++)
            {
                this.hurtBoxesList.Add(hurtBox);
                List<HurtBox> tempList = new List<HurtBox>();
                while (this.hurtBoxesList.Count > 0)
                {
                    int randPos = Random.Range(1,hurtBoxesList.Count) - 1;
                    tempList.Add(hurtBoxesList.ElementAt(randPos));
                    this.hurtBoxesList.RemoveAt(randPos);
                }
                this.hurtBoxesList = tempList;
            }
        }
        private void FixedUpdate()
        {
            if (this.hurtBoxesList.Count > 0)
            {
                if(this.queueTimer > 0)
                {
                    this.queueTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    while(!this.hurtBoxesList.ElementAt(0))
                    {
                        this.hurtBoxesList.RemoveAt(0);
                    }
                    this.CallSwarm(this.hurtBoxesList.ElementAt(0));
                    this.hurtBoxesList.RemoveAt(0);
                    this.queueTimer = 0.05f;
                }
            }
        }
    }
}

