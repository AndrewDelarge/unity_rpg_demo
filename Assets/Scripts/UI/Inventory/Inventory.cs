using Scriptable;
using UnityEngine;

namespace UI.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public GameObject inventoryUI;
        public Transform slotsHub;

        public SlotInfo slotInfo;
        
        private Player.Inventory _inventory;

        Slot[] slots;
        
        // Start is called before the first frame update
        void Start()
        {
            _inventory = Player.Inventory.instance;
            _inventory.onItemChangedCallback += this.UpdateUI;

            slots = slotsHub.GetComponentsInChildren<Slot>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Inventory"))
            {
                inventoryUI.SetActive(! inventoryUI.activeSelf);
            }
        }

        public void ShowInfo(Item item)
        {
            HideInfo();
            slotInfo.Show(item);
        }
        public void HideInfo()
        {
            slotInfo.Hide();
        }
        
        
        void UpdateUI()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (i < _inventory.items.Count)
                {
                    slots[i].AddItem(_inventory.items[i]);
                }
                else
                {
                    slots[i].Clear();
                }
            }
           
        }
        
    }
}
