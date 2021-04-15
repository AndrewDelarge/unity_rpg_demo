using GameSystems;
using Managers;
using UnityEngine;

namespace Gameplay
{
    public abstract class Teleport : MonoBehaviour
    {
        protected void TeleportToPoint(int spawnpointId)
        {
            PlayerManager.Instance().TeleportToPoint(spawnpointId);
        }
        
        protected void TeleportToScene(TeleportScene scene)
        {
            GameManager.Instance().StartScene(scene.scene);
        }
        
        
        protected void TeleportToLevel()
        {
            GameManager.Instance().sceneController.NextLevel();
        }
    }

    [System.Serializable]
    public struct TeleportScene
    {
        public int scene;
        public int spawnPointId;
    }
}