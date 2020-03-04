using Actors.Base;
using UnityEngine;

namespace GameInput
{
    public abstract class BaseInput : MonoBehaviour
    {
        protected Transform target = null;
        
        public float vertical = 0f;
        public float horizontal = 0f;

        public event System.Action OnAttackPressed;

        
        public virtual void Init(Actor actor)
        {
            
        }

        public bool IsSomeDirection()
        {
            return vertical != 0f || horizontal != 0f;
        }

        public Transform GetTarget()
        {
            return target;
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public bool HasTarget()
        {
            return target != null;
        }

        public void Attack()
        {
            if (OnAttackPressed != null)
            {
                OnAttackPressed.Invoke();
            }
        }
        
    }
}