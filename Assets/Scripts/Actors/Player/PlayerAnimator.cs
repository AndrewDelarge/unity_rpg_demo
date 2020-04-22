using System.Collections.Generic;
using Actors.Base;
using Actors.Base.Interface;
using Scriptable;
using UnityEngine;

namespace Actors.Player
{
    public class PlayerAnimator : CommonAnimator
    {
        public WeaponAnimation[] weaponAnimations;
        public AnimationClip defaultCombatIdleAnim;
        public float defaultAttackSpeed = 1f;
        Dictionary<WeaponType, WeaponAnimation> weaponAnimationDict;
        
        public override void Init(Base.Combat actCombat, IControlable actMovement, Stats actStats)
        {
            base.Init(actCombat, actMovement, actStats);
            
            GameController.instance.playerManager.equipmentManager.onWeaponEquip += OnWeaponEquip;
            GameController.instance.playerManager.equipmentManager.onWeaponUnequip += OnWeaponUnequip;
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
                 animator.SetFloat("attackSpeedMultiplier", weaponAnimationDict[equipment.type].animationSpeed);
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