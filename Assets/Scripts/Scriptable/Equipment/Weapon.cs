using Actors.Base;
using GameSystems;
using UnityEngine;

namespace Scriptable
{
    public class Weapon : Equipment
    {
        [Header("Weapon Settings")]

        public GameObject weaponModel;
        public WeaponType type;
        public int damage = 1; 

        [Header("Position And Rotation")]
        public Vector3 localPosition = Vector3.zero;
        public Vector3 localRotation = Vector3.zero;

        
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