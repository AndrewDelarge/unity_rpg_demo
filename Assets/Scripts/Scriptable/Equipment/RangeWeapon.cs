using Actors.Base;
using GameSystems;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "Range", menuName = "Inventory/Weapon")]
    public class RangeWeapon : Equipment
    {
        [Header("Weapon Settings")]
        public GameObject weaponModel;
        public WeaponType type;
        public int damage = 1; 
        
        [Header("Particles")]
        public GameObject trail;
        public Vector3 trailSpawnLocalPos = Vector3.zero;
        public Vector3 trailSpawnLocalRotation = Vector3.zero;
        public Vector3 trailSpawnLocalScale = Vector3.one;


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