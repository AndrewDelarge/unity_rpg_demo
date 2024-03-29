using System.Collections;
using Gameplay.Actors.Base;
using Gameplay.Actors.Base.StatsStuff;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Actors.AI.Behavior
{
    public class AIMeleeBehavior : BaseBehavior
    {
        protected Vector3 lastIdlePosition;
        
        private float visionUpdateTime = .5f;
        
        
        public override void Init(Actor baseActor)
        {
            base.Init(baseActor);

            state = BehaviorState.Idle;
            lastIdlePosition = actor.transform.position;
            actor.stats.onGetDamage += OnGetDamage;
            actor.vision.visionUpdateTime = visionUpdateTime;
        }

        public override void AIUpdate()
        {
            switch (state)
            {
                case BehaviorState.Idle:
                    Idle();
                    break;
                case BehaviorState.Chasing:
                    Chasing();
                    break;
                case BehaviorState.Attack:
                    Attack();
                    break;
                case BehaviorState.ReturnToIdle:
                    ReturnToIdle();
                    break;
            }
        }

        protected override void OnActorDied(GameObject died)
        {
            actor.stats.onGetDamage -= OnGetDamage;
            base.OnActorDied(died);
        }

        void OnGetDamage(Damage damage)
        {
            if (damage.GetOwner() == null)
            {
                return;
            }
            
            Defence(damage.GetOwner());
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

        protected void Chasing()
        {
            if (! TargetExists() || GetDistanceToTarget() > actor.vision.viewRadius * 2)
            {
                ReturnToIdle();
                return;
            }

            
            
            if (! actor.combat.InMeleeRange(attackTarget.GetTransform()))
            {
                if (actor.movement.IsMoving())
                {
                    return;
                }
                actor.movement.Follow(attackTarget.GetTransform());
                return;
            }

            if (GetAttackToken())
            {
                SetState(BehaviorState.Attack);
                return;
            }
            
            Vector3 position = new Vector3(0, 0, Random.Range(-1, 0)) * 3;
            actor.movement.MoveTo(actor.transform.TransformPoint(position));
        }

        
        protected override void Attack()
        {
            if (actor.combat.IsMeleeAttacking())
            {
                return;
            }
            ReturnAttackToken();
            SetState(BehaviorState.Chasing);
        }

        protected override void SetState(BehaviorState state)
        {
            switch (state)
            {
                case BehaviorState.Attack:
                    actor.movement.StopFollow();
                    actor.MeleeAttack(attackTarget);
                    break;
            }

            this.state = state;
        }
        
        public override void ReturnToIdle()
        {
            if (Vector3.Distance(lastIdlePosition, actor.transform.position) <= 1)
            {
                state = BehaviorState.Idle;
                return;
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
    }
}