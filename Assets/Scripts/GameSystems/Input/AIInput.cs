using Gameplay.Actors.AI.Behavior;
using Gameplay.Actors.Base;
using UnityEngine;

namespace GameSystems.Input
{
    public class AIInput : BaseInput
    {
        [SerializeField]
        private Actor actor;
        
        public BehaviorType behaviorType = BehaviorType.Default;
        
        public BaseBehavior behavior;

        public bool aiEnabled = true;

        private BehaviorState currentBstate;

        public override void Init()
        {
            // TODO rework case to inspector select
            switch (behaviorType)
            {
                case BehaviorType.Default:
                case BehaviorType.Warior:
                    behavior = new AIMeleeBehavior();
                    break;
                case BehaviorType.Range:
                    behavior = new AIRangeBehavior();
                    break;
            }

            behavior.Init(actor);
            enabled = true;
        }


        private void FixedUpdate()
        {
            if (aiEnabled)
            {
                StartCoroutine(behavior.AIUpdate());
                currentBstate = behavior.GetState();
            }
        }
        
    }
}