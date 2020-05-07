using Actors.Base;
using GameSystems;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
    public class Equipment : Item
    {
        [Header("Equipment Settings")]
        public EquipmentSlot equipmentSlot;
        public string skinName = "default";

        [Header("Equipment Stats")]
        public int armorModifire;
        public int attackPowerModifire;
        public int staminaModifire;
        public int critChanceModifire;
        
        public override void Use(Actor actor)
        {
            base.Use(actor);
            
            GameController.instance.playerManager.equipmentManager.Equip(this);
//            RemoveFromInventory();
        }
        
    }
}
