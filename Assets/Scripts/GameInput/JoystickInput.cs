using UnityEngine;

namespace GameInput
{
    public class JoystickInput : BaseInput
    {
        [SerializeField]
        private Joystick joystick;
        

        private bool inited = false; 

        private void Start()
        {
            if (joystick != null)
            {
                inited = true;
            }
        }

        private void Update()
        {
            if (! inited)
            {
                return;
            }
            
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;
        }
    }
}