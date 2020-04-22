using UnityEngine;

namespace UI.Hud
{
    public class ActionBar : MonoBehaviour
    {
        
        public Joystick joystick;

        
        public delegate void OnActionKeyClick();
        public OnActionKeyClick onActionKeyClick;

        public void Init()
        {
            onActionKeyClick = null;
        }
        
        public void OnActionClick()
        {
            onActionKeyClick?.Invoke();
        }
    }
}
