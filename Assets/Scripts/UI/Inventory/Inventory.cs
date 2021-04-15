using System.Collections.Generic;
using GameSystems;
using Managers;
using Managers.Player;
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
        public ItemReceived itemReceived; 
        public InventoryStatsView inventoryStatsView; 
        
        public SlotInfo slotInfo;

        private InventoryManager inventory;
        public Interactable lootTarget;
        
        private Slot[] inventorySlots;
        private LootSlot[] lootSlots;

        public void Init()
        {
            PlayerManager playerManager = PlayerManager.Instance();
            inventory = playerManager.inventoryManager;
            lootSlots = lootSlotsHub.GetComponentsInChildren<LootSlot>();
            inventorySlots = slotsHub.GetComponentsInChildren<Slot>();

            itemReceived.Init();
            
            RegisterEvents();
            
            playerManager.onPlayerInited += () => inventoryStatsView.Init(playerManager);
            UpdateUI();
        }

        private void RegisterEvents()
        {
            inventory.onItemsChangedCallback += UpdateUI;
            inventory.onItemAddWithVisual += itemReceived.ShowItem;
            inventory.onLoot += ShowLoot;
            inventory.onLootEnd += HideLoot;
        }
        
        public void ToggleInventory()
        {
            GameManager.Instance().Pause(!inventoryUI.activeSelf);
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
            GameManager.Instance().Pause(true);
            lootTarget.onLootChange += UpdateLoot;
            
            UpdateLoot();
            
            lootUI.SetActive(true);
        }

        public void HideLoot()
        {
            lootUI.SetActive(false);
            GameManager.Instance().Pause(false);
            if (lootTarget != null)
            {
                lootTarget.onLootChange -= UpdateLoot;
            }
        }

    }
}
