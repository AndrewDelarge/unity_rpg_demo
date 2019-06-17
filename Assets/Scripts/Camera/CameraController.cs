using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
    
        public float pitch = 2f;
        public float minZoom = 5f;
        public float maxZoom = 20f;
        public float zoomSpeed = 3f;
        public float yawSpeed = 100f;

        private float currentZoom = 10f;
        private float currentYaw = 0f;

        // Start is called before the first frame update
        void Update()
        {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = target.position - offset * currentZoom;
            transform.LookAt(target.position + Vector3.up * pitch);
        
            transform.RotateAround(target.position, Vector3.up, currentYaw);
        }
    }
}
