using Player;
using UnityEngine;

namespace UI.Hud
{
    public class UpperPanel : MonoBehaviour
    {
        public delegate void OnInventoryButtonClick();

        public OnInventoryButtonClick onInventoryButtonClick;

        public System.Action onMenuButtonClick;

        public void InventoryButtonClick()
        {
            if (onInventoryButtonClick != null)
            {
                onInventoryButtonClick.Invoke();
            }
        }


        public void MenuButtonClick()
        {
        }
    }
}
