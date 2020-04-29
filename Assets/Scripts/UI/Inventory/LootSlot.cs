using Managers.Player;
using Scriptable;

namespace UI.Inventory
{
    public class LootSlot : Slot
    {
        public new void AddItem(Item newItem)
        {
            item = newItem;

            icon.sprite = item.icon;
            icon.enabled = true;
        }

        public new void Clear()
        {
            item = null;

            icon.sprite = null;
            icon.enabled = false;
        }

        public new void UseItem()
        {
            if (item != null)
            {
                if (InventoryManager.instance.Add(item))
                {
                    inventoryUI.lootTarget.RemoveFromLoot(item);
                }
            }
        }

    }
}
