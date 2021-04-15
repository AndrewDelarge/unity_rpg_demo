using System.Collections;
using Gameplay.Actors.Base;
using Gameplay.Actors.Base.Interface;
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
            Actor targetActor = hitted.GetComponentInParent<Actor>();
            
            if (targetActor != null && targetActor.IsFriend(damage.GetOwner()))
                return;
            
            Stop();
            
            IHealthable healthable = hitted.GetComponentInParent<IHealthable>();
            
            if (healthable == null)
                return;
            
            healthable.TakeDamage(damage);
            Destroy(gameObject);
        }


        IEnumerator Destroy()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}