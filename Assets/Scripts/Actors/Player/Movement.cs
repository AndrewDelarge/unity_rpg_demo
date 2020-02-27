using Actors.Base;
using GameInput;
using UnityEngine;
using UnityEngine.AI;

namespace Actors.Player
{
    
    public class Movement : MonoBehaviour, IControlable
    {
        private BaseInput input;
        private Stats stats;
        private CharacterController characterController;
        private Camera cam;
        
        private bool stopped = false;

        public void Init(Stats actorStats, BaseInput input)
        {
            this.input = input;
            cam = Camera.main;
            characterController = GetComponent<CharacterController>();
            stats = actorStats;
        }

        private void FixedUpdate()
        {
            Vector3 direction = Vector3.zero;

            if (input.IsSomeDirection() && IsCanMove())
            {
                direction = new Vector3(input.horizontal, 0f, input.vertical);
                direction = cam.transform.TransformDirection(direction);
                direction.y = 0;
                direction = direction.normalized;

                float speedMultiply;
                
                if (Mathf.Abs(input.horizontal) >= .3f || Mathf.Abs(input.vertical) >= .3f)
                {
                    speedMultiply = 1f;
                }
                else
                {
                    speedMultiply = .4f;
                }
                direction *= speedMultiply;
                
                
                FaceDirection(direction);
            }
            
            Move(direction);
        }

        public bool IsCanMove()
        {
            return characterController.isGrounded && !stopped;
        }
        
        
        public float GetSpeed()
        {
            return stats.GetMovementSpeed();
        }

        public float GetCurrentMagnitude()
        {
            return characterController.velocity.magnitude;
        }

        public void Move(Vector3 direction)
        {
            direction.y -= 500f * Time.deltaTime;
            
            characterController.Move(direction * Time.deltaTime);
        }

        public void MoveTo(Vector3 point)
        {
            return;
        }

        public void Follow(Vector3 newTarget, float stoppingDistance = 1f)
        {
            FaceTarget(newTarget);
        }

        public void StopFollow()
        {
            return;
        }

        public void Stop()
        {
            return;
        }
        
        public void FaceTarget(Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, 15f * Time.deltaTime);
        }
        
        
        public void FaceDirection(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, 15f * Time.deltaTime);
        }

    }
}