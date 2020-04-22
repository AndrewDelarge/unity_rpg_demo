using Scriptable;
using UnityEngine;

namespace Player
{
    public class PlayerStats : CharacterStats
    {
        void Start()
        {
//            EquipmentManager.instance.onItemEquip += ItemEquip;
//            EquipmentManager.instance.onItemUnequip += ItemUnequip;
        }

        void ItemEquip(Equipment item)
        {
            armor.AddModifier(item.armorModifire);
            damage.AddModifier(item.attackPowerModifire);
        }

        void ItemUnequip(Equipment item)
        {
            armor.RemoveModifier(item.armorModifire);
            damage.RemoveModifier(item.attackPowerModifire);
        }

        public override void Die()
        {
            base.Die();
            
            PlayerManager.instance.RestartScene();
        }
    }
}
