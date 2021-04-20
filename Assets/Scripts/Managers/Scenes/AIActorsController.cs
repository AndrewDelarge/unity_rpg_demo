using System.Collections;
using System.Collections.Generic;
using Gameplay.Actors.AI;
using GameSystems;
using UnityEngine;

namespace Managers.Scenes
{
    public class AIActorsController
    {
        protected List<AIActor> aliveActors;
        public List<AIActor> AliveActors => aliveActors;

        private const int DESTROY_AFTER_DEATH_TIME = 3;

        private int attackTokens;
        private int currentAttackTokens;
        
        public AIActorsController(int attackTokens = 4)
        {
            aliveActors = new List<AIActor>();
            this.attackTokens = attackTokens;
            currentAttackTokens = attackTokens;
            
            AIActor[] actors = GameObject.FindObjectsOfType<AIActor>();
            for (int i = 0; i < actors.Length; i++)
            {
                actors[i].Init();
                aliveActors.Add(actors[i]);
                actors[i].stats.onDied += diedObject => GameManager.Instance().StartCoroutine(OnActorDied(diedObject));
            }
            Debug.Log($" # -AI- # Actors on current level {actors.Length}, alive - {aliveActors.Count}");
            
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