using System.Collections.Generic;
using Gameplay.Actors.Base;
using Gameplay.Actors.Base.Interface;
using GameSystems;
using Managers;
using Scriptable;
using UnityEngine;

namespace Gameplay.Actors.Player
{
    public class PlayerAnimator : CommonAnimator
    {
        public WeaponAnimation[] weaponAnimations;
        public AnimationClip defaultCombatIdleAnim;
        public float defaultAttackSpeed = 1f;
        Dictionary<WeaponType, WeaponAnimation> weaponAnimationDict;
        
        

        public override void Init(IControlable actMovement)
        {
            base.Init(actMovement);
            
            PlayerManager.Instance().equipmentManager.onMeleeWeaponEquip += OnWeaponEquip;
            PlayerManager.Instance().equipmentManager.onMeleeWeaponUnequip += OnWeaponUnequip;
            weaponAnimationDict = new Dictionary<WeaponType, WeaponAnimation>();

            foreach (WeaponAnimation anim in weaponAnimations)
            {
                weaponAnimationDict.Add(anim.weapon, anim);
            }
        }

        void OnWeaponEquip(Weapon equipment)
        {
             if (weaponAnimationDict.ContainsKey(equipment.type))
             {
                 currentAttackAnimSet = weaponAnimationDict[equipment.type].clips;
                 overrideController[defaultCombatIdleAnim.name] = weaponAnimationDict[equipment.type].idle;
                 animator.SetFloat("attackSpeedMultiplier", combat.commonCombatSpeedMultiplier);
             }
        }


        void OnWeaponUnequip(Weapon equipment)
        {
            currentAttackAnimSet = defaultAttackAnimSet;
            overrideController[defaultCombatIdleAnim.name] = defaultCombatIdleAnim;
            animator.SetFloat("attackSpeedMultiplier", defaultAttackSpeed);
        }
    }
    
    [System.Serializable]
    public struct WeaponAnimation
    {
        // delays and speed len == clips len!
        public WeaponType weapon;
        public AnimationClip[] clips;
        public AnimationClip idle;
        public float animationSpeed;
    }
}