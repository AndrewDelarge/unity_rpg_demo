using UnityEngine;

namespace UI.Hud
{
    public class ActionBar : MonoBehaviour
    {
        
        public Joystick joystick;
        public Joystick bowControlStick;

        
        public delegate void OnActionKeyClick();
        public delegate void OnSecKeyClick();
        public OnActionKeyClick onActionKeyClick;
        public OnSecKeyClick onSecKeyClick;

        public void Init()
        {
            onActionKeyClick = null;
            onSecKeyClick = null;
        }
        
        public void OnActionClick()
        {
            onActionKeyClick?.Invoke();
        }
        
        public void OnSecClick()
        {
            onSecKeyClick?.Invoke();
        }
    }
}
