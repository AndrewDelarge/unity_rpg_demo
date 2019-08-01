using UnityEngine;

namespace UI.Hud
{
    public class ActionBar : MonoBehaviour
    {
        public delegate void OnActionKeyClick();

        public OnActionKeyClick onActionKeyClick;
        
        
        
        
        public void OnActionClick()
        {
            if (onActionKeyClick != null)
            {
                onActionKeyClick.Invoke();
            }
        }
    }
}
