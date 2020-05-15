using System.Collections;
using UnityEngine;

namespace GameSystems
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public float pitch = 2f;

        public float currentZoom = 10f;
        
        [Range(0.1f, 25)]
        public float cameraFollowSpeed = 1f;
        [Range(0.01f, 25)]
        public float cameraRotationSpeed = 1f;
        
        
        private float lastRotation;
        private float lastPointerActive;

        private Camera currentCamera;
        private Camera defaultCamera;
        private bool positionFreezzed = false;

        private void Start()
        {
            currentCamera = GetComponent<Camera>();
            defaultCamera = Camera.main;

            if (target != null)
            {
                currentCamera.transform.position = target.position - offset * currentZoom;
                currentCamera.transform.LookAt(target.position + Vector3.up * pitch);
            }
            
        }
        
        void FixedUpdate()
        {
            if (Time.timeScale > 0f && target != null)
            {
                Vector3 pos = Vector3.Slerp(currentCamera.transform.position, target.position - offset * currentZoom, cameraFollowSpeed * Time.deltaTime);
                
                
                if (!positionFreezzed)
                {
                    currentCamera.transform.position = pos;
                }



                
                Quaternion targetRotation = Quaternion.LookRotation(target.position - currentCamera.transform.position + Vector3.up * pitch, target.up);
                currentCamera.transform.rotation = Quaternion.Slerp(currentCamera.transform.rotation, targetRotation, cameraRotationSpeed * Time.deltaTime);
            }
            
        }

        public IEnumerator Shake(float power = 1f, float speed = .1f)
        {
            float oldSize = currentCamera.orthographicSize;
            currentCamera.orthographicSize += power;
            float t = oldSize;
            
            while (t < currentCamera.orthographicSize)
            {
                if (power >= 0)
                {
                    currentCamera.orthographicSize -= Time.deltaTime / speed;
                }
                else
                {
                    currentCamera.orthographicSize += Time.deltaTime / speed;
                }
                
                yield return null;
            }

            currentCamera.orthographicSize = oldSize;
        }

        public void SetCamera(Camera camera, bool freezed = false)
        {
            StartCoroutine(SetCurCamera(camera, freezed));
        }

        IEnumerator SetCurCamera(Camera camera, bool freezed)
        {
            yield return new WaitForSeconds(1f);
            if (camera == null)
            {
                yield break;
            }
            positionFreezzed = freezed;
            currentCamera.enabled = false;
            camera.enabled = true;
            currentCamera = camera;
        }
        
        public void SetCamFreeze(bool freeze)
        {
            this.positionFreezzed = freeze;
        }
        
        public void ResetCamera()
        {
            StartCoroutine(SetCurCamera(defaultCamera, false));
        }


        public Camera GetCamera()
        {
            return currentCamera;
        }
        
    }
}
