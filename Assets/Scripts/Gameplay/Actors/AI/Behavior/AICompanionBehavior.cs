using System.Collections;
using Gameplay.Actors.Base;

namespace Gameplay.Actors.AI.Behavior
{
    public class AiMeleeCompanionBehavior : AIMeleeBehavior 
    {
        public float visionTickRate = 0.1f;


        public override void AIUpdate()
        {
            switch (state)
            {
                case BehaviorState.Idle:
                    Idle();
                    break;

                case BehaviorState.Attack:
                    Attack();
                    break;
            }
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