using System.Collections;
using Actors.Base;

namespace Actors.AI.Behavior
{
    public class AICompanionBehavior : AIBehavior 
    {
        public float visionTickRate = 0.1f;


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
        
        public override void Idle()
        {
            Actor newTarget = GetNextEnemy();
            if (newTarget != null)
            {
                SetAttackTarget(newTarget.stats);
            }
        }
    }
}