using System.Collections;
using Actors.Base.Interface;
using UnityEngine;

namespace Gameplay.Projectile
{
    public class Arrow : BaseProjectile
    {
        
        protected override void Awake()
        {
            base.Awake();

            onHitSomething += OnHealthableHit;
        }

        private void OnHealthableHit(GameObject hitted)
        {
            Stop();
            gameObject.transform.parent = hitted.transform;
            IHealthable healthable = hitted.GetComponentInParent<IHealthable>();
            StartCoroutine(Destroy());
            
            if (healthable == null)
            {
                
                return;
            }
            
            healthable.TakeDamage(damage);
        }


        IEnumerator Destroy()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}