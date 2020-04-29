using UnityEngine;

namespace Gameplay.Quests.Conditions
{
    [AddComponentMenu("Quest/Condition/StepInTriggerClass")]
    public class StepInTrigger : ConditionBase
    {
        public Trigger targetTrigger;

        public override void Init()
        {
            targetTrigger.OnEnter.AddListener(Completed);
        }
        

    }
}