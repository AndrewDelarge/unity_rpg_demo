using System.Collections.Generic;
using Actors.AI;
using GameInput;
using UnityEngine;

namespace Managers
{
    public class AIActorsManager
    {

        protected List<AIActor> aliveActors;
        
        
        public AIActorsManager()
        {
            AIActor[] actors = GameObject.FindObjectsOfType<AIActor>();
            aliveActors = new List<AIActor>();
            
            for (int i = 0; i < actors.Length; i++)
            {
                actors[i].Init();
                
                aliveActors.Add(actors[i]);
            }
        }
    }
}