using System.Collections;
using Gameplay.Scenario.Actions;
using UnityEngine;

namespace Gameplay.Scenario
{
    public class Scenario : MonoBehaviour
    {
        public ScenarioAction[] actions;
        private ScenarioAction currentAction;

        private bool started = false;
        
        public float startDelay = 0f;
        private void Start()
        {
            if (actions.Length == 0)
            {
                actions = GetComponentsInChildren<ScenarioAction>();
            }
         }

        public void StartScenario()
        {
            if (!started)
            {
                StartCoroutine(StartActions());
                started = true;
            }
        }


        IEnumerator StartActions()
        {
            yield return new WaitForSeconds(startDelay);

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

            started = false;
        }

        public void StopScenario()
        {
            started = false;
            StopCoroutine(StartActions());
        }
    }
}