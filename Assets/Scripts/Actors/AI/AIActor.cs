using Actors.Base;
using Actors.Base.Interface;
using GameInput;
using UnityEngine;

namespace Actors.AI
{
    [RequireComponent(typeof(AICombat))]
    public class AIActor : Actor
    {
        public Collider collider{ get; protected set; }


        protected override void Init()
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
                rigidbody.AddForce(- gameObject.transform.forward * 10, ForceMode.Impulse);
            }
            
        }
    }
}