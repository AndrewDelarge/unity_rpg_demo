using UnityEngine;

namespace Gameplay.Objects
{
    public class LockAt : MonoBehaviour
    {
        public Transform target;


        private void FixedUpdate()
        {
            transform.LookAt(target);

        }
    }
}