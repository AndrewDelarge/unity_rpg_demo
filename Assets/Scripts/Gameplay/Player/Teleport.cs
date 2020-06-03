using Actors.Player;
using GameSystems;
using UnityEngine;

namespace Gameplay
{
    public abstract class Teleport : MonoBehaviour
    {
        protected void TeleportToPoint(int spawnpointId)
        {
            GameController.instance.playerManager.TeleportToPoint(spawnpointId);
        }
        
        protected void TeleportToScene(TeleportScene scene)
        {
            GameController.instance.StartScene(scene.scene);
        }
    }

    [System.Serializable]
    public struct TeleportScene
    {
        public int scene;
        public int spawnPointId;
    }
}