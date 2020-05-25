using System;
using System.Collections;
using System.Collections.Generic;
using Actors.Base.Interface;
using GameSystems.Input;
using Scriptable;
using UnityEngine;

namespace Actors.Base
{
    [RequireComponent(typeof(Vision), typeof(CommonAnimator))]
    public abstract class Actor : MonoBehaviour
    {
        public GameActor actorScript;
        public Stats stats{ get; protected set; }
        public Combat combat{ get; protected set; }
        public CommonAnimator animator{ get; protected set; }
        public Vision vision{ get; protected set; }
        public BaseInput input{ get; protected set; }
        
        public IControlable movement { get; protected set; }


        public virtual void Init()
        {
            animator = GetComponent<CommonAnimator>();
            vision = GetComponent<Vision>();
            combat = GetComponent<Combat>();
            input = GetComponent<BaseInput>();
            stats = GetComponent<Stats>();
            movement = GetComponent<IControlable>();
            stats.Init();
            input.Init(this);
            
            combat.Init(stats, input);
            movement.Init(stats, input);
            animator.Init(combat, movement, stats);

            stats.onDied += Die;
        }
        
        protected virtual void Die(GameObject go)
        {
            enabled = false;
            animator.Disable();
            movement.Disable();
            input.enabled = false;
        }
        
        public bool IsEnemy(Actor actor)
        {
            return actorScript.fraction.FractionInEnemies(actor.actorScript.fraction);
        }

        public bool IsFriend(Actor actor)
        {
            return actorScript.fraction.GetInstanceID() == actor.actorScript.fraction.GetInstanceID();
        }

        public bool InCombat()
        {
            return combat.InCombat();
        }

        public bool IsDead()
        {
            return stats.IsDead();
        }

        public virtual void MeleeAttack()
        {
            throw new NotImplementedException("Implement Melee Attack method!");
        }
        
        public virtual void MeleeAttack(IHealthable actor)
        {
            throw new NotImplementedException("Implement Melee Attack method!");
        }

        public virtual void Aim(Vector3 point)
        {
            throw new NotImplementedException("Implement Aim method!");
        }

        public virtual void StopAim()
        {
            throw new NotImplementedException("Implement StopAim method!");
        }

        public virtual void RangeAttack()
        {
            throw new NotImplementedException("Implement RangeAttack method!");
        }
        
        public virtual void Dash()
        {
            throw new NotImplementedException("Implement Dash method!");
        }
    }
}