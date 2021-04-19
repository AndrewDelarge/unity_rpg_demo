using System.Collections.Generic;
using GameSystems;
using Managers;
using Managers.Player;
using Scriptable;
using UI.Base;
using UI.Hud;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class Inventory : UIWindow
    {
        public GameObject inventoryUI;
        public GameObject lootUI;
        public Transform slotsHub;
        public Transform lootSlotsHub;
        public Button substrate;
        
        public ItemReceived itemReceived; 
        public InventoryStatsView inventoryStatsView;
        public SlotInfo slotInfo;
        public QuestPanel questPanel;
        public Interactable lootTarget;

        private InventoryManager inventoryManager;

        private Slot[] inventorySlots;
        private LootSlot[] lootSlots;

        public void Init()
        {
            inventoryManager = InventoryManager.Instance();
            
            lootSlots = lootSlotsHub.GetComponentsInChildren<LootSlot>();
            inventorySlots = slotsHub.GetComponentsInChildren<Slot>();

            itemReceived.Init();
            
            RegisterEvents();
            
            UpdateUI();
        }


        public override void Open()
        {
            base.Open();
            GameManager.Instance().Pause(true);
        }

        public override void Close()
        {
            base.Close();
            GameManager.Instance().Pause(false);
        }

        private void RegisterEvents()
        {
            PlayerManager.Instance().onPlayerInited += inventoryStatsView.Init;

            inventoryManager.onItemsChangedCallback += UpdateUI;
            inventoryManager.onItemAddWithVisual += itemReceived.ShowItem;
            
            inventoryManager.onLoot += ShowLoot;
            inventoryManager.onLootEnd += HideLoot;
            
            substrate.onClick.AddListener(() => UIManager.Instance().CloseWindow(UIManager.UIWindows.Inventory));
        }
        
        public void ToggleInventory()
        {
            GameManager.Instance().Pause(!inventoryUI.activeSelf);
//            inventoryUI.SetActive(!inventoryUI.activeSelf);
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
                if (i < inventoryManager.items.Count)
                    inventorySlots[i].AddItem(inventoryManager.items[i]);
                else
                    inventorySlots[i].Clear();
            }
        }

        void UpdateLoot()
        {
            List<Item> loot = lootTarget.GetLoot();
            
            for (int i = 0; i < lootSlots.Length; i++)
            {
                if (i < loot.Count)
                    lootSlots[i].AddItem(loot[i]);
                else
                    lootSlots[i].Clear();
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
                lootTarget.onLootChange -= UpdateLoot;
        }

    }
}
