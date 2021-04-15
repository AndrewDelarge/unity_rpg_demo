using Gameplay.Actors.Base;
using GameSystems;
using Managers;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "Range", menuName = "Inventory/Range Weapon")]
    public class RangeWeapon : Weapon
    {
        [Header("Particles")] 
        public GameObject projectile;


        public override void Use(Actor actor)
        {
            // TODO hardcoded tag
            if (actor.CompareTag("Player"))
            {
                PlayerManager.Instance().equipmentManager.Equip(this);
            }
        }
    }
}