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
        

        private Camera currentCamera;
        private Camera defaultCamera;
        private bool positionFreezzed = false;

        public void Init()
        {
            currentCamera = GetComponent<Camera>();
            defaultCamera = Camera.main;

            AlignCamera();
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
            {
                Transform targetT = transform;
                float oldY = targetT.position.y;
                Vector3 newPos = targetT.position;
                newPos.y += power;
                targetT.position = newPos;
                yield return new WaitForSeconds(speed);
                newPos = targetT.position;
                newPos.y = oldY;
                targetT.position = newPos;
            }
            
            
            
            
            yield break;
            // Pitching
            {
                float oldPitch = pitch;
                pitch += power;

//            AlignCamera();
                yield return new WaitForSeconds(speed);
            
                pitch = oldPitch;
                Quaternion targetRotation = Quaternion.LookRotation(target.position - currentCamera.transform.position + Vector3.up * oldPitch, target.up);
                currentCamera.transform.rotation = Quaternion.Slerp(currentCamera.transform.rotation, targetRotation, 0.5f);
            }
            
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


        public void AlignCamera()
        {
            if (target != null)
            {
                currentCamera.transform.position = target.position - offset * currentZoom;
                currentCamera.transform.LookAt(target.position + Vector3.up * pitch);
            }
        }

        public Camera GetCamera()
        {
            return currentCamera;
        }
        
    }
}
