using Actors.Base;
using GameInput;
using UnityEngine;

namespace Actors.AI.Behavior
{
    public enum BehaviorType
    {
        Default, Warior
    }
    
    public abstract class BaseBehavior
    {
        protected Actor actor;
        protected BaseInput input;

        public virtual void Init(Actor baseActor, BaseInput baseInput)
        {
            actor = baseActor;
            input = baseInput;
        }

        public abstract void AIUpdate();
    }
}