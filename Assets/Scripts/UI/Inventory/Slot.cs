using System;
using Scriptable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image icon;
        
        public Button remove;

        private Item item;

        private Inventory inventoryUI;
        
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
            Player.Inventory.instance.Drop(item);
            Player.Inventory.instance.Remove(item);
        }

        public void UseItem()
        {
            if (item != null)
            {
                item.Use();
            }
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (item != null)
            {
                inventoryUI.slotInfo.Show(item);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.slotInfo.Hide();
        }
    }
}
