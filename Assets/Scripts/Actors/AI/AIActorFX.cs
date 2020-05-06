using System.Collections;
using Actors.Base.Interface;
using GameSystems.FX;
using UnityEngine;

namespace Actors.AI
{
    public class AIActorFX : ParticleSpawner
    {

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
                StartCoroutine(SpawnParticle(healParticle, target, particleLifetime));
            }
            else if (args.healthChange < 0)
            {
                StartCoroutine(SpawnParticle(hitParticle, target, particleLifetime));
            }
        }
    }
    
    
}