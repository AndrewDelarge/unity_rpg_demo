using System.Collections;
using Gameplay.Actors.Base;
using Gameplay.Actors.Base.Interface;
using GameSystems;
using GameSystems.Input;
using Managers;
using Managers.Scenes;
using UnityEngine;

namespace Gameplay.Actors.AI.Behavior
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
        protected AIActorsController actorsController;
        protected bool hasAttackToken;
        protected float stoppingDistance = 1.5f;
        
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

        protected virtual void OnActorDied(GameObject died)
        {
            SetState(BehaviorState.Dead);
            if (hasAttackToken)
            {
                ReturnAttackToken();
            }
        }
        public abstract void AIUpdate();
        
        public virtual void Defence(Actor attackedBy)
        {
            if (attackTarget != null)
            {
                return;
            }

            
            actor.movement.Follow(attackedBy.transform, stoppingDistance);
            
//            SetAttackTarget(attackedBy.stats);

            Actor[] friends;
            int friendsCount = GetClosestFriends(out friends);
            
            
//            Debug.Log($"Closests friends: {friendsCount} ");

            for (int i = 0; i < friendsCount; i++)
            {
                if (!friends[i].movement.IsMoving())
                {
                    friends[i].movement.SetTarget(attackedBy.transform);
                }
            }
        }

        public void SetAttackTarget(IHealthable target)
        {
            attackTarget = target;
            state = BehaviorState.Chasing;
        }
        
        protected Actor GetNextEnemy()
        {
            Actor[] actors;
            int countActors = actor.vision.GetActorsInViewAngle(out actors);
            for (int i = 0; i < countActors; i++)
            {
                if (actor.IsEnemy(actors[i]))
                {
                    return actors[i];
                }
            }

            return null;
        }
        
        protected Actor GetClosestFriend()
        {
            foreach (Actor npcActor in actor.vision.actorsInViewRadius)
            {
                if (actor.IsFriend(npcActor))
                {
                    return npcActor;
                }
            }

            return null;
        }
        
        protected int GetClosestFriends(out Actor[] actors)
        {
            actors = new Actor[actor.vision.actorsInViewRadius.Count];
            int friendsCount = 0;
            foreach (Actor npcActor in actor.vision.actorsInViewRadius)
            {
                if (actor.IsFriend(npcActor))
                {
                    actors[friendsCount] = npcActor;
                    friendsCount++;
                }
            }

            return friendsCount;
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
            if (actorsController == null)
            {
                actorsController = GameManager.Instance().sceneController.GetActorsManager();
            }

            return hasAttackToken = actorsController.GetAttackToken();
        }

        public void ReturnAttackToken()
        {
            hasAttackToken = false;
            actorsController.ReturnAttackToken();
        }


        public void ShowPosition(Vector3 position)
        {
            GameManager.Instantiate(Resources.Load("System/Point") as GameObject, position, Quaternion.identity);
        }
    }
}