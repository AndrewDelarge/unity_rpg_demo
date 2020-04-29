using Actors.Player;
using GameSystems;
using UnityEngine;

namespace Gameplay
{
    public class Teleport : MonoBehaviour
    {

        public Transform point;
        
        public TeleportScene scene;

        private void OnTriggerEnter(Collider other)
        {
            PlayerActor actor = other.gameObject.GetComponent<PlayerActor>();

            if (actor == null)
            {
                return;
            }


            if (point != null)
            {
                TeleportToPoint();
                return;
            }

            if (! scene.Equals(null))
            {
                TeleportToScene();
            }
        }


        private void TeleportToPoint()
        {
            GameController.instance.playerManager.ChangePlayerPos(point.position);
        }
        
        private void TeleportToScene()
        {
            GameController.instance.StartScene(scene.scene, scene.spawnPointId);
        }
    }

    [System.Serializable]
    public struct TeleportScene
    {
        public int scene;
        public int spawnPointId;
    }
}