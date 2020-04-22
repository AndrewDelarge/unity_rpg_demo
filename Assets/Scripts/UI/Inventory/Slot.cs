using Managers.Player;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class Slot : MonoBehaviour
    {
        public Image icon;
        public Button remove;

        protected Item item;
        protected Inventory inventoryUI;
        
        private void Start()
        {
            inventoryUI = GetComponentInParent<Inventory>();
        }

        public void AddItem(Item newItem)
        {
            item = newItem;

            icon.sprite = item.icon;
            icon.enabled = true;
            remove.interactable = true;
        }

        public void Clear()
        {
            item = null;

            icon.sprite = null;
            icon.enabled = false;
            remove.interactable = false;
        }

        public void OnDropClick()
        {
            InventoryManager.instance.Drop(item);
            InventoryManager.instance.Remove(item);
        }

        public void UseItem()
        {
            if (item != null)
            {
                inventoryUI.slotInfo.Show(item);
//                item.Use();
            }
        }
    }
}
