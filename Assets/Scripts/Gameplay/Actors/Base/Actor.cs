using System;
using Gameplay.Actors.Base.Interface;
using GameSystems.Input;
using Scriptable;
using UnityEngine;

namespace Gameplay.Actors.Base
{
    [RequireComponent(typeof(Vision), typeof(CommonAnimator))]
    public abstract class Actor : MonoBehaviour
    {
        public CommonAnimator animator{ get; protected set; }
        public IControlable movement { get; protected set; }
        public BaseInput input{ get; protected set; }
        public GameActor actorScript;
        public Combat combat{ get; protected set; }
        public Vision vision{ get; protected set; }
        public Stats stats{ get; protected set; }


        public virtual void Init()
        {
            animator     = GetComponent<CommonAnimator>();
            movement     = GetComponent<IControlable>();
            vision       = GetComponent<Vision>();
            combat       = GetComponent<Combat>();
            input        = GetComponent<BaseInput>();
            stats        = GetComponent<Stats>();
            
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

        protected Vector3 GetLastDamageDealerPosition()
        {
            Vector3 damagePosition = transform.TransformPoint(Vector3.forward);

            if (stats.lastDamage.GetOwner() != null)
            {
                damagePosition = stats.lastDamage.GetOwner().transform.position;
            }

            return damagePosition;
        }

        protected Vector3 GetDirection(Vector3 point)
        {
            Vector3 heading = point - transform.position;
            float distance = heading.magnitude;
            return heading / distance;
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

        public virtual void PushBack(Vector3 pusherPos, float force = 1)
        {
            throw new NotImplementedException("Implement push back method!");
        }
    }
}