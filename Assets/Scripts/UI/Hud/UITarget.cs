using System.Collections;
using GameSystems;
using UI.Base;
using UnityEngine;

namespace UI.Hud
{
    public class UITarget : HideableUi
    {
        public GameObject visualPointer;
        public GameObject doneGameObject;
        
        
        private Transform worldTarget;
        private float slerpPosSpeed = 1f;
        private CameraManager cameraManager;
        
        
        public override void Init()
        {
            cameraManager = CameraManager.Instance();
            curElement = visualPointer;
            showSpeedTime = 2f;
            inited = true;
            Hide();
            doneGameObject.SetActive(false);
        }


        public void SetTarget(Transform target)
        {
            if (target == null)
            {
                Hide();
                return;
            }
            Show();
            worldTarget = target;
            doneGameObject.SetActive(false);
            visualPointer.transform.localPosition = Vector3.zero;
            StartCoroutine(ChangeSlerpPointerPosSpeed());
        }


        IEnumerator ChangeSlerpPointerPosSpeed()
        {
            slerpPosSpeed = 0f;
            yield return new WaitForSeconds(0.5f);
            slerpPosSpeed = 0.1f;
            yield return new WaitForSeconds(1f);
            slerpPosSpeed = 1f;
        }
        
        public void Done()
        {
            doneGameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            if (worldTarget == null)
            {
                return;
            }
            
            float x = 100 * Time.time * Time.deltaTime;
            float sinX = Mathf.Abs(Mathf.Sin(x)) * 0.5f;
            visualPointer.transform.localScale = new Vector3(1 + sinX, 1 + sinX);
            
            Camera camera = cameraManager.GetCamera();
            Vector3 pos = camera.WorldToScreenPoint(СalcWorldPosition(worldTarget.position, camera));

            pos.y = GetPosition(Screen.height, pos.y);
            pos.x = GetPosition(Screen.width, pos.x);
            pos.z = 0;
            
            pos = Vector3.Slerp(visualPointer.transform.position, pos, slerpPosSpeed);
            visualPointer.transform.position = pos;
        }
        
        
        private Vector3 СalcWorldPosition(Vector3 position, Camera camera) {  
            Vector3 camNormal = camera.transform.forward;
            Vector3 vectorFromCam = position - camera.transform.position;
            
            // TODO normalized
            float camNormDot = Vector3.Dot (camNormal, vectorFromCam.normalized);
            
            if (camNormDot <= 0f) {
                float camDot = Vector3.Dot (camNormal, vectorFromCam);
                Vector3 proj = (camNormal * camDot * 1.01f);
                position = camera.transform.position + (vectorFromCam - proj);
            }
 
            return position;
        }


        float GetPosition(int max, float current)
        {
            if (current > max)
            {
                return max;
            }

            if (current < 0)
            {
                return 0;
            }

            return current;
        }
        
    }
}