using System.Collections.Generic;
using Managers.Player;
using Player;
using Scriptable;
using UnityEngine;

namespace UI.Inventory
{
    public class Inventory : MonoBehaviour
    {
        public GameObject inventoryUI;
        public GameObject lootUI;
        public Transform slotsHub;
        public Transform lootSlotsHub;

        public SlotInfo slotInfo;

        private InventoryManager inventory;
        public Interactable lootTarget;
        
        private Slot[] inventorySlots;
        private LootSlot[] lootSlots;

        // Start is called before the first frame update
        public void Init()
        {
            inventory = GameController.instance.playerManager.inventoryManager;
            inventory.onItemChangedCallback += UpdateUI;

            inventorySlots = slotsHub.GetComponentsInChildren<Slot>();
            lootSlots = lootSlotsHub.GetComponentsInChildren<LootSlot>();
            
            inventory.onLoot += ShowLoot;
            inventory.onLootEnd += HideLoot;

            UpdateUI();
        }

        void FixedUpdate()
        {
            if (Input.GetButtonDown("Inventory"))
            {
                inventoryUI.SetActive(!inventoryUI.activeSelf);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                inventoryUI.SetActive(false);
                lootUI.SetActive(false);
                PlayerManager.instance.Pause(false);
            }
        }

        public void ToggleInventory()
        {
            GameController.instance.Pause(!inventoryUI.activeSelf);
            inventoryUI.SetActive(!inventoryUI.activeSelf);
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
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (i < inventory.items.Count)
                {
                    inventorySlots[i].AddItem(inventory.items[i]);
                }
                else
                {
                    inventorySlots[i].Clear();
                }
            }
        }

        void UpdateLoot()
        {
            List<Item> loot = lootTarget.GetLoot();
            
            for (int i = 0; i < lootSlots.Length; i++)
            {
                if (i < loot.Count)
                {
                    Debug.Log(loot[i]);

                    lootSlots[i].AddItem(loot[i]);
                }
                else
                {
                    lootSlots[i].Clear();
                }
            }
        }
        
        void ShowLoot(Interactable loot)
        {
            lootTarget = loot;
            GameController.instance.Pause(true);
            lootTarget.onLootChange += UpdateLoot;
            
            UpdateLoot();
            
            lootUI.SetActive(true);
        }

        public void HideLoot()
        {
            lootUI.SetActive(false);
            GameController.instance.Pause(false);
            if (lootTarget != null)
            {
                lootTarget.onLootChange -= UpdateLoot;
            }
        }

    }
}
