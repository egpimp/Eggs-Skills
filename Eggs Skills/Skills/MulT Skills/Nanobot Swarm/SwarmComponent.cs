﻿using UnityEngine;
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
        //Skills++
        internal static float spp_damageMult = 1f;
        internal static int spp_swarmBonus = 0;
        internal static float spp_healMult = 1f;

        //Body of the owner
        private CharacterBody ownerBody;

        //Damage coefficient per orb
        private static readonly float damageCoefficient = 0.8f * spp_damageMult;
        //Heal coefficient per orb
        private static readonly float healCoef = 0.015f * spp_healMult;
        //Time to reset the number to
        private static readonly float maxQueueTimer = 0.05f;
        //Proc coefficient
        private static readonly float nanoProcCoef = 0.4f;
        //Handles the countdown timer for releasing nanobots
        private float queueTimer;

        //How many nanobots per enemy
        private static readonly int nanoBotCount = Configuration.GetConfigValue(Configuration.ToolbotNanobotCountperenemy) + spp_swarmBonus;
        
        //List of all the hurtboxes to throw nanobots at
        private List<HurtBox> hurtBoxesList;

        private void Start()
        {
            //Gets the body of the owner
            ownerBody = GetComponent<CharacterBody>();
            //Establishes the list for the hurtboxes
            hurtBoxesList = new List<HurtBox>();
            //Set the queuetimer
            queueTimer = maxQueueTimer;
        }

        private void CallSwarm(HurtBox hurtBox)
        {
            //Create a new orb to fire
            NanobotOrb nanobotOrb = new NanobotOrb();
            //The attacker is the player
            nanobotOrb.attacker = gameObject;
            //Set the orb damage
            nanobotOrb.damageValue = ownerBody.damage * damageCoefficient;
            //Set orb healing
            nanobotOrb.healthFraction = healCoef;
            //Check if it crits
            nanobotOrb.isCrit = ownerBody.RollCrit();
            //Origin of the orb is straight out the center of the player body
            nanobotOrb.origin = ownerBody.corePosition;
            //Target is the main hurtbox of the hurtbox we found earlier
            nanobotOrb.target = hurtBox.hurtBoxGroup.mainHurtBox;
            //Proc coefficient is proc coefficient :v
            nanobotOrb.procCoefficient = nanoProcCoef;
            nanobotOrb.damageType = DamageTypeCombo.GenericSecondary;
            if (NetworkServer.active) OrbManager.instance.AddOrb(nanobotOrb);
        }

        internal void GetTargets(Vector3 impactPos)
        {
            //Spheresearch around the spot we hit
            foreach (HurtBox hurtBox in new SphereSearch
            {
                origin = impactPos,
                radius = 25,
                mask = LayerIndex.entityPrecise.mask
            }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.GetComponent<TeamComponent>().teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
            {
                //Every valid target we find, add them to the hitlist >:)
                AddToTargets(hurtBox);
            }
        }

        private void AddToTargets(HurtBox hurtBox)
        {
            //Repeat this x times where x is the amount of nanobots, basically puts them in the list once for each nanobot that they gonna get fired at them
            for (int i = 0; i < nanoBotCount; i++)
            {
                //Add them to the list specifically
                hurtBoxesList.Add(hurtBox);
                //Templist for shuffling
                List<HurtBox> tempList = new List<HurtBox>();
                //While there are still items in the hurtbox list
                while (hurtBoxesList.Count > 0)
                {
                    //Grab a random index based on the hurtbox list
                    int randPos = Random.Range(1,hurtBoxesList.Count) - 1;
                    //Put the item at index at the end of the templist
                    tempList.Add(hurtBoxesList.ElementAt(randPos));
                    //Take it out of the hurtbox list
                    hurtBoxesList.RemoveAt(randPos);
                }
                //Replace the now-empty hurtboxlist with the new shuffled list
                hurtBoxesList = tempList;
            }
        }

        private void FixedUpdate()
        {
            //As long as there is shit in the list
            if (hurtBoxesList.Count > 0)
            {
                //If the timer is over 0 tick it down
                if(queueTimer > 0) queueTimer -= Time.fixedDeltaTime;
                else
                {
                    //Remove anything that doesn't actually exist anymore quickly
                    while(!hurtBoxesList.ElementAt(0)) hurtBoxesList.RemoveAt(0);
                    //Fire out a 'nanobot swarm' that's actually just a squid orb basically lol
                    CallSwarm(hurtBoxesList.ElementAt(0));
                    //Remove the thing we just fired at from the list
                    hurtBoxesList.RemoveAt(0);
                    //Reset the queue timer
                    queueTimer = 0.05f;
                }
            }
        }
    }
}

