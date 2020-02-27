using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
    public class Weapon : Equipment
    {
        [SerializeField] private WeaponType type;
        
    }
}