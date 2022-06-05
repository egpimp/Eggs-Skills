using UnityEngine;
using RoR2;
using EggsUtils.Buffs;
using EntityStates.Bandit2;
using EggsSkills.Config;
using UnityEngine.Networking;

namespace EggsSkills
{
    class InvisHandler : MonoBehaviour
    {
        //Skills++
        public float spp_stunRadius = 0f;
        public float spp_moveSpeedBonus = 1f;

        //Whether or not player is invis
        private bool isInvis;

        //Character body
        private CharacterBody characterBody;

        //How long til player turns invis while sprinting
        private readonly float baseTimeTilInvis = 0.4f;
        //How long does buff last after exiting invis
        private readonly float buffDuration = Configuration.GetConfigValue(Configuration.BanditInvissprintBuffduration);
        //Helps stop cd during invisibility
        private float holdTimer;
        //Counts down while sprinting before invisible
        private float timeTilInvis;

        //Input bank
        private InputBankTest bank;

        private GenericSkill utilitySlot;
        private void Start()
        {
            //Establish timer
            timeTilInvis = baseTimeTilInvis;
            //Nab characterbody
            characterBody = base.GetComponent<CharacterBody>();
            //Get the slot for utility
            utilitySlot = characterBody.skillLocator.utility;
            //Set invis to false
            isInvis = false;
            //nab input bank
            bank = base.GetComponent<InputBankTest>();
        }
        internal void MakeInvis()
        {
            //Execute server only
            if (NetworkServer.active)
            {
                //Add cloak
                characterBody.AddBuff(RoR2Content.Buffs.Cloak);
                //Add cloak speed boost
                characterBody.AddBuff(RoR2Content.Buffs.CloakSpeed);
            }
            //Flag as invis
            isInvis = true;
            //Reset time-to-invis timer
            timeTilInvis = baseTimeTilInvis;
            //Grab the current amount of cd counted down and put it in the hold
            holdTimer = utilitySlot.rechargeStopwatch;
            //Play the fx
            PlayEffects();

            characterBody.characterMotor.walkSpeedPenaltyCoefficient = spp_moveSpeedBonus;
        }
        internal void RemoveInvis()
        {
            if (NetworkServer.active)
            {
                //Remove the cloak buff
                characterBody.RemoveBuff(RoR2Content.Buffs.Cloak);
                //Remove the cloak speed boost
                characterBody.RemoveBuff(RoR2Content.Buffs.CloakSpeed);
                //Give the post-cloak buff
                characterBody.AddTimedBuff(BuffsLoading.buffDefCunning, buffDuration);
            }
            //Take off a stock of the ability
            utilitySlot.DeductStock(1);
            //Flag as visible
            isInvis = false;
            //Play the fx
            PlayEffects();

            if(spp_stunRadius > 0f)
            {
                new BlastAttack
                {
                    baseDamage = 0f,
                    radius = spp_stunRadius,
                    attacker = null,
                    inflictor = null,
                    teamIndex = characterBody.teamComponent.teamIndex,
                    crit = false,
                    losType = BlastAttack.LoSType.None,
                    position = characterBody.corePosition,
                    damageType = DamageType.Stun1s
                }.Fire();
            }

            characterBody.characterMotor.walkSpeedPenaltyCoefficient = 1f;
        }
        private void FixedUpdate()
        {
            //Structure here is weird, but we need slightly different events for slightly different cases
            //If sprinting AND not invis
            if (characterBody.isSprinting && !isInvis)
            {
                //AND time exists to countdown, count time down
                if (timeTilInvis > 0) timeTilInvis -= Time.fixedDeltaTime;
                //If countdown is up and there is a stock
                else if (characterBody.skillLocator.utility.stock > 0) MakeInvis();
            }
            //If NOT sprinting AND invis
            else if (!characterBody.isSprinting)
            {
                //If NOT sprinting AND invis, take away the invis
                if (isInvis) RemoveInvis();
                //Reset the invis timer
                timeTilInvis = baseTimeTilInvis;
            }

            //If invis, don't count the cooldown yet
            if(isInvis) utilitySlot.rechargeStopwatch = holdTimer;

            //Lastly, kill invis if m2 is used because they're agile I guess
            if (bank.skill2.down && isInvis) RemoveInvis();
        }
        internal bool IsInvis()
        {
            //Accessor for invis check
            return isInvis;
        }
        private void PlayEffects()
        {
            //Play smoke-bomb fx
            EffectManager.SimpleMuzzleFlash(StealthMode.smokeBombEffectPrefab, gameObject, StealthMode.smokeBombMuzzleString, false);
            //Sound string establish, enter sound if in stealth, exit sound if not stealth
            string soundString = isInvis ? StealthMode.enterStealthSound : StealthMode.exitStealthSound;
            //Play sound
            Util.PlaySound(soundString, gameObject);
        }
    }
}
