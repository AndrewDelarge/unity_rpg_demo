using UnityEngine;

namespace Gameplay.Player
{
    public class SpawnPoint : MonoBehaviour
    {
        public int id;


        private void Start()
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }
    }
}