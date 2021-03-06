using System.Collections;
using System.Collections.Generic;
using Gameplay.Actors.AI;
using GameSystems;
using Managers.Player;
using UnityEngine;

namespace Managers
{
    public class AIActorsManager
    {
        protected List<AIActor> aliveActors;

        private const int DESTROY_AFTER_DEATH_TIME = 3;

        private int attackTokens;
        private int currentAttackTokens;
        
        public AIActorsManager(int attackTokens = 4)
        {
            aliveActors = new List<AIActor>();
            this.attackTokens = attackTokens;
            currentAttackTokens = attackTokens;
            
            AIActor[] actors = GameObject.FindObjectsOfType<AIActor>();
            for (int i = 0; i < actors.Length; i++)
            {
                actors[i].Init();
                aliveActors.Add(actors[i]);
                actors[i].stats.onDied += diedObject => GameController.instance.StartCoroutine(OnActorDied(diedObject));
            }
        }

        public bool GetAttackToken()
        {
            
            if (currentAttackTokens == 0)
            {
                return false;
            }

            currentAttackTokens--;
            return true;
        }

        public void ReturnAttackToken()
        {
            if (currentAttackTokens == attackTokens)
            {
                return;
            }

            currentAttackTokens++;
        }
        

        IEnumerator OnActorDied(GameObject actorGO)
        {
            yield return new WaitForSeconds(DESTROY_AFTER_DEATH_TIME);

            if (actorGO == null)
            {
                yield break;
            }
            AIActor aiActor = actorGO.GetComponent<AIActor>();

            aliveActors.Remove(aiActor);
            GameObject.Destroy(actorGO);
        }
    }
}