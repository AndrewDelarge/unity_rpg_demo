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
        
        
        protected ParticleSystem currentTrail;
        
        private float trailTime;
        private Base.Combat combat;
        private EquipmentManager equipmentManager;
        private Transform target;
        private IHealthable stats;
        
        public void Init(Base.Combat combat)
        {
            this.combat = combat;
            equipmentManager = GameController.instance.playerManager.equipmentManager;
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
                currentTrail.Play();
                
                yield return new WaitForSeconds(combat.GetCurrentMeleeAttackSpeed());
                
                currentTrail.Stop();
            }
        }


        void HideTrail()
        {
            if (currentTrail != null)
            {
                currentTrail.Stop();
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
            
            Vector3 pos = new Vector3(0.0025f, -0.01745f, -0.0013f);

            currentTrail = Instantiate(trail, weaponTransform, false).GetComponent<ParticleSystem>();
            if (currentTrail == null)
            {
                return;
            }
            currentTrail.Stop();
            currentTrail.transform.localPosition = weapon.trailSpawnLocalPos;
            currentTrail.transform.localScale = weapon.trailSpawnLocalScale;
            currentTrail.transform.localRotation = Quaternion.Euler(
                weapon.trailSpawnLocalRotation.x, weapon.trailSpawnLocalRotation.y, weapon.trailSpawnLocalRotation.z);
            
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