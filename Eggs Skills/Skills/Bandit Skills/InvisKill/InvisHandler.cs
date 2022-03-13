using UnityEngine;
using RoR2;
using EggsUtils.Buffs;
using EntityStates.Bandit2;
using EggsSkills.Config;

namespace EggsSkills
{
    [RequireComponent(typeof(CharacterBody))]
    class InvisHandler : MonoBehaviour
    {
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

        private GenericSkill utilitySlot;
        private void Start()
        {
            //Establish timer
            this.timeTilInvis = this.baseTimeTilInvis;
            //Nab characterbody
            this.characterBody = base.GetComponent<CharacterBody>();
            //Get the slot for utility
            this.utilitySlot = this.characterBody.skillLocator.utility;
            //Set invis to false
            this.isInvis = false;
        }
        internal void MakeInvis()
        {
            //Add cloak
            this.characterBody.AddBuff(RoR2Content.Buffs.Cloak);
            //Add cloak speed boost
            this.characterBody.AddBuff(RoR2Content.Buffs.CloakSpeed);
            //Flag as invis
            this.isInvis = true;
            //Reset time-to-invis timer
            this.timeTilInvis = this.baseTimeTilInvis;
            //Grab the current amount of cd counted down and put it in the hold
            this.holdTimer = this.utilitySlot.rechargeStopwatch;
            //Play the fx
            this.PlayEffects();
        }
        internal void RemoveInvis()
        {
            //Remove the cloak buff
            this.characterBody.RemoveBuff(RoR2Content.Buffs.Cloak);
            //Remove the cloak speed boost
            this.characterBody.RemoveBuff(RoR2Content.Buffs.CloakSpeed);
            //Give the post-cloak buff
            this.characterBody.AddTimedBuff(BuffsLoading.buffDefCunning, this.buffDuration);
            //Take off a stock of the ability
            this.utilitySlot.DeductStock(1);
            //Flag as visible
            this.isInvis = false;
            //Play the fx
            this.PlayEffects();
        }
        private void FixedUpdate()
        {
            //Structure here is weird, but we need slightly different events for slightly different cases
            //If sprinting...
            if(this.characterBody.isSprinting)
            {
                //If sprinting AND not invis
                if (!this.isInvis)
                {
                    //AND time exists to countdown, count time down
                    if (this.timeTilInvis > 0) this.timeTilInvis -= Time.fixedDeltaTime;
                    //AND countdown is up
                    else
                    {
                        //AND there is a stock, activate invisibility
                        if (this.characterBody.skillLocator.utility.stock > 0) this.MakeInvis();
                    }
                }
            }
            //If NOT sprinting
            else
            {
                //If NOT sprinting AND invis, take away the invis
                if (this.isInvis) this.RemoveInvis();
                //Reset the invis timer
                this.timeTilInvis = this.baseTimeTilInvis;
            }

            //If invis, don't count the cooldown yet
            if(this.isInvis) this.utilitySlot.rechargeStopwatch = this.holdTimer;
        }
        internal bool IsInvis()
        {
            //Accessor for invis check
            return this.isInvis;
        }
        private void PlayEffects()
        {
            //Play smoke-bomb fx
            EffectManager.SimpleMuzzleFlash(StealthMode.smokeBombEffectPrefab, base.gameObject, StealthMode.smokeBombMuzzleString, false);
            //Sound string establish, enter sound if in stealth, exit sound if not stealth
            string soundString = isInvis ? StealthMode.enterStealthSound : StealthMode.exitStealthSound;
            //Play sound
            Util.PlaySound(soundString, base.gameObject);
        }
    }
}
