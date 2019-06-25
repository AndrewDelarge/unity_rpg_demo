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
            NPCActor actor = collider.gameObject.GetComponent<NPCActor>();

            if (actor != null)
            {
                actorsInRadius.Add(actor);
                onActorEnterRadius.Invoke(actor);
            }
        }
        

        private void OnTriggerExit(Collider collider)
        {
            NPCActor actor = collider.gameObject.GetComponent<NPCActor>();

            if (actor != null)
            {
                actorsInRadius.Remove(actor);
                onActorOutRadius.Invoke(actor);
            }
        }
    
    
    }
}
