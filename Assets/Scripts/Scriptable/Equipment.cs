using Actors.Base;
using GameSystems;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
    public class Equipment : Item
    {
        public EquipmentSlot equipmentSlot;
        public int armorModifire;
        public int attackPowerModifire;
        public int staminaModifire;
        public int critChanceModifire;
        public string skinName = "default";
        public SkinnedMeshRenderer mesh;
        public EquipmentMeshRegion[] coveredMeshRegion;
        
        public override void Use(Actor actor)
        {
            base.Use(actor);
            
            GameController.instance.playerManager.equipmentManager.Equip(this);
//            RemoveFromInventory();
        }
        
    }
}
