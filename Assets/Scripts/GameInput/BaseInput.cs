using Actors.Base;
using UnityEngine;

namespace GameInput
{
    public abstract class BaseInput : MonoBehaviour
    {
        protected Vector3 target = Vector3.zero;
        
        public float vertical = 0f;
        public float horizontal = 0f;


        public virtual void Init(Actor actor)
        {
            
        }

        public bool IsSomeDirection()
        {
            return vertical != 0f || horizontal != 0f;
        }

        public Vector3 GetTarget()
        {
            return target;
        }

        public void SetTarget(Vector3 target)
        {
            this.target = target;
        }
    }
}