using Actors.Player;
using GameSystems;
using UnityEngine;

namespace Gameplay
{
    public class TeleportToSpawnpoint : Teleport
    {
        public int spawnpointId;

        private void OnTriggerEnter(Collider other)
        {
            if (! other.CompareTag("Player"))
            {
                return;
            }

            TeleportToPoint(spawnpointId);
        }

    }
}