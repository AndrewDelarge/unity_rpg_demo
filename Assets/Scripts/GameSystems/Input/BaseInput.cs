using Gameplay.Actors.Base;
using UnityEngine;

namespace GameSystems.Input
{
    public abstract class BaseInput : MonoBehaviour
    {
        protected float vertical = 0f;
        protected float horizontal = 0f;

        public float Vertical => vertical;
        public float Horizontal => horizontal;

        public virtual void Init()
        {
            
        }
        public bool IsSomeDirection()
        {
            return vertical != 0f || horizontal != 0f;
        }
        
    }
}