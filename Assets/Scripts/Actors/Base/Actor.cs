using System.Collections.Generic;
using Actors.Base.Interface;
using GameInput;
using Scriptable;
using UnityEngine;

namespace Actors.Base
{
    [RequireComponent(typeof(Vision), typeof(CommonAnimator))]
    public abstract class Actor : MonoBehaviour
    {
        public GameActor actorScript;
        public GameObject target { get; protected set; }
        public Stats stats{ get; protected set; }
        public Combat combat{ get; protected set; }
        public CommonAnimator animator{ get; protected set; }
        public Vision vision{ get; protected set; }
        public BaseInput input{ get; protected set; }
        
        public IControlable movement { get; protected set; }

        private void Awake()
        {
            Init();
        }

        protected abstract void Init();

        void Ragdoll(GameObject gameObject)
        {
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
//                rigidbody.AddForce(- gameObject.transform.forward * 10, ForceMode.Impulse);
            }
            
            animator.enabled = false;

            
            this.enabled = false;
//            _collider.center = new Vector3();
        }
    }
}