using UnityEngine;

namespace UI.Base
{
    public class UIView : MonoBehaviour
    {
        public bool IsActive => isActive;

        private bool isActive;
        
        public virtual void Open()
        {
            isActive = true;
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            isActive = false;
            gameObject.SetActive(false);
        }
    }
}