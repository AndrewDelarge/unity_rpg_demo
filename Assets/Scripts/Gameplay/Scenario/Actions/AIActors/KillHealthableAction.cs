using Actors.Base.Interface;
using GameSystems.Input;
using UnityEngine;

namespace Gameplay.Scenario.Actions.AIActors
{
    public class KillHealthableAction : ScenarioAction
    {
        
        public AIInput input;

        public GameObject target;

        protected IHealthable healthable;
        public override void Do()
        {
            base.Do();
            
            healthable = target.GetComponent<IHealthable>();

            if (healthable == null)
            {
                Debug.LogWarning($"{name} action havent healthable target, skipping");
            }
            
            doing = true;
            
            input.behavior.SetAttackTarget(healthable);
        }

        public override void Stop()
        {
            input.behavior.ReturnToIdle();
            doing = false;
        }


        public override void CheckDoing()
        {
            if (healthable.IsDead())
            {
                doing = false;
            }
        }
    }
}