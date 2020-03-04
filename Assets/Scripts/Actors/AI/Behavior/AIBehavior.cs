using Actors.Base;
using GameInput;
using UnityEngine;

namespace Actors.AI.Behavior
{
    public class AIBehavior : BaseBehavior
    {
        private float visionUpdateTime = .0f;
        public float visionTickRate = 0.1f;


        private Transform lastIdlePosition;
        public override void Init(Actor baseActor, BaseInput baseInput)
        {
            base.Init(baseActor, baseInput);

            state = BehaviorState.Idle;
            lastIdlePosition = actor.transform;
        }

        public override void AIUpdate()
        {
            switch (state)
            {
                case BehaviorState.Idle:
                    Idle();
                    lastIdlePosition = actor.transform;
                    break;
                case BehaviorState.Patrol:
                    Patrol();
                    break;
                case BehaviorState.Attack:
                    Attack();
                    break;
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
            if (attackTarget == null || attackTarget.IsDead())
            {
                Debug.Log("FIIAFSAs");
                attackTarget = null;
                state = BehaviorState.Idle;
                input.SetTarget(lastIdlePosition);
                return;
            }
            
            
            input.SetTarget(attackTarget.transform);

            if (actor.combat.InMeleeRange(attackTarget.transform))
            {
                actor.combat.SetTarget(attackTarget);
                input.Attack();
            }
        }

        protected override void Patrol(Vector3[] points = null)
        {
            
        }

        public override void Defence(Actor attackedBy)
        {
            base.Defence(attackedBy);

            Actor closestFriend = GetClosestFriend();

            if (closestFriend == null || closestFriend.InCombat())
            {
                return;
            }
            
            BaseBehavior aiBehavior = closestFriend.GetComponent<BaseBehavior>();

            if (aiBehavior != null)
            {
                aiBehavior.Defence(attackedBy);
            }
        }
        
        
    }
}