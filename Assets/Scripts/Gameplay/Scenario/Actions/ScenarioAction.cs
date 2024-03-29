using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Scenario.Actions
{
    public abstract class ScenarioAction : MonoBehaviour
    {
        public float maxTime = 10f;

        protected float startTime;
        protected bool doing = false;
        
        
        public delegate void OnComplete();
        public OnComplete onComplete;
        public delegate void OnStart();
        public OnStart onStart;


        public virtual void Do()
        {
            onStart?.Invoke();
        }

        public abstract void Stop();
        
        protected virtual void FixedUpdate()
        {
            if (! IsDoing())
            {
                return;
            }
            
            CheckDoing();

            if (Time.time - startTime >= maxTime)
            {
                doing = false;
            }
            
            if (! doing)
            {
                onComplete?.Invoke();
            }
        }


        public abstract void CheckDoing();
        
        public bool IsDoing()
        {
            return doing;
        }
        
        
    }
}