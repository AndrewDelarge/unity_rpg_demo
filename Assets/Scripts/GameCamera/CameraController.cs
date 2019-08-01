using Player;
using UnityEngine;
using UnityEngine.AI;

namespace GameCamera
{
    public class CameraController : MonoBehaviour
    {
        private const float ROTATION_RETURN_TIME = 2f; 
        
        
        public Transform target;
        public Vector3 offset;
    
        public float pitch = 2f;
        public float minZoom = 5f;
        public float maxZoom = 20f;
        public float zoomSpeed = 3f;
        public float yawSpeed = 100f;

        private float currentZoom = 10f;
        private float currentYaw = 0f;
        private NavMeshAgent targetRigidbody;
        private float lastRotation;

        
        private void Start()
        {
            targetRigidbody = target.GetComponent<NavMeshAgent>();
        }

        // Start is called before the first frame update
        void Update()
        {
            currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            float horizontal = Input.GetAxis("Mouse X");


            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                
                if (touch.phase == TouchPhase.Moved)
                {
                    currentYaw -= horizontal * yawSpeed * Time.deltaTime;
                    lastRotation = Time.time;
                }
            }
           
        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = target.position - offset * currentZoom;
            transform.LookAt(target.position + Vector3.up * pitch);

            
            
            
//            Debug.Log(targetRigidbody.hasPath);
//            if (targetRigidbody.hasPath && Input.GetAxis("Mouse X") == 0f && isCanReturnRotation())
//            {
//                currentYaw = target.eulerAngles.y - 180;
//            }
//            
//            
//            transform.RotateAround(
//                target.position,
//                Vector3.up,
//                currentYaw
//            );
            
        }

        bool isCanReturnRotation()
        {
            return (Time.time - lastRotation) > ROTATION_RETURN_TIME;
        }
        
    }
}
