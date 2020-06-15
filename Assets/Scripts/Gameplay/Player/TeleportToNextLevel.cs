using Actors.Player;
using GameSystems;
using UnityEngine;

namespace Gameplay
{
    public class TeleportToNextLevel : Teleport
    {
        private void OnTriggerEnter(Collider other)
        {
            if (! other.CompareTag("Player"))
            {
                return;
            }

            TeleportToLevel();
        }

    }
}