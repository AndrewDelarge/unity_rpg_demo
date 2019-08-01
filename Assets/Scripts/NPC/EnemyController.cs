using System;
using Player;
using Scriptable;
using UnityEngine;
using UnityEngine.AI;

namespace NPC
{
    public class EnemyController : NPCActorController
    {
        private NavMeshAgent agent;
    
        
        protected override void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();
            actor.characterStats.onDied += Ragdoll;
        }

    
        void Update()
        {
            // Debug
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Ragdoll(gameObject);
                return;
            }
            
            if (actor.target != null)
            {
                agent.SetDestination(actor.target.gameObject.transform.position);

                if (actor.target.InInteracableDistance(transform))
                {
                    FaceTarget();
                    actor.Attack(actor.target);
                }
            }
        }

        protected override void OnActorEnterRadius(NPCActor radiusActor)
        {
            if (! actor.IsEnemy(radiusActor))
            {
                Debug.Log(actor.actorScript + " frendly for : " + radiusActor.actorScript.title);
                return;
            }
            
            

            if (actor.InCombat())
            {
                Debug.Log(actor.actorScript + " already in combat ");

                return;
            }
            
            Debug.Log(actor.actorScript + " going attack for : " + radiusActor.actorScript.title);

            actor.combat.inCombat = true;
            actor.SetTarget(radiusActor);
            FaceTarget();
        }


        protected override void OnActorOutRadius(NPCActor radiusActor)
        {
            if (actor.target == radiusActor)
            {
                NPCActor newTarget = GetNextEnemy();

                actor.RemoveTarget();
               
                if (newTarget == null)
                {
                    actor.combat.inCombat = false;
                }
                else
                {
                    actor.SetTarget(newTarget);
                }
            }
        }


        NPCActor GetNextEnemy()
        {
            foreach (NPCActor npcActor in actorLookRadius.actorsInRadius)
            {
                if (actor.IsEnemy(npcActor))
                {
                    return npcActor;
                }
            }

            return null;
        }
        
        
        private void FaceTarget()
        {
            Vector3 direction = (actor.target.gameObject.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f);
        }


        public override void Defence(NPCActor attackedBy)
        {
            base.Defence(attackedBy);

            NPCActor closestFriend = GetClosestFriend();

            if (closestFriend == null || closestFriend.InCombat())
            {
                return;
            }

            NPCActorController actorController = closestFriend.GetComponent<NPCActorController>();

            actorController.Defence(attackedBy);
        }

        NPCActor GetClosestFriend()
        {
            foreach (NPCActor npcActor in actorLookRadius.actorsInRadius)
            {
                if (actor.IsFriend(npcActor))
                {
                    return npcActor;
                }
            }

            return null;
        }

        void Ragdoll(GameObject gameObject)
        {
            Debug.Log("Ragdolled " + name);
            
            Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
                rigidbody.AddForce(- gameObject.transform.forward * 10, ForceMode.Impulse);
            }

            agent.enabled = false;
            GetComponentInChildren<Animator>().enabled = false;
            this.enabled = false;
        }
    }
}
