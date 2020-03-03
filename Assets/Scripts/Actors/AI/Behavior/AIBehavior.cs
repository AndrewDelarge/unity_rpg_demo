using Actors.Base;
using GameInput;
using UnityEngine;

namespace Actors.AI.Behavior
{
    public class AIBehavior : BaseBehavior
    {
        private float visionUpdateTime = .0f;
        public float visionTickRate = 0.1f;


        public override void Init(Actor baseActor, BaseInput baseInput)
        {
            base.Init(baseActor, baseInput);
            
        }

        public override void AIUpdate()
        {
            
        }

    }
}