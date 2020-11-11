using Exceptions.Game.Projectile;
using Gameplay.Actors.Base.StatsStuff;
using UnityEngine;

namespace Gameplay.Projectile
{
    public abstract class BaseProjectile : MonoBehaviour
    {
        public float speed = .5f;
        public float lifeTime = 30f;
        public bool ignorePlayer = false;
        public float angleSpeed = 1;
        protected float launchTime;
        protected bool moving = false;
        protected float currentSpeed;

        protected new Rigidbody rigidbody;
        protected Damage damage;
        private new Collider collider;


        public delegate void OnHitSomething(GameObject hitted);
        public OnHitSomething onHitSomething;
        protected virtual void Awake()
        {
            moving = false;
            collider = GetComponent<Collider>();
            rigidbody = GetComponent<Rigidbody>();
            currentSpeed = speed;
            if (collider == null)
            {
                throw new ProjectileDontHaveColider();
            }
        }


        public virtual void Launch(Damage damage)
        {
            this.damage = damage;
            launchTime = Time.time;
            moving = true;
        }

        public virtual void Stop()
        {
            moving = false;
            rigidbody.isKinematic = true;
            onHitSomething = null;
        }
        
        protected virtual void FixedUpdate()
        {
            if (!moving)
            {
                return;
            }

            if (Time.time - launchTime >= lifeTime)
            {
                Stop();
                Destroy(gameObject);
            }

            Move();
        }

        protected virtual void Move()
        {
            transform.forward =
                Vector3.Slerp(transform.forward, rigidbody.velocity.normalized, Time.deltaTime * angleSpeed);
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && ignorePlayer)
            {
                return;
            }
            
            onHitSomething?.Invoke(other.gameObject);
        }
    }
}