using System.Collections.Generic;
using Scriptable;
using UnityEngine;

namespace NPC
{
    public class ActorLookRadius : MonoBehaviour
    {

    
        public delegate void OnActorEnterRadius(NPCActor actor);
        public delegate void OnActorOutRadius(NPCActor actor);

        public OnActorEnterRadius onActorEnterRadius;
        public OnActorOutRadius onActorOutRadius;


        public List<NPCActor> actorsInRadius = new List<NPCActor>();


        private void OnTriggerEnter(Collider collider)
        {
            AddToList(collider.gameObject);

        }
        

        private void OnTriggerExit(Collider collider)
        {
            RemoveFromList(collider.gameObject);
        }


        void RemoveOnDied(GameObject diedObject)
        {
            RemoveFromList(diedObject);
        }

        
        void AddToList(GameObject gameObject)
        {
            NPCActor actor = gameObject.GetComponent<NPCActor>();
               
            if (actor != null)
            {
                actorsInRadius.Add(actor);
                actor.characterStats.onDied += RemoveOnDied;

                if (onActorEnterRadius != null)
                {
                    onActorEnterRadius.Invoke(actor);
                }
            }
        }

        void RemoveFromList(GameObject gameObject)
        {
            NPCActor actor = gameObject.GetComponent<NPCActor>();
            
            if (actor != null)
            {
                actorsInRadius.Remove(actor);
                
                if (onActorOutRadius != null)
                {
                    onActorOutRadius.Invoke(actor);
                }
            }
        }
        
    }
}
