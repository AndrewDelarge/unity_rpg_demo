using System.Collections.Generic;
using Actors.Base;
using Actors.Base.Interface;
using GameInput;
using Scriptable;
using UnityEngine;

namespace Actors.AI
{
    [RequireComponent(typeof(AICombat))]
    public class AIActor : Actor
    {
        public Collider actorColider{ get; protected set; }


        private Collider[] skilletColliders;

        public override void Init()
        {
            base.Init();

            actorColider = GetComponent<Collider>();
            
            skilletColliders = GetComponentsInChildren<Collider>();
            
            foreach (Collider collider in skilletColliders)
            {
                if (collider == actorColider)
                {
                    continue;
                }
                collider.isTrigger = true;
            }
        }

        protected override void Die(GameObject go)
        {
            base.Die(go);
            actorColider.enabled = false;
            Ragdoll();
        }

        void Ragdoll()
        {
            foreach (Collider collider in skilletColliders)
            {
                if (collider == actorColider)
                {
                    continue;
                }
                
                collider.isTrigger = false;
            }
            
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                
                rigidbody.AddForce(- gameObject.transform.forward * 10, ForceMode.Impulse);
            }
        }

        public override void MeleeAttack(Actor target)
        {
            if (combat.InMeleeRange(target.transform.position))
            {
                movement.FaceTarget(target.transform.position);
                List<IHealthable> attackList = new List<IHealthable>();
                attackList.Add(target.stats);
                
                combat.MeleeAttack(attackList);
            }
        }

        public override void SetActorTarget(Actor newTarget)
        {
            base.SetActorTarget(newTarget);
            
            combat.SetTarget(newTarget);
        }

        public override void SetTransformTarget(Transform newTarget)
        {
            base.SetTransformTarget(newTarget);
            
            movement.SetTarget(newTarget);
        }
    }
}