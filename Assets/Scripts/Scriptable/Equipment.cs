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
        
        public override void Use()
        {
            base.Use();
            
            GameController.instance.playerManager.equipmentManager.Equip(this);
//            RemoveFromInventory();
        }
        
    }
}
