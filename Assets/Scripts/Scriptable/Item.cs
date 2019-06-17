using Player;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        public new string name = "New Item";
        public string description = "Some item description";
        public Sprite icon = null;
        public GameObject gameObject;
        public bool isDefaultItem = false;

        public virtual void Use()
        {
            Debug.Log("Using " + name);
        }

        public void RemoveFromInventory()
        {
            Inventory.instance.Remove(this);
        }
    }
}
