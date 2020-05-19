using System;
using Actors.Base.Interface;
using GameSystems.Languages;
using UnityEngine;

namespace Gameplay.Scenario.Actions.AIActors
{
    public class ActionSpeak : ScenarioAction
    {
        public GameObject speakableGameObject;
        public Text text;
        
    
        private ISpeakable speakable;
        public override void Do()
        {
            base.Do();

            speakable = speakableGameObject.GetComponent<ISpeakable>();
            if (speakable == null)
            {
                onComplete?.Invoke();
                Debug.Log($"{name} action havent speakable entity");
                return;
            }

            startTime = Time.time;
            doing = true;
            speakable.Say(text, maxTime);
        }


        public override void CheckDoing()
        {
            return;
        }

        public override void Stop()
        {
            speakable.StopSay();
        }
    }
}