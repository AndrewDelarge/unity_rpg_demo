using Actors.Base;
using GameSystems;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Inventory/MeleeWeapon")]
    public class MeleeWeapon : Weapon
    {

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