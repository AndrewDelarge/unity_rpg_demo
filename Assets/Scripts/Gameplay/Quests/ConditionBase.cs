using Scriptable.Quests;
using UnityEngine;

namespace Gameplay.Quests
{
    public abstract class ConditionBase : MonoBehaviour
    {
        [SerializeField]
        protected ConditionInfo info;
        public delegate void OnComplete (ConditionBase thisCondition);
        public OnComplete onComplete;
        
        private bool isCompleted;
        
        protected string title;
        
        private void Start()
        {
            title = info.title;
        }

        public abstract void Init();

        public void Completed()
        {
            if (isCompleted)
            {
                return;
            }
            this.isCompleted = true;
            if (onComplete != null)
            {
                onComplete.Invoke(this);
            }

            onComplete = null;
        }

        public string GetTitle()
        {
            return this.title;
        }
        
        public bool IsCompleted()
        {
            return isCompleted;
        }
    }
}