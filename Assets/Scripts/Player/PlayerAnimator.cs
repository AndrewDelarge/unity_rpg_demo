using System.Collections.Generic;
using Scriptable;
using UnityEngine;

namespace Player
{
    public class PlayerAnimator : CharacterAnimator
    {
        public WeaponAnimation[] weaponAnimations;
        Dictionary<Equipment, AnimationClip[]> weaponAnimationDict;

        private CharacterController _characterController;
        protected override void Start()
        {
            base.Start();


            _characterController = GetComponent<CharacterController>();
            EquipmentManager.instance.onItemEquip += OnItemEquip;
            EquipmentManager.instance.onItemUnequip += OnItemUnequip;
            weaponAnimationDict = new Dictionary<Equipment, AnimationClip[]>();

            foreach (WeaponAnimation anim in weaponAnimations)
            {
                weaponAnimationDict.Add(anim.weapon, anim.clips);
            }
        }

        void Update()
        {
            float speedPercent = _characterController.velocity.magnitude;
            animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
        
            animator.SetBool("inCombat", combat.inCombat);
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
            animator.SetLayerWeight(attackLayerId, 1);
            animator.SetTrigger("attack");
            int animIndedx = Random.Range(0, currentAttackAnimSet.Length);

            overrideController[replaceableAttackClip.name] = currentAttackAnimSet[animIndedx];
        }
        
        
        protected override void OnAttackEnd()
        {
            
            animator.SetLayerWeight(attackLayerId, 0);
        }
    }

    [System.Serializable]
    public struct WeaponAnimation
    {
        public Equipment weapon;
        public AnimationClip[] clips;
    }
}
