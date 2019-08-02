using UnityEngine;

namespace UI
{
    public class Pointer : MonoBehaviour
    {
        public bool target = false;

        private Renderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            if (target)
            {
                Material material = _renderer.material;
                
                material.color = Color.red;
            }
        }
    }
}
