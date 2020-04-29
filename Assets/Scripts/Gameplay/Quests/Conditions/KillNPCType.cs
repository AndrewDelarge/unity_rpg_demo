using Actors.AI;
using Scriptable;
using UnityEngine;

namespace Gameplay.Quests.Conditions
{
    [AddComponentMenu("Quest/Condition/KillNPCType")]
    public class KillNPCType : ConditionBase
    {
        public GameActor enemyType;
        public int howMuch = 1;
        
        private int currentKills = 0;
        private AIActor[] _npcActors;
        
        
        public override void Init()
        {
//            PlayerManager.instance.player.onTargetDied += Completing;



            UpdateEnemyList();
            
            SetCounterInTitle();
        }


        private void UpdateEnemyList()
        {
            _npcActors = FindObjectsOfType<AIActor>();

            
            for (int i = 0; i < _npcActors.Length; i++)
            {
                AIActor npcActor = _npcActors[i];
                _npcActors[i].stats.onDied += x => Completing(npcActor);
            }
        }
        
        void Completing(AIActor actor)
        {
            if (actor.actorScript != enemyType)
            {
                return;
            }
            currentKills++;
            SetCounterInTitle();
            
            if (onComplete != null)
            {
                onComplete.Invoke(this);
            }
            
            if (currentKills >= howMuch)
            {
                Completed();
            }
        }

        void SetCounterInTitle()
        {
            this.title = info.title + " " + currentKills + "/" + howMuch;
        }
        

    }

}