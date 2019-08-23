using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthUI : MonoBehaviour
    {
        public GameObject uiPrefab;
        public Transform target;

        private Transform ui;
        private Image healthImage;
        private Transform cam;
        private CharacterStats stats;
        private void Awake()
        {
            stats = GetComponent<CharacterStats>();
            
            cam = Camera.main.transform;
            foreach (Canvas canvas in FindObjectsOfType<Canvas>())
            {
                if (canvas.renderMode == RenderMode.WorldSpace)
                {
                    ui = Instantiate(uiPrefab, canvas.transform).transform;
                    healthImage = ui.GetChild(0).GetComponent<Image>();
                }
            }

            stats.onHealthChange += SetHealth;
            ui.gameObject.SetActive(false);
        }


        void SetHealth(int value, int health)
        {
            ui.gameObject.SetActive(true);

            if (ui == null)
            {
                return;
            }
            healthImage.fillAmount = health / (float) stats.maxHealth;

            if (health <= 0)
            {
                Destroy(ui.gameObject);
            }
        }
        
        void Update()
        {
            if (ui == null)
            {
                return;
            }
            ui.position = target.position;
            ui.forward = -cam.forward;
        }
    }
}
