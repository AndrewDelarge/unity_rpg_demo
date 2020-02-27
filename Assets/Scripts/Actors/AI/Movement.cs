using Actors.Base;
using UnityEngine;
using UnityEngine.AI;

namespace Actors.AI
{
    
    [RequireComponent(typeof(NavMeshAgent))]
    public class Movement : MonoBehaviour, IControlable
    {
        private Vector3 target;
        private NavMeshAgent agent;

        private Stats stats;

        public void Init(Stats actorStats)
        {
            agent = GetComponent<NavMeshAgent>();
            stats = actorStats;
        }

        private void Update()
        {
            if (target != Vector3.zero)
            {
                FaceTarget();

                agent.speed = stats.GetMovementSpeed();
                agent.SetDestination(target);
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

            target = newTarget;

            agent.stoppingDistance = stoppingDistance;
            agent.updateRotation = false;
        }

        public void StopFollow()
        {
            agent.stoppingDistance = 0f;
            agent.updateRotation = true;

            target = Vector3.zero;
        }

        public void Stop()
        {
            agent.isStopped = true;
        }
        
        public void FaceTarget()
        {
            Vector3 direction = (target - transform.position).normalized;
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