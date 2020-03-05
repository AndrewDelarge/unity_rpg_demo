using Actors.Base;
using Actors.Combat;
using GameInput;
using UnityEngine;

namespace Actors.AI.Behavior
{
    public class AIBehavior : BaseBehavior
    {
        private float visionUpdateTime = .0f;
        public float visionTickRate = 0.1f;


        private Vector3 lastIdlePosition;

        private bool moved = false;
        public override void Init(Actor baseActor)
        {
            base.Init(baseActor);

            state = BehaviorState.Idle;
            lastIdlePosition = actor.transform.position;
            actor.stats.onGetDamage += OnGetDamage;
        }

        public override void AIUpdate()
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
            if (attackTarget == null || attackTarget.IsDead())
            {
                attackTarget = null;
                state = BehaviorState.Idle;
                actor.movement.StopFollow();
                actor.movement.MoveTo(lastIdlePosition);
                return;
            }
            
            
            actor.movement.Follow(attackTarget.transform);
            actor.SetActorTarget(attackTarget);

            if (actor.combat.InMeleeRange(attackTarget.transform))
            {
                input.Attack();
            }
        }

        protected override void Patrol(Vector3[] points = null)
        {
            
        }
       
        
    }
}