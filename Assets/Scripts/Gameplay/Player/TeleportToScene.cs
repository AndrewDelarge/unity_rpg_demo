using GameSystems;
using UnityEngine;

namespace Gameplay
{
    public class TeleportToScene : Teleport
    {
        public TeleportScene scene;

        private void OnTriggerEnter(Collider other)
        {
            if (! other.CompareTag("Player"))
            {
                return;
            }

            TeleportToScene(scene);
        }

    }
}