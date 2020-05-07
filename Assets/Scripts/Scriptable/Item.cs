using Actors.Base;
using Managers.Player;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
    public class Item : ScriptableObject
    {
        [Header("Item common")]
        public new string name = "New Item";
        public string description = "Some item description";
        public Sprite icon = null;
        public GameObject gameObject;

        public virtual void Use(Actor actor)
        {
            Debug.Log("Using " + name);
        }
    }
}
