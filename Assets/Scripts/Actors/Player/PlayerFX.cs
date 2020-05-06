using System.Collections;
using Actors.Base.Interface;
using GameSystems;
using GameSystems.FX;
using Managers.Player;
using Scriptable;
using UnityEngine;

namespace Actors.Player
{
    public class PlayerFX : ParticleSpawner
    {
        public GameObject healParticle;
        public GameObject hitParticle;
        public float particleLifetime;
        
        
        protected TrailRenderer currentTrail;
        
        private float trailTime;
        private Base.Combat combat;
        private EquipmentManager equipmentManager;
        private Transform target;
        private IHealthable stats;
        
        public void Init(Base.Combat combat)
        {
            equipmentManager = GameController.instance.playerManager.equipmentManager;
            this.combat = combat;
            equipmentManager.onWeaponEquip += SpawnTrail;
            equipmentManager.onWeaponUnequip += DestroyTrail;
            combat.OnAttack += () => StartCoroutine(ShowTrail());
            
            stats = GetComponent<IHealthable>();
            stats.OnHealthChange += ShowHealChange;
            target = transform;
            Transform targetRend = GetComponentInChildren<Transform>();
            if (targetRend != null)
            {
                target = targetRend.transform;
            }
            
        }

        void ShowHealChange(object healthable, HealthChangeEventArgs args)
        {
            if (args.healthChange > 0)
            {
                StartCoroutine(SpawnParticle(healParticle, transform, particleLifetime));
            }
            else if (args.healthChange < 0)
            {
                StartCoroutine(SpawnParticle(hitParticle, transform, particleLifetime));
            }
        }
        

        IEnumerator ShowTrail()
        {
            if (currentTrail != null)
            {
                currentTrail.enabled = true;
                
                yield return new WaitForSeconds(combat.GetCurrentMeleeAttackSpeed());
                
                currentTrail.enabled = false;
            }
        }


        void HideTrail()
        {
            if (currentTrail != null)
            {
                currentTrail.enabled = false;
            }
        }


        void SpawnTrail(Weapon weapon)
        {
            Transform weaponTransform = equipmentManager.GetEquipmentTransform(weapon);
            GameObject trail = weapon.trail;
            
            if (trail == null)
            {
                return;
            }
            
            Vector3 pos = new Vector3(0.002f, -0.015f, 0.0036f);

            currentTrail = Instantiate(trail, weaponTransform).GetComponent<TrailRenderer>();
//            currentTrail.transform.position = pos;
            currentTrail.enabled = false;
            trailTime = currentTrail.time;
        }

        void DestroyTrail(Weapon weapon)
        {
            if (currentTrail != null)
            {
                Destroy(currentTrail);
                currentTrail = null;
            }
        }
    }
}