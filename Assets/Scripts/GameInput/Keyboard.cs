using UnityEngine;

namespace GameInput
{
    public class Keyboard : BaseInput
    {

        public KeyCode actionKey = KeyCode.E;
        public KeyCode secondKey = KeyCode.Q;
        
        
        
        private void Update()
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical   = Input.GetAxis("Vertical");
        }
    }
}