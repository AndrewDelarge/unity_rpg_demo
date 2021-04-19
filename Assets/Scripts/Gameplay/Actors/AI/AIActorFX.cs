using Gameplay.Actors.Base;
using Gameplay.Actors.Base.Interface;
using GameSystems;
using GameSystems.FX;
using Managers;
using UI;
using UnityEngine;

namespace Gameplay.Actors.AI
{
    public class AIActorFX : ParticleSpawner
    {
        // TODO particle helper
        public GameObject hitParticle;
        public GameObject healParticle;
        
        public float particleLifetime;
        
        
        private IHealthable stats;
        private Transform target;
        public void Init()
        {
            stats = GetComponent<IHealthable>();
            stats.OnHealthChange += ShowHealChange;
            
            target = transform;

            // TODO: WTF
            Transform targetRend = GetComponentInChildren<Transform>();
            
            if (targetRend != null)
                target = targetRend.transform;
            
        }
        
        private void ShowHealChange(object healthable, HealthChangeEventArgs args)
        {
            if (args.healthChange > 0)
            {
                StartCoroutine(SpawnParticle(healParticle, target, particleLifetime));
                return;
            }

            var damageInitiator = args.modifier.GetOwner();
            if (damageInitiator == null)
            {
                StartCoroutine(SpawnParticle(hitParticle, target, particleLifetime));
                return;
            }
            
            Quaternion quaternion = new Quaternion();
            quaternion.SetLookRotation(damageInitiator.transform.position);
            StartCoroutine(SpawnParticle(hitParticle, target, particleLifetime, quaternion));
        }


    }
    
    
}