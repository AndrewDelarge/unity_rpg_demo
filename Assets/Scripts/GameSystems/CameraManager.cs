using System.Collections;
using CoreUtils;
using UnityEngine;

namespace GameSystems
{
    public enum GameCameras
    {
        Menu,
        InGame
    }
    public class CameraManager : SingletonDD<CameraManager>
    {
        [Header("Cameras")] 
        [SerializeField] 
        private Camera _menuCamera;
        [SerializeField] 
        private Camera _inGameCamera;
        
        public Transform target;
        public Vector3 offset;
        public float pitch = 2f;

        public float currentZoom = 10f;
        
        [Range(0.1f, 25)]
        public float cameraFollowSpeed = 1f;
        [Range(0.01f, 25)]
        public float cameraRotationSpeed = 1f;
        

        private Camera currentCamera;
        private bool positionFreezzed = false;

        private void Awake()
        {
            Init();
        }
        
        public void Init()
        {
            currentCamera = _menuCamera;
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
            Transform targetTransform = currentCamera.transform;
            Vector3 newPos = targetTransform.position;
            
            float oldY = targetTransform.position.y;
            
            newPos.y += power;
            
            targetTransform.position = newPos;
            
            yield return new WaitForSeconds(speed);
            
            newPos = targetTransform.position;
            newPos.y = oldY;
            
            targetTransform.position = newPos;
        }

        public void SwitchCamera(GameCameras cameraType)
        {
            DeactivateAllCameras();
            switch (cameraType)
            {
                case GameCameras.Menu:
                    SetCamera(_menuCamera, false, 0);
                    break;
                case GameCameras.InGame:
                    SetCamera(_inGameCamera, false, 0);
                    break;
            }
        }


        private void DeactivateAllCameras()
        {
            _menuCamera.gameObject.SetActive(false);
            _inGameCamera.gameObject.SetActive(false);
        }
        
        public void SetCamera(Camera camera, bool freezed = false, float delay = 1f)
        {
            StartCoroutine(SetCurrentCamera(camera, freezed, delay));
        }

        private IEnumerator SetCurrentCamera(Camera camera, bool freezed, float delay)
        {
            if (camera == null)
                yield break;
            
            yield return new WaitForSeconds(delay);
            
            positionFreezzed = freezed;
            
            if (currentCamera != null)
                currentCamera.gameObject.SetActive(false);
            
            camera.gameObject.SetActive(true);
            currentCamera = camera;
        }
        
        public void SetCamFreeze(bool freeze)
        {
            this.positionFreezzed = freeze;
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
