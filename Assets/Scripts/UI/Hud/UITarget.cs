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
            
            inited = true;
            ResetPointer();
        }


        public void SetTarget(Transform target)
        {
            ResetPointer();
            
            if (target == null)
                return;
            
            worldTarget = target;
            
            Show();
            StartCoroutine(ChangeSlerpPointerPosSpeed());
        }

        public void ResetPointer()
        {
            doneGameObject.SetActive(false);
            visualPointer.transform.localPosition = Vector3.zero;
            Hide(true);
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
                return;
            
            visualPointer.transform.localScale = GetLocalScaleAnimation();
            
            visualPointer.transform.position = GetPointerPosition();
        }


        private Vector3 GetLocalScaleAnimation()
        {
            float x = 100 * Time.time * Time.deltaTime;
            float sinX = Mathf.Abs(Mathf.Sin(x)) * 0.5f;
            return new Vector3(1 + sinX, 1 + sinX);
        }

        private Vector3 GetPointerPosition()
        {
            Vector3 pos = cameraManager.WorldToScreenPoint(worldTarget.position);

            pos.y = LimitPosition(Screen.height, pos.y);
            pos.x = LimitPosition(Screen.width, pos.x);
            pos.z = 0;
            
            return Vector3.Slerp(visualPointer.transform.position, pos, slerpPosSpeed);
        }
        

        float LimitPosition(int max, float current)
        {
            if (current > max)
                return max;

            if (current < 0)
                return 0;

            return current;
        }
        
    }
}