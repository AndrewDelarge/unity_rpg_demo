using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace GameCamera
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public float pitch = 2f;

        private float currentZoom = 10f;
        private float lastRotation;
        private float lastPointerActive;


        private void Start()
        {
            if (target != null)
            {
                transform.position = target.position - offset * currentZoom;
                transform.LookAt(target.position + Vector3.up * pitch);
            }
            
        }
        
        void FixedUpdate()
        {
            if (Time.timeScale > 0f && target != null)
            {
                StartCoroutine(MoveCamera(transform.position, target.position));
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
                transform.position = Vector3.Lerp(offsetCamPos, offsetTargetPos, t * 2);
                transform.LookAt(Vector3.Lerp(targetPos + Vector3.up * pitch, target.position + Vector3.up * pitch, t * 2));
                yield return null;
            }
        }

        public IEnumerator Shake(float power = 1f, float speed = .1f)
        {
            float oldSize = Camera.main.orthographicSize;
            Camera.main.orthographicSize += power;
            float t = oldSize;
            
            while (t < Camera.main.orthographicSize)
            {
                if (power >= 0)
                {
                    Camera.main.orthographicSize -= Time.deltaTime / speed;
                }
                else
                {
                    Camera.main.orthographicSize += Time.deltaTime / speed;
                }
                
                yield return null;
            }

            Camera.main.orthographicSize = oldSize;
        }
        
    }
}
