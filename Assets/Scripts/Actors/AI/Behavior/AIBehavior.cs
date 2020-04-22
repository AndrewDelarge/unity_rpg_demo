using System.Collections;
using Actors.Base;
using Actors.Combat;
using GameInput;
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
        
        protected override void Idle()
        {
            Actor newTarget = GetNextEnemy();
            if (newTarget != null)
            {
                SetAttackTarget(newTarget);
            }
        }

        protected override void Attack()
        {
            if (attackTarget == null 
                || attackTarget.IsDead()
                || Vector3.Distance(attackTarget.transform.position, actor.transform.position) > actor.vision.viewRadius)
            {
                attackTarget = null;
                state = BehaviorState.Idle;
                actor.movement.StopFollow();
                actor.movement.MoveTo(lastIdlePosition);
                return;
            }
            
            actor.movement.Follow(attackTarget.transform);
            actor.MeleeAttack(attackTarget);
        }


        protected void FollowTarget(Transform transform)
        {
            
        }

        protected override void Patrol(Vector3[] points = null)
        {
            
        }
       
        
    }
}