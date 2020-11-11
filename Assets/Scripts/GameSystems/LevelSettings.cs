using UnityEngine;
using UnityEngine.Events;

namespace GameSystems
{
    public class LevelSettings : MonoBehaviour
    {
        [Header("Camera")]
        public Vector3 cameraOffset;
        public float cameraZoom;
        [Range(0.1f, 25)]
        public float cameraFollowSpeed = 3f;
        [Range(0.01f, 25)]
        public float cameraRotationSpeed = 4f;

        public int spawnPointId = 0;
        
        public UnityEvent onStart;
        
        public void Apply()
        {
            GameController gameController = GameController.instance;
            CameraController cameraController = gameController.GetCameraController();
            cameraController.offset = cameraOffset;
            cameraController.currentZoom = cameraZoom;
            cameraController.cameraFollowSpeed = cameraFollowSpeed;
            cameraController.cameraRotationSpeed = cameraRotationSpeed;
            
            cameraController.AlignCamera();
            onStart?.Invoke();
        }
        
        
        
        
        
        
        
    }
}