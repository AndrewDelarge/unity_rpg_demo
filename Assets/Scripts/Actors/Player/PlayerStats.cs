using Actors.Base;
using Managers.Player;
using Scriptable;
using UnityEngine;

namespace Actors.Player
{
    public class PlayerStats : Stats
    {
        private EquipmentManager equipmentManager;
        
        public override void Init()
        {
            base.Init();
            equipmentManager = GameController.instance.playerManager.equipmentManager; 
            
            equipmentManager.onItemEquip += ItemEquip;
            equipmentManager.onItemUnequip += ItemUnequip;
        }

        void ItemEquip(Equipment item)
        {
            armor.AddModifier(item.armorModifire);
            attackPower.AddModifier(item.attackPowerModifire);
            stamina.AddModifier(item.staminaModifire);
        }

        void ItemUnequip(Equipment item)
        {
            armor.RemoveModifier(item.armorModifire);
            attackPower.RemoveModifier(item.attackPowerModifire);
            stamina.RemoveModifier(item.staminaModifire);
        }


        public override int GetDamageValue()
        {

            int damage = GetWeaponDamage();
            damage += ConvertAPToDamage(attackPower);
            int chance = Mathf.FloorToInt(GetCriticalChance());
            int throwed = Random.Range(0, 99);

            if (throwed <= chance)
            {
                damage = Mathf.FloorToInt(damage * CRIT_MULTIPLIER);
            }

            // Damage Randomising 
            damage = Mathf.FloorToInt(damage * UnityEngine.Random.Range(.9f, 1.1f));

            return damage;
        }


        private int GetWeaponDamage()
        {
            Weapon weapon = equipmentManager.GetCurrentWeapon();

            if (weapon == null)
            {
                return 0;
            }

            return weapon.damage;
        }
    }
}