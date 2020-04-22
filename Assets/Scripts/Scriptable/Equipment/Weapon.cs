using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
    public class Weapon : Equipment
    {
        public WeaponType type;
        public int damage = 1; 
        public override void Use()
        {
            GameController.instance.playerManager.equipmentManager.Equip(this);
        }
    }
}