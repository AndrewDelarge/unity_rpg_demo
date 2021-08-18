using System.Collections;
using Gameplay.Actors.Base;
using Gameplay.Actors.Base.StatsStuff;
using UnityEngine;

namespace Gameplay.Actors.AI.Behavior
{
    public class AIRangeBehavior : BaseBehavior
    {
        private float visionUpdateTime = .5f;

        protected Vector3 lastIdlePosition;

        private int fearCooldown = 10;
        private int fearTime = 3;
        private float lastFearTime;
        private Vector3 lastTargetPos;
        
        private float EvadeRange => (actor.vision.viewRadius * 2);
        private float FearRange => (actor.vision.viewRadius * 0.25f);
        private float AttackRange => (actor.vision.viewRadius * 0.8f);
        private float StoppingRange => (actor.vision.viewRadius * 0.7f);

        public override void Init(Actor baseActor)
        {
            base.Init(baseActor);

            state = BehaviorState.Idle;
            lastIdlePosition = actor.transform.position;
            lastFearTime = Time.time - fearCooldown;
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
                case BehaviorState.Fear:
                    Fear();
                    break;
                case BehaviorState.Attack:
                    Attack();
                    break;
                case BehaviorState.ReturnToIdle:
                    ReturnToIdle();
                    break;
            }

            UpdateTarget();
        }

        void UpdateTarget()
        {
            if (TargetExists())
            {
                lastTargetPos = attackTarget.GetTransform().position;
            }
        }
        
        protected void Chasing()
        {
            if (! TargetExists() || GetDistanceToTarget() > EvadeRange)
            {
                ReturnToIdle();
                return;
            }

            if (GetDistanceToTarget() < FearRange && CanFear())
            {
                Fearing();
                return;
            }
            
            if (actor.movement.IsMoving())
            {
                return;
            }
            
            actor.movement.FaceTarget(attackTarget.GetTransform().position);

            
            if (GetDistanceToTarget() > AttackRange)
            {
                actor.movement.Follow(attackTarget.GetTransform(), StoppingRange);
                return;
            }
            
            if (GetAttackToken())
            {
                SetState(BehaviorState.Attack);
                return;
            }
            
            // If we in range attack but havent token 
            // Moving right or left

            Vector3 position = new Vector3(Random.Range(-1, 1), 0, 0) * 5;
            actor.movement.MoveTo(actor.transform.TransformPoint(position));
            
        }




        void Fearing()
        {
            state = BehaviorState.Fear;
            lastFearTime = Time.time;

            actor.movement.StopFollow();
            actor.movement.MoveTo(actor.transform.TransformPoint(Vector3.back * 10));
        }

        bool CanFear()
        {
            if (Time.time - lastFearTime >= fearCooldown && state != BehaviorState.Fear)
            {
                return true;
            }

            return false;
        }
        
        void Fear()
        {
            if (Time.time - lastFearTime >= fearTime)
            {
                actor.movement.StopFollow();
                state = BehaviorState.Chasing;
            }

            if (!actor.movement.IsMoving())
            {
                actor.movement.MoveTo(actor.transform.TransformPoint(Vector3.back * 10));
            }
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
                actor.movement.Stop();
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
            actor.movement.FaceTarget(attackTarget.GetTransform().position);

            if (actor.combat.GetRangeCooldown() < actor.combat.rangeAttackCooldown)
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
                    TryToRange();
                    break;
            }

            this.state = state;
        }
        
        
        void TryToRange()
        {
            Transform tTarget = attackTarget.GetTransform();
            Vector3 playerPos = tTarget.position;
            
            if (attackTarget.GetTransform().position != lastTargetPos)
            {
                playerPos = tTarget.TransformPoint(Vector3.forward / 2);
            }
            playerPos.y += 1;

            actor.combat.RangeAttack(playerPos);
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