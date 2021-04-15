using Gameplay.Actors.Base;
using UnityEngine;

namespace GameSystems.Input
{
    public abstract class BaseInput : MonoBehaviour
    {
        public float vertical = 0f;
        public float horizontal = 0f;

        
        public virtual void Init()
        {
            
        }
        public bool IsSomeDirection()
        {
            return vertical != 0f || horizontal != 0f;
        }
        
    }
}