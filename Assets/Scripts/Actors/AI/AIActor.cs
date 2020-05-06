using System.Collections.Generic;
using Actors.Base;
using Actors.Base.Interface;
using Actors.Base.StatsStuff;
using UI;
using UnityEngine;

namespace Actors.AI
{
    [RequireComponent(typeof(AICombat))]
    public class AIActor : Actor
    {
        public Collider actorColider{ get; protected set; }
        private Collider[] skilletColliders;

        private AIActorFX actorFx;
        private AIDialog dialog;
        
        public override void Init()
        {
            base.Init();

            actorFx = GetComponent<AIActorFX>();
            if (actorFx != null)
            {
                actorFx.Init();
            }
            
            dialog = GetComponent<AIDialog>();
            if (dialog != null)
            {
                dialog.Init();
            }
            
            HealthUI healthUi = GetComponent<HealthUI>();
            if (healthUi != null)
            {
                healthUi.Init();
            }
            
            
            actorColider = GetComponent<Collider>();
            
            skilletColliders = GetComponentsInChildren<Collider>();
            
            SetSkilletColliderActivity(false);

            stats.onGetDamage += PushBack;
        }

        void PushBack(Damage damage)
        {
            float power = .1f;
            if (damage.IsCrit())
            {
                power *= .5f;
            }

            Vector3 pos = transform.forward;
            
            Actor owner = damage.GetOwner();

            if (owner != null)
            {
                pos = transform.InverseTransformPoint(owner.transform.position);
            }
            transform.position = transform.TransformPoint((-pos + Vector3.up) * power);
        }
        
        protected override void Die(GameObject go)
        {
            base.Die(go);
            actorColider.enabled = false;
            Ragdoll();
        }

        void Ragdoll()
        {
            SetSkilletColliderActivity(true);
            
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                
                rigidbody.AddForce(- gameObject.transform.forward * 10, ForceMode.Impulse);
            }
        }

        void SetSkilletColliderActivity(bool activity)
        {
            foreach (Collider collider in skilletColliders)
            {
                if (collider == actorColider)
                {
                    continue;
                }
                
                collider.isTrigger = ! activity;
            }
        }
        
        public override void MeleeAttack(IHealthable target)
        {
            if (combat.InMeleeRange(target.GetTransform()))
            {
                movement.FaceTarget(target.GetTransform().position);
                List<IHealthable> attackList = new List<IHealthable>();
                attackList.Add(target);
                
                combat.MeleeAttack(attackList);
            }
        }
    }
}