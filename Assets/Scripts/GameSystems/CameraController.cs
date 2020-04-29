using System.Collections;
using UnityEngine;

namespace GameSystems
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public float pitch = 2f;

        private float currentZoom = 10f;
        private float lastRotation;
        private float lastPointerActive;

        private Camera mainCamera;
        private Camera defaultCamera;
        private bool positionFreezzed = false;
        
        
        
        private void Start()
        {
            mainCamera = Camera.main;
            defaultCamera = Camera.main;
            
            if (target != null)
            {
                mainCamera.transform.position = target.position - offset * currentZoom;
                mainCamera.transform.LookAt(target.position + Vector3.up * pitch);
            }
            
        }
        
        void FixedUpdate()
        {
            if (Time.timeScale > 0f && target != null)
            {
                StartCoroutine(MoveCamera(mainCamera.transform.position, target.position));
            }
            
        }

        private IEnumerator MoveCamera(Vector3 camPos, Vector3 targetPos)
        {
            Vector3 offsetTargetPos = targetPos - offset * currentZoom;
            Vector3 offsetCamPos = camPos - offset * currentZoom;
            
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / .1f;
                
                if (!positionFreezzed)
                {
                    mainCamera.transform.position = Vector3.Slerp(offsetCamPos, offsetTargetPos, t * 2);
                }
                mainCamera.transform.LookAt(Vector3.Slerp(targetPos + Vector3.up * pitch, target.position + Vector3.up * pitch, t * 2));
                yield return null;
            }
        }

        public IEnumerator Shake(float power = 1f, float speed = .1f)
        {
            float oldSize = mainCamera.orthographicSize;
            mainCamera.orthographicSize += power;
            float t = oldSize;
            
            while (t < mainCamera.orthographicSize)
            {
                if (power >= 0)
                {
                    mainCamera.orthographicSize -= Time.deltaTime / speed;
                }
                else
                {
                    mainCamera.orthographicSize += Time.deltaTime / speed;
                }
                
                yield return null;
            }

            mainCamera.orthographicSize = oldSize;
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
            mainCamera.enabled = false;
            camera.enabled = true;
            mainCamera = camera;
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
            return mainCamera;
        }
        
    }
}
