using System.Collections.Generic;
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

        public delegate void OnItemUse(Item item);
        public delegate void OnLoot(Interactable lootTarget);

        public OnItemUse onItemUse;
        public OnLoot onLoot;
        public OnItemChanged onItemChangedCallback;
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
            onItemChangedCallback = null;
            onLootEnd = null;
            
            defaultItemGameObject = Resources.Load<GameObject>("Equipments/EmptyItem");
        }

        public bool Add(Item item)
        {
            if (items.Count >= space)
            {
                // Not enough space
                return false;
            }
            items.Add(item);

            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
            
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
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
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
            if (onLoot != null)
            {
                onLoot.Invoke(lootTarget);
            }
        }

        public void StopLoot()
        {
            if (onLootEnd != null)
            {
                onLootEnd();
            }
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
            item.Use();

            onItemUse?.Invoke(item);
        }
    }
}
