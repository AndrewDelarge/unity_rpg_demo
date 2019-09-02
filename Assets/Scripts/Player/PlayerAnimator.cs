using System.Collections.Generic;
using Scriptable;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : CharacterAnimator
    {
        public WeaponAnimation[] weaponAnimations;
        Dictionary<Equipment, AnimationClip[]> weaponAnimationDict;
        
        protected override void Start()
        {
            base.Start();

            EquipmentManager.instance.onItemEquip += OnItemEquip;
            EquipmentManager.instance.onItemUnequip += OnItemUnequip;
            weaponAnimationDict = new Dictionary<Equipment, AnimationClip[]>();

            foreach (WeaponAnimation anim in weaponAnimations)
            {
                weaponAnimationDict.Add(anim.weapon, anim.clips);
            }
        }

        void OnItemEquip(Equipment equipment)
        {
            if (equipment.equipmentSlot == EquipmentSlot.Weapon)
            {
                if (weaponAnimationDict.ContainsKey(equipment))
                {
                    currentAttackAnimSet = weaponAnimationDict[equipment];
                }
            }
        }

        void OnItemUnequip(Equipment equipment)
        {
            if (equipment.equipmentSlot == EquipmentSlot.Weapon)
            {
                currentAttackAnimSet = defaultAttackAnimSet;
            }
        }
        
        protected override void OnAttack()
        {
            animator.SetTrigger("attack");
            int animIndedx = Random.Range(0, currentAttackAnimSet.Length);

            overrideController[replaceableAttackClip.name] = currentAttackAnimSet[animIndedx];
        }
    }

    [System.Serializable]
    public struct WeaponAnimation
    {
        public Equipment weapon;
        public AnimationClip[] clips;
    }
}
