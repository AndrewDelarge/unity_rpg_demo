using System;
using Actors.Player;
using UnityEngine;

namespace GameCamera
{
    
    [RequireComponent(typeof(Collider))]
    public class FixedCameraZone : MonoBehaviour
    {
        public Camera curCamera;
        private Collider curCollider;

        private GameObject target;
        private void Start()
        {

            curCollider = GetComponent<Collider>();

            if (curCamera == null)
            {
                throw new Exception("Camera not found in " + name);
            }
            
            
            curCamera.enabled = false;
        }


        private void OnTriggerEnter(Collider collider)
        {
            if (! collider.CompareTag("Player"))
            {
                return;
            }

            target = collider.gameObject;
            
            GameController.instance.mainCamera.SetCamera(curCamera);
            GameController.instance.mainCamera.SetCamFreeze(true);
        }

        private void FixedUpdate()
        {
            if (target != null)
            {
                curCamera.transform.LookAt(target.transform);
            }
        }
        
        private void OnTriggerExit(Collider collider)
        {
            if (! collider.CompareTag("Player"))
            {
                return;
            }

            target = null;
            GameController.instance.mainCamera.ResetCamera();
            GameController.instance.mainCamera.SetCamFreeze(false);
        }
    }
    
}