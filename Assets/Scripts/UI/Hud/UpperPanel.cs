using Managers;
using UnityEngine;

namespace UI.Hud
{
    public class UpperPanel : MonoBehaviour
    {
        public void InventoryButtonClick()
        {
            UIManager.Instance().OpenWindow(UIManager.UIWindows.Inventory);
        }

        public void OnPauseButtonClick()
        {
            UIManager.Instance().OpenWindow(UIManager.UIWindows.Pause);
        }
    }
}
