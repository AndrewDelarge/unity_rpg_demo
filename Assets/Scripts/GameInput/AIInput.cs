using Actors.AI.Behavior;
using Actors.Base;

namespace GameInput
{
    public class AIInput : BaseInput
    {
        public BehaviorType behaviorType = BehaviorType.Default;
        
        public BaseBehavior behavior;

        public bool aiEnabled = true;

        private BehaviorState currentBstate;
        
        public override void Init(Actor actor)
        {
            // TODO rework case to inspector select
            switch (behaviorType)
            {
                case BehaviorType.Default:
                case BehaviorType.Warior:
                    behavior = new AIBehavior();
                    break;
            }

            behavior.Init(actor, this);
        }


        private void Update()
        {
            if (aiEnabled)
            {
                behavior.AIUpdate();
                currentBstate = behavior.GetState();
            }
        }
        
        
        
    }
}