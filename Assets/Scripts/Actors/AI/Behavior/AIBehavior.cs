using System.Collections;
using Actors.Base;
using Actors.Base.StatsStuff;
using UnityEngine;

namespace Actors.AI.Behavior
{
    public class AIBehavior : BaseBehavior
    {
        private float visionUpdateTime = .5f;

        protected Vector3 lastIdlePosition;

        public override void Init(Actor baseActor)
        {
            base.Init(baseActor);

            state = BehaviorState.Idle;
            lastIdlePosition = actor.transform.position;
            actor.stats.onGetDamage += OnGetDamage;
            actor.vision.visionUpdateTime = visionUpdateTime;
        }

        public override IEnumerator AIUpdate()
        {
            switch (state)
            {
                case BehaviorState.Idle:
                    Idle();
                    break;
                case BehaviorState.Patrol:
                    Patrol();
                    break;
                case BehaviorState.Attack:
                    Attack();
                    break;
            }

            yield return null;
        }

        void OnGetDamage(Damage damage)
        {
            if (damage.GetOwner() != null)
            {
                Defence(damage.GetOwner());
            }
        }
        
        public override void Idle()
        {
            Actor newTarget = GetNextEnemy();

            if (newTarget != null)
            {
                SetAttackTarget(newTarget.stats);
                
                Actor friend = GetClosestFriend();
                if (friend != null && !friend.movement.IsMoving())
                {
                    friend.movement.SetTarget(newTarget.transform);
                }
            }
        }

        protected override void Attack()
        {
            if (! TargetExists() || GetDistanceToTarget() > actor.vision.viewRadius)
            {
                ReturnToIdle();
                return;
            }
            
            actor.movement.Follow(attackTarget.GetTransform());
            actor.MeleeAttack(attackTarget);
        }

        public override void ReturnToIdle()
        {
            if (Vector3.Distance(lastIdlePosition, actor.transform.position) <= 1)
            {
                state = BehaviorState.Idle;
            }

            if (state == BehaviorState.ReturnToIdle) {
                return;
            }

            attackTarget = null;
            state = BehaviorState.ReturnToIdle;
            actor.movement.StopFollow();
            actor.movement.MoveTo(lastIdlePosition);
        }


        float GetDistanceToTarget()
        {
            return Vector3.Distance(attackTarget.GetTransform().position, actor.transform.position);
        }

        bool TargetExists()
        {
            return attackTarget != null && ! attackTarget.IsDead();
        }
        
        protected void FollowTarget(Transform transform)
        {
            
        }

        protected override void Patrol(Vector3[] points = null)
        {
            
        }
       
        
    }
}