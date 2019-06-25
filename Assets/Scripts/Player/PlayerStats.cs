using Scriptable;
using UnityEngine;

namespace Player
{
    public class PlayerStats : CharacterStats
    {
        // Start is called before the first frame update
        void Start()
        {
            EquipmentManager.instance.onItemEquip += ItemEquip;
            EquipmentManager.instance.onItemUnequip += ItemUnequip;
        }


        void ItemEquip(Equipment item)
        {
            armor.AddModifier(item.armorModifire);
            damage.AddModifier(item.damageModifire);
        }

        void ItemUnequip(Equipment item)
        {
            armor.RemoveModifier(item.armorModifire);
            damage.RemoveModifier(item.damageModifire);
        }
    }
}
