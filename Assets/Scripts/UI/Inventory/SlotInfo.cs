using Managers.Player;
using Player;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class SlotInfo : MonoBehaviour
    {
        public Image image;
        public Text title;
        public Text description;
        public Item item;

        public void Show(Item item)
        {
            this.item = item;
            image.sprite = item.icon;
            image.enabled = true;
            title.text = item.name;
            description.text = item.description;

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            image.sprite = null;
            image.enabled = false;
            title.text = null;
            description.text = null;
            
            gameObject.SetActive(false);
        }

        public void Use()
        {
            if (item != null)
            {
                Hide();
                InventoryManager.instance.Use(item);
            }
        }
    }
}
