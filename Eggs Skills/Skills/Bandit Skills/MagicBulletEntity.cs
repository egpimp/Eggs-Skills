using EggsSkills.Config;
using EntityStates.Bandit2.Weapon;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace EggsSkills.EntityStates
{
    class MagicBulletEntity : Bandit2FirePrimaryBase
    {
        private Bandit2FireRifle assetRef = new Bandit2FireRifle();

        private bool isCrit;

        private BulletAttack attack;

        private float baseCoef = 2f;
        private float damage;

        private int luckyMod = Configuration.GetConfigValue<int>(Configuration.BanditMagicbulletLuckmod);
        private int maxRecursion = Configuration.GetConfigValue<int>(Configuration.BanditMagicbulletRicochets);
        private int recursion;

        private List<HurtBox> hitHurtBoxes = new List<HurtBox>();
        public override void OnEnter()
        {
            this.baseDuration = 1.2f;
            this.minimumBaseDuration = 0.1f;
            this.isCrit = base.RollCrit();
            if (isCrit)
            {
                base.characterBody.master.luck += this.luckyMod;
            }
            this.recursion = 0;
            base.OnEnter();
        }

        public override void FireBullet(Ray aimRay)
        {
            Util.PlaySound(this.assetRef.fireSoundString, base.gameObject);
            this.damage = this.baseCoef * base.characterBody.damage;
            attack = new BulletAttack
            {
                origin = aimRay.origin,
                aimVector = aimRay.direction,
                tracerEffectPrefab = this.assetRef.tracerEffectPrefab,
                muzzleName = this.assetRef.muzzleName,
                hitEffectPrefab = this.assetRef.hitEffectPrefab,
                damage = this.damage,
                owner = base.gameObject,
                isCrit = this.isCrit,
                bulletCount = 1u,
                maxDistance = 1000f,
                smartCollision = true,
                procCoefficient = 1f,
                damageType = DamageType.Generic,
                HitEffectNormal = false,
                falloffModel = BulletAttack.FalloffModel.None,
                weapon = base.gameObject,
                force = 50f,
                hitCallback = CallBack,
            };
            attack.Fire();
            EffectManager.SimpleMuzzleFlash(assetRef.muzzleFlashPrefab, base.gameObject, this.assetRef.muzzleName, false);
        }
        public override void OnExit()
        {
            if (isCrit)
            {
                base.characterBody.master.luck -= this.luckyMod;
            }
            base.OnExit();
        }

        private bool CallBack(ref BulletAttack.BulletHit hitInfo)
        {
            bool result = this.attack.DefaultHitCallback(ref hitInfo);
            Vector3 pos = hitInfo.point;
            if (hitInfo.hitHurtBox)
            {
                hitHurtBoxes.Add(hitInfo.hitHurtBox.hurtBoxGroup.mainHurtBox);
            }
            HandleRichochet(pos);
            return result;
        }


        private void HandleRichochet(Vector3 pos)
        {
            bool targetFound = false;
            if (recursion < maxRecursion)
            {
                this.damage /= 2;
                foreach (HurtBox hurtBox in new SphereSearch
                {
                    origin = pos,
                    radius = 25f,
                    mask = LayerIndex.entityPrecise.mask,
                }.RefreshCandidates().FilterCandidatesByHurtBoxTeam(TeamMask.GetEnemyTeams(base.teamComponent.teamIndex)).OrderCandidatesByDistance().FilterCandidatesByDistinctHurtBoxEntities().GetHurtBoxes())
                {
                    HurtBox mainBox = hurtBox.hurtBoxGroup.mainHurtBox;
                    if (!hitHurtBoxes.Contains(mainBox))
                    {
                        this.hitHurtBoxes.Add(mainBox);
                        targetFound = true;
                        this.recursion += 1;
                        SimulateBullet(pos, mainBox);
                        break;
                    }
                }
                if (!targetFound)
                {
                    this.recursion = this.maxRecursion;
                }
            }
        }

        private void SimulateBullet(Vector3 pos, HurtBox box)
        {
            Vector3 origin = pos;
            Vector3 dir = (box.transform.position - pos).normalized;
            float dist = Vector3.Distance(box.transform.position, pos);
            EffectData data = new EffectData()
            {
                start = origin,
                origin = origin + dir * dist
            };
            EffectManager.SpawnEffect(assetRef.tracerEffectPrefab, data, true);
            if(base.isAuthority)
            {
                DamageInfo info = new DamageInfo
                {
                    damage = this.damage,
                    procCoefficient = 1f,
                    damageType = DamageType.Generic,
                    attacker = base.gameObject,
                    crit = this.isCrit,
                    inflictor = base.gameObject,
                    position = box.transform.position
                };
                box.healthComponent.TakeDamage(info);
                GlobalEventManager.instance.OnHitEnemy(info, box.gameObject);
                GlobalEventManager.instance.OnHitAll(info, box.gameObject);
                this.HandleRichochet(box.transform.position);
            }
        }
    }
}
