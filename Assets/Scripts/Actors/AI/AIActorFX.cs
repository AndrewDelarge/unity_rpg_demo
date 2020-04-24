using System.Collections;
using Actors.Base;
using Actors.Base.Interface;
using Actors.Combat;
using UnityEngine;

namespace Actors.AI
{
    public class AIActorFX : MonoBehaviour
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
                StartCoroutine(SpawnParticle(healParticle));
            }
            else if (args.healthChange < 0)
            {
                StartCoroutine(SpawnParticle(hitParticle));
            }
        }
        
        
        IEnumerator SpawnParticle(GameObject particle)
        {
            if (particle == null)
            {
                yield break;
            }
            
            Vector3 pos = target.position;
            pos.y += 1;
            
            GameObject currentParticle = Instantiate(particle, pos, new Quaternion(), target);

            currentParticle.transform.localScale = transform.localScale;
            
            yield return new WaitForSeconds(particleLifetime);
            
            Destroy(currentParticle);
        }

    }
    
    
}