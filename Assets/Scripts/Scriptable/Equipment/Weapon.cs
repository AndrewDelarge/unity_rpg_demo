using Actors.Base;
using GameSystems;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
    public class Weapon : Equipment
    {
        public WeaponType type;
        public int damage = 1; 
        public GameObject trail;
        
        public override void Use(Actor actor)
        {
            // TODO hardcoded tag
            if (actor.CompareTag("Player"))
            {
                GameController.instance.playerManager.equipmentManager.Equip(this);
            }
        }
    }
}