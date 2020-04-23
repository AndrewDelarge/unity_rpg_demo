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
        public float speedMultiplier = 1f;
        private NavMeshAgent agent;
        public Transform target { get; private set; }
        
        private Stats stats;
        private BaseInput input;

        private void Awake()
        {
            enabled = false;
        }
        public void Init(Stats actorStats, BaseInput baseInput)
        {
            agent = GetComponent<NavMeshAgent>();
            stats = actorStats;
            input = baseInput;
            enabled = true;
        }

        private void FixedUpdate()
        {
            agent.speed = stats.GetMovementSpeed() * speedMultiplier;

            if (target != null)
            {
//                FaceTarget();

                agent.SetDestination(target.transform.position);
            }

            if (input.IsSomeDirection())
            {
                Vector3 direction = new Vector3(input.horizontal, 0f, input.vertical);
                direction = Camera.main.transform.TransformDirection(direction);
                direction.y = 0;
                direction = direction.normalized;

                Move(direction);
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

        public void Follow(Transform newTarget, float stoppingDistance = 1.5f)
        {
            agent.isStopped = false;
            target = newTarget;
            agent.stoppingDistance = stoppingDistance;
            agent.updateRotation = true;
        }

        public void StopFollow()
        {
            agent.stoppingDistance = 0f;
            agent.updateRotation = true;
            agent.isStopped = true;
            target = null;
        }

        public void Stop()
        {
            agent.isStopped = true;
        }
        
        public void FaceTarget()
        {
            FaceTarget(target.position);
        }
        
        
        public void FaceTarget(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f);
            }
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        public void Disable()
        {
            enabled = false;
            agent.enabled = false;
        }

        public void Enable()
        {
            agent.enabled = true;
        }

        public void Jump()
        {
            return;
        }
        
        public void SetSpeed(float multiplier)
        {
            speedMultiplier = multiplier;
        }
    }
}