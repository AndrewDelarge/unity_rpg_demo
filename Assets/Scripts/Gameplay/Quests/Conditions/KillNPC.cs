using Gameplay.Actors.AI;
using UnityEngine;

namespace Gameplay.Quests.Conditions
{
    [AddComponentMenu("Quest/Condition/KillNPC")]
    public class KillNPC : ConditionBase
    {
        public AIActor[] targets;

        private int currentKills = 0;
        
        public override void Init()
        {
            foreach (AIActor target in targets)
            {
                target.stats.onDied += Completing;
            }

            SetCounterInTitle();
        }

        void Completing(GameObject gameObject)
        {
            currentKills++;
            SetCounterInTitle();
            
            if (onComplete != null)
            {
                onComplete.Invoke(this);
            }
            if (currentKills >= targets.Length)
            {
                Completed();
                return;
            }
        }

        void SetCounterInTitle()
        {
            this.title = info.title + " " + currentKills + "/" + targets.Length;
        }
        
    }
}