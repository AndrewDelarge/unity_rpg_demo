using Actors.Base;
using Actors.Base.Interface;
using GameInput;

namespace Actors.AI
{
    public class AIActor : Actor
    {
        protected override void Init()
        {
            animator = GetComponent<CommonAnimator>();
            vision = GetComponent<Vision>();
            combat = GetComponent<Base.Combat>();
            input = GetComponent<AIInput>();
            stats = GetComponent<Stats>();
            movement = GetComponent<IControlable>();
            stats.Init();
            input.Init(this);
            combat.Init(stats, input);
            movement.Init(stats, input);
            animator.Init(combat, movement, stats);
        }
    }
}