using System.Collections.Generic;
using GameSystems;
using Scriptable;
using UnityEngine;

namespace Managers.Player
{
    public class InventoryManager : MonoBehaviour
    {
        #region Singleton
    
        public static InventoryManager instance;

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogWarning("More that one inventory founded");
                return;
            }

            instance = this;
        }
    
        #endregion

        public delegate void OnItemChanged();
        public delegate void OnItemAddWithVisual(Item item);

        public delegate void OnItemUse(Item item);
        public delegate void OnLoot(Interactable lootTarget);

        public OnItemUse onItemUse;
        public OnLoot onLoot;
        public OnItemChanged onItemsChangedCallback;
        public OnItemAddWithVisual onItemAddWithVisual;
        public System.Action onLootEnd;

        public int space = 20;
        public List<Item> items;
        
        private GameObject defaultItemGameObject;
        private EquipmentManager equipmentManager;
        public void Init()
        {
            equipmentManager = GameController.instance.playerManager.equipmentManager;
            equipmentManager.onItemUnequip += AddAfterUnequip;
            equipmentManager.onItemEquip += Remove;

            onItemUse = null;
            onLoot = null;
            onItemsChangedCallback = null;
            onLootEnd = null;
            
            defaultItemGameObject = Resources.Load<GameObject>("Equipments/EmptyItem");
        }

        public bool Add(Item item, bool visual = false)
        {
            if (items.Count >= space)
            {
                // Not enough space
                return false;
            }
            items.Add(item);

            if (visual)
            {
                onItemAddWithVisual?.Invoke(item);
            }
            
            onItemsChangedCallback?.Invoke();
            
            return true;
        }

        protected void AddAfterUnequip(Item item)
        {
            if (! Add(item))
            {
                Drop(item);
            }
        }

        public void Remove(Item item)
        {
            items.Remove(item);
            
            onItemsChangedCallback?.Invoke();
        }

        public void Loot()
        {
            // TODO 
//            Interactable target = PlayerManager.instance.player.target;
//            
//            if (onLoot != null && target != null)
//            {
//                onLoot.Invoke(target);
//            }
        }
        
        public void Loot(Interactable lootTarget)
        {
            onLoot?.Invoke(lootTarget);
            
        }

        public void StopLoot()
        {
            onLootEnd?.Invoke();
        }
        
        public void Drop(Item item)
        {
            // TODO
//            if (item.gameObject != null)
//            {
//                Transform playerTransform = PlayerManager.instance.player.transform;
//                Instantiate<GameObject>(item.gameObject, new Vector3(playerTransform.position.x - 1, playerTransform.position.y + 2, playerTransform.position.z -1), Quaternion.identity);
//            }
        }

        public void Use(Item item)
        {
            item.Use(GameController.instance.playerManager.GetPlayer());

            onItemUse?.Invoke(item);
        }
    }
}
