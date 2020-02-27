using UnityEngine;

namespace GameInput
{
    public abstract class BaseInput : UnityEngine.MonoBehaviour
    {
        public float vertical = 0f;
        public float horizontal = 0f;


        public bool IsSomeDirection()
        {
            return vertical != 0f || horizontal != 0f;
        }
    }
}