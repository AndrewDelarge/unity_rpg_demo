using System;
using Scriptable;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class EnemyController : NPCActorController
    {

        public float lockRadius = 10f;
        private NPCActor target;

        private NavMeshAgent agent;
    
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            
            actor = GetComponent<NPCActor>();
            
            
            if (actor == null)
            {
                throw new Exception("NPCActor Must Be set in object " + transform.name);
            }

            actorLookRadius = lookRadiusObject.GetComponent<ActorLookRadius>();

            if (actorLookRadius == null)
            {
                throw new Exception("ActorLookRadius Must Be set in Look Radius Object ");
            }

            actorLookRadius.onActorEnterRadius += OnActorEnterRadius;
            actorLookRadius.onActorOutRadius += OnActorOutRadius;
        }

    
        // Update is called once per frame
        void Update()
        {
            if (target != null)
            {
                agent.SetDestination(target.gameObject.transform.position);
            }
        }

        protected override void OnActorEnterRadius(NPCActor radiusActor)
        {
            if (target == radiusActor)
            {
                Debug.Log(actor.actorScript + " allready have target : " + radiusActor.actorScript.title);
                return; 
            }

            if (actor.actorScript.fraction.FractionInEnemies(radiusActor.actorScript.fraction))
            {
                target = radiusActor;
                Debug.Log(actor.actorScript + " going attack : " + radiusActor.actorScript.title);
                return;
            }
            
            Debug.Log(actor.actorScript + " frendly for : " + radiusActor.actorScript.title);

        }

        protected override void OnActorOutRadius(NPCActor radiusActor)
        {
            if (target == radiusActor)
            {
                target = null;
                Debug.Log(actor.actorScript + " remove from target : " + radiusActor.actorScript.title);
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lockRadius);
        }
    }
}
