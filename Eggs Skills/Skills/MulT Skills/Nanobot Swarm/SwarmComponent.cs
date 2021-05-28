using UnityEngine;
using RoR2;
using RoR2.Orbs;
using EggsSkills.Orbs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Networking;

namespace EggsSkills
{
    [RequireComponent(typeof(TeamComponent))]
    [RequireComponent(typeof(CharacterBody))]
    class SwarmComponent : MonoBehaviour
    {
        private CharacterBody ownerBody;
        private List<HurtBox> hurtBoxesList;
        private float queueTimer;
        private void Start()
        {
            ownerBody = GetComponent<CharacterBody>();
            hurtBoxesList = new List<HurtBox>();
            queueTimer = 0.1f;
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
                AddToTargets(hurtBox);
            }
        }
        private void AddToTargets(HurtBox hurtBox)
        {
            for (int i = 0; i < 3; i++)
            {
                hurtBoxesList.Add(hurtBox);
                List<HurtBox> tempList = new List<HurtBox>();
                while (hurtBoxesList.Count > 0)
                {
                    int randPos = Random.Range(1,hurtBoxesList.Count) - 1;
                    tempList.Add(hurtBoxesList.ElementAt(randPos));
                    hurtBoxesList.RemoveAt(randPos);
                }
                hurtBoxesList = tempList;
            }
        }
        private void FixedUpdate()
        {
            if (hurtBoxesList.Count > 0)
            {
                if(queueTimer > 0)
                {
                    queueTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    while(!hurtBoxesList.ElementAt(0))
                    {
                        hurtBoxesList.RemoveAt(0);
                    }
                    CallSwarm(hurtBoxesList.ElementAt(0));
                    hurtBoxesList.RemoveAt(0);
                    queueTimer = 0.05f;
                }
            }
        }
    }
}

