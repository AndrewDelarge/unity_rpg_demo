using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Managers
{
    public class TriggersManager
    {
        protected List<Trigger> triggers;
        
        public TriggersManager(Trigger[] sceneTriggers)
        {
            triggers = new List<Trigger>();
            for (int i = 0; i < sceneTriggers.Length; i++)
            {
                sceneTriggers[i].gameObject.SetActive(false);
                triggers.Add(sceneTriggers[i]);
            }
        }


        public void Init()
        {
            for (int i = 0; i < triggers.Count; i++)
            {
                triggers[i].Init();
            }
        }
    }
}