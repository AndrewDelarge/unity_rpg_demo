using Actors.Base;
using Actors.Base.Interface;
using GameInput;
using UnityEngine;
using UnityEngine.AI;

namespace Actors.AI
{
    
    [RequireComponent(typeof(NavMeshAgent))]
    public class AIMovement : MonoBehaviour, IControlable
    {
        private NavMeshAgent agent;

        private Stats stats;
        private BaseInput input;

        public void Init(Stats actorStats, BaseInput baseInput)
        {
            agent = GetComponent<NavMeshAgent>();
            stats = actorStats;
            input = baseInput;
        }

        private void Update()
        {
            Transform target = input.GetTarget();
            if (target != null)
            {
                FaceTarget();

                agent.speed = stats.GetMovementSpeed();
                agent.SetDestination(target.transform.position);
            }

            if (input.IsSomeDirection())
            {
//                agent.SetDestination(target.transform.position);
            }
        }

        public float GetSpeed()
        {
            return agent.speed;
        }

        public float GetCurrentMagnitude()
        {
            return agent.velocity.magnitude;
        }

        public void Move(Vector3 direction)
        {
            agent.isStopped = false;
            
            agent.SetDestination(gameObject.transform.position + direction);
        }

        public void MoveTo(Vector3 point)
        {
            agent.isStopped = false;
            
            agent.SetDestination(point);
        }

        public void Follow(Vector3 newTarget, float stoppingDistance = 1f)
        {
            agent.isStopped = false;

//            target = newTarget;

            agent.stoppingDistance = stoppingDistance;
            agent.updateRotation = false;
        }

        public void StopFollow()
        {
            agent.stoppingDistance = 0f;
            agent.updateRotation = true;
            agent.isStopped = true;

//            target = Vector3.zero;
        }

        public void Stop()
        {
            agent.isStopped = true;
        }
        
        public void FaceTarget()
        {
            Vector3 direction = (input.GetTarget().transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f);
        }
        
        
        public void FaceTarget(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f);
        }


    }
}