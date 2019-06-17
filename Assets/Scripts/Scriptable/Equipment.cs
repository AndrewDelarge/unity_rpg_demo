using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Equipment")]
    public class Equipment : Item
    {
        public EquipmentSlot equipmentSlot;
        public int armorModifire;
        public int damageModifire;

        public SkinnedMeshRenderer mesh;

        public EquipmentMeshRegion[] coveredMeshRegion;
        
        public override void Use()
        {
            base.Use();
            
            Player.EquipmentManager.instance.Equip(this);
//            RemoveFromInventory();
        }
        
    }
}
