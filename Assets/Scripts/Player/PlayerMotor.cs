using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMotor : MonoBehaviour
    {

        private Transform target;
        private NavMeshAgent agent;
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }


        private void Update()
        {
            if (target != null)
            {
                FaceTarget();
                agent.SetDestination(target.position);
            }
        }

        public void MoveTo(Vector3 point)
        {
            agent.isStopped = false;
            
            agent.SetDestination(point);
        }


        public void Follow(Interactable newTarget)
        {
            agent.isStopped = false;

            target = newTarget.interactableTransform;

            agent.stoppingDistance = newTarget.radius * .8f;
            agent.updateRotation = false;

        }

        public void StopFollow()
        {
            agent.stoppingDistance = 0f;
            agent.updateRotation = true;

            target = null;
        }

        public void Stop()
        {
            agent.isStopped = true;
        }
        
        
        private void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f);
        }
        
        
        private void FaceTarget(Transform target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10f);
        }
    }
}
