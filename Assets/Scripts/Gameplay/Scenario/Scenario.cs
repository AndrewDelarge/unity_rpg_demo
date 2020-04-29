using System.Collections;
using Gameplay.Scenario.Actions;
using UnityEngine;

namespace Gameplay.Scenario
{
    public class Scenario : MonoBehaviour
    {
        public ScenarioAction[] actions;

        private ScenarioAction currentAction;
        private void Start()
        {
            if (actions.Length == 0)
            {
                actions = GetComponentsInChildren<ScenarioAction>();
            }
            
            StartScenario();
        }

        public void StartScenario()
        {
            StartCoroutine(StartActions());
        }


        IEnumerator StartActions()
        {
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].Do();
                
                currentAction = actions[i];

                for (int sec = 0; sec < actions[i].maxTime; sec++)
                {
                    if (! currentAction.IsDoing())
                    {
                        break;
                    }
                    
                    yield return new WaitForSeconds(1);
                }
                
            }
        }

        public void StopScenario()
        {
            StopCoroutine(StartActions());
        }
    }
}