using UnityEngine;
using RoR2;
using EggsBuffs;
using EntityStates.Bandit2;

namespace EggsSkills
{
    [RequireComponent(typeof(CharacterBody))]
    class InvisHandler : MonoBehaviour
    {
        private bool isInvis;
        private CharacterBody characterBody;
        private float timeTilInvis;
        private GenericSkill utilitySlot;
        private float holdTimer;
        private void Start()
        {
            timeTilInvis = 0.4f;
            characterBody = GetComponent<CharacterBody>();
            utilitySlot = characterBody.skillLocator.utility;
            isInvis = false;
        }
        public void MakeInvis()
        {
            characterBody.AddBuff(RoR2Content.Buffs.Cloak);
            characterBody.AddBuff(RoR2Content.Buffs.CloakSpeed);
            isInvis = true;
            timeTilInvis = 0.4f;
            holdTimer = utilitySlot.rechargeStopwatch;
            PlayEffects();
        }
        public void RemoveInvis()
        {
            characterBody.RemoveBuff(RoR2Content.Buffs.Cloak);
            characterBody.RemoveBuff(RoR2Content.Buffs.CloakSpeed);
            characterBody.AddTimedBuff(BuffsLoading.buffDefCunning, 3f);
            utilitySlot.DeductStock(1);
            isInvis = false;
            PlayEffects();
        }
        private void FixedUpdate()
        {
            if(characterBody.isSprinting)
            {
                if (!isInvis)
                {
                    if (timeTilInvis > 0)
                    {
                        timeTilInvis -= Time.fixedDeltaTime;
                    }
                    else
                    {
                        if (characterBody.skillLocator.utility.stock > 0)
                        {
                            MakeInvis();
                        }
                    }
                }
            }
            else
            {
                if (isInvis)
                {
                    RemoveInvis();
                }
                timeTilInvis = 0.4f;
            }
            if(isInvis)
            {
                utilitySlot.rechargeStopwatch = holdTimer;
            }
        }
        public bool IsInvis()
        {
            return isInvis;
        }
        private void PlayEffects()
        {
            Util.PlaySound(StealthMode.enterStealthSound, gameObject);
            EffectManager.SimpleMuzzleFlash(StealthMode.smokeBombEffectPrefab, gameObject, StealthMode.smokeBombMuzzleString, false);
        }
    }
}
