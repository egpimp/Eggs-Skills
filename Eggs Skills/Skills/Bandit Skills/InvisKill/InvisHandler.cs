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
        private bool isInvis;

        private CharacterBody characterBody;

        private float baseTimeTilInvis = 0.4f;
        private float buffDuration = Configuration.GetConfigValue<float>(Configuration.BanditInvissprintBuffduration);
        private float holdTimer;
        private float timeTilInvis;

        private GenericSkill utilitySlot;
        private void Start()
        {
            this.timeTilInvis = this.baseTimeTilInvis;
            this.characterBody = base.GetComponent<CharacterBody>();
            this.utilitySlot = this.characterBody.skillLocator.utility;
            this.isInvis = false;
        }
        internal void MakeInvis()
        {
            this.characterBody.AddBuff(RoR2Content.Buffs.Cloak);
            this.characterBody.AddBuff(RoR2Content.Buffs.CloakSpeed);
            this.isInvis = true;
            this.timeTilInvis = this.baseTimeTilInvis;
            this.holdTimer = this.utilitySlot.rechargeStopwatch;
            this.PlayEffects();
        }
        internal void RemoveInvis()
        {
            this.characterBody.RemoveBuff(RoR2Content.Buffs.Cloak);
            this.characterBody.RemoveBuff(RoR2Content.Buffs.CloakSpeed);
            this.characterBody.AddTimedBuff(BuffsLoading.buffDefCunning, this.buffDuration);
            this.utilitySlot.DeductStock(1);
            this.isInvis = false;
            this.PlayEffects();
        }
        private void FixedUpdate()
        {
            if(this.characterBody.isSprinting)
            {
                if (!this.isInvis)
                {
                    if (this.timeTilInvis > 0)
                    {
                        this.timeTilInvis -= Time.fixedDeltaTime;
                    }
                    else
                    {
                        if (this.characterBody.skillLocator.utility.stock > 0)
                        {
                            this.MakeInvis();
                        }
                    }
                }
            }
            else
            {
                if (this.isInvis)
                {
                    this.RemoveInvis();
                }
                this.timeTilInvis = this.baseTimeTilInvis;
            }
            if(this.isInvis)
            {
                this.utilitySlot.rechargeStopwatch = this.holdTimer;
            }
        }
        internal bool IsInvis()
        {
            return this.isInvis;
        }
        private void PlayEffects()
        {
            EffectManager.SimpleMuzzleFlash(StealthMode.smokeBombEffectPrefab, base.gameObject, StealthMode.smokeBombMuzzleString, false);
            string soundString;
            if (this.isInvis)
            {
                soundString = StealthMode.enterStealthSound;
            }
            else
            {
                soundString = StealthMode.exitStealthSound;
            }
            Util.PlaySound(soundString, base.gameObject);
        }
    }
}
