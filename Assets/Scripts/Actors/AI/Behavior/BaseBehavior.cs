using Actors.Base;
using GameInput;
using UnityEngine;

namespace Actors.AI.Behavior
{
    public enum BehaviorType
    {
        Default, Warior
    }

    public enum BehaviorState
    {
        Idle, Patrol, Attack, Fear, Stan, Dead
    }
    
    public abstract class BaseBehavior
    {
        protected Actor actor;
        protected BaseInput input;
        protected Actor attackTarget;
        protected BehaviorState state;
        
        public virtual void Init(Actor baseActor, BaseInput baseInput)
        {
            actor = baseActor;
            input = baseInput;
        }

        public abstract void AIUpdate();
        
        public virtual void Defence(Actor attackedBy)
        {
            if (! actor.InCombat())
            {
                SetAttackTarget(attackedBy);
            }
        }

        public void SetAttackTarget(Actor target)
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

        protected abstract void Idle();

        protected abstract void Patrol(Vector3[] points = null);

        protected abstract void Attack();

        public BehaviorState GetState()
        {
            return state;
        }
        
    }
}