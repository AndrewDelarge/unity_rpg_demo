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
        public Collider collider{ get; protected set; }

        protected virtual void Awake()
        {
            Init();
        }


        public override void Init()
        {
            base.Init();

            collider = GetComponent<Collider>();
        }

        protected override void Die(GameObject go)
        {
            base.Die(go);
            collider.enabled = false;
            Ragdoll();
        }

        void Ragdoll()
        {
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