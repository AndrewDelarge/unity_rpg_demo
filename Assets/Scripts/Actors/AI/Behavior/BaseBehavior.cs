using System.Collections;
using Actors.Base;
using Actors.Base.Interface;
using GameSystems.Input;
using UnityEngine;

namespace Actors.AI.Behavior
{
    public enum BehaviorType
    {
        Default, Warior
    }

    public enum BehaviorState
    {
        Idle, Patrol, Attack, Fear, Stan, Dead, ReturnToIdle
    }
    
    public abstract class BaseBehavior
    {
        protected Actor actor;
        protected BaseInput input;
        protected IHealthable attackTarget;
        protected BehaviorState state;
        
        public virtual void Init(Actor baseActor)
        {
            actor = baseActor;
            input = actor.input;
        }

        public abstract IEnumerator AIUpdate();
        
        public virtual void Defence(Actor attackedBy)
        {
            if (! actor.InCombat())
            {
                SetAttackTarget(attackedBy.stats);
            }
        }

        public void SetAttackTarget(IHealthable target)
        {
            attackTarget = target;
            state = BehaviorState.Attack;
        }
        
        protected Actor GetNextEnemy()
        {
            foreach (Actor npcActor in actor.vision.actors)
            {
                if (actor.IsEnemy(npcActor))
                {
                    return npcActor;
                }
            }

            return null;
        }
        
        protected Actor GetClosestFriend()
        {
            foreach (Actor npcActor in actor.vision.actors)
            {
                if (actor.IsFriend(npcActor))
                {
                    return npcActor;
                }
            }

            return null;
        }

        public abstract void Idle();
        public abstract void ReturnToIdle();

        protected abstract void Patrol(Vector3[] points = null);

        protected abstract void Attack();

        public BehaviorState GetState()
        {
            return state;
        }
        
    }
}