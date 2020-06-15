using System.Collections;
using Actors.Base;
using Actors.Base.Interface;
using GameSystems;
using GameSystems.Input;
using Managers;
using UnityEngine;

namespace Actors.AI.Behavior
{
    public enum BehaviorType
    {
        Default, Warior, Range
    }

    public enum BehaviorState
    {
        Idle, Patrol, Attack, Fear, Stan, Dead, ReturnToIdle, Chasing
    }
    
    public abstract class BaseBehavior
    {
        protected Actor actor;
        protected BaseInput input;
        protected IHealthable attackTarget;
        protected BehaviorState state;
        protected AIActorsManager actorsManager;
        protected bool hasAttackToken;
        public virtual void Init(Actor baseActor)
        {
            actor = baseActor;
            input = actor.input;
            actor.stats.onDied += diedObject => OnActorDied(diedObject);

        }
        
        protected virtual void SetState(BehaviorState state)
        {
            this.state = state;
        }

        void OnActorDied(GameObject died)
        {
            SetState(BehaviorState.Dead);
            if (hasAttackToken)
            {
                ReturnAttackToken();
            }
        }
        public abstract IEnumerator AIUpdate();
        
        public virtual void Defence(Actor attackedBy)
        {
            if (attackTarget == null)
            {
                SetAttackTarget(attackedBy.stats);
            }
        }

        public void SetAttackTarget(IHealthable target)
        {
            attackTarget = target;
            state = BehaviorState.Chasing;
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

        protected abstract void Attack();

        public BehaviorState GetState()
        {
            return state;
        }

        public bool GetAttackToken()
        {
            if (actorsManager == null)
            {
                actorsManager = GameController.instance.sceneController.GetActorsManager();
            }

            return hasAttackToken = actorsManager.GetAttackToken();
        }

        public void ReturnAttackToken()
        {
            hasAttackToken = false;
            actorsManager.ReturnAttackToken();
        }


        public void ShowPosition(Vector3 position)
        {
            GameController.Instantiate(Resources.Load("System/Point") as GameObject, position, Quaternion.identity);
        }
    }
}