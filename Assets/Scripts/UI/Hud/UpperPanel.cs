using Player;
using UnityEngine;

namespace UI.Hud
{
    public class UpperPanel : MonoBehaviour
    {
        public delegate void OnInventoryButtonClick();

        public OnInventoryButtonClick onInventoryButtonClick;


        public void InventoryButtonClick()
        {
            if (onInventoryButtonClick != null)
            {
                onInventoryButtonClick.Invoke();
            }
        }
    }
}
