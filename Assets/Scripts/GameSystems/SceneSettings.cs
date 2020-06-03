using UnityEngine;
using UnityEngine.Events;

namespace GameSystems
{
    public class SceneSettings : MonoBehaviour
    {
        [Header("Camera")]
        public Vector3 cameraOffset;
        public float cameraZoom;
        [Range(0.1f, 25)]
        public float cameraFollowSpeed = 3f;
        [Range(0.01f, 25)]
        public float cameraRotationSpeed = 4f;

        public bool spawnPlayer = true;
        public int spawnPointId = 0;
        
        public UnityEvent onStart;
        
        public void Apply(GameController controller)
        {
            controller.cameraController.offset = cameraOffset;
            controller.cameraController.currentZoom = cameraZoom;
            controller.cameraController.cameraFollowSpeed = cameraFollowSpeed;
            controller.cameraController.cameraRotationSpeed = cameraRotationSpeed;
            
            
            onStart?.Invoke();
        }
        
        
        
        
        
        
        
    }
}