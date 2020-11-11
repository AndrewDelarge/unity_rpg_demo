using Gameplay.Actors.AI.Behavior;
using Gameplay.Actors.Base;

namespace GameSystems.Input
{
    public class AIInput : BaseInput
    {
        public BehaviorType behaviorType = BehaviorType.Default;
        
        public BaseBehavior behavior;

        public bool aiEnabled = true;

        private BehaviorState currentBstate;
        
        private void Awake()
        {
            enabled = false;
        }
        
        public override void Init(Actor actor)
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