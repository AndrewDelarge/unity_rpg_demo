using System;
using Actors.Base.Interface;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthUI : MonoBehaviour
    {
        public GameObject uiPrefab;
        public Transform target;

        private GameObject levelGameObject;
        private Transform ui;
        private Image healthImage;
        private Text healthText;
        private Transform cam;
        private IHealthable stats;
        
        private void Start()
        {
            stats = GetComponent<IHealthable>();
            
            
            
            if (Camera.main == null)
            {
                return;
            }
            
            cam = Camera.main.transform;
            
            foreach (Canvas canvas in FindObjectsOfType<Canvas>())
            {
                if (canvas.renderMode == RenderMode.WorldSpace)
                {
                    ui = Instantiate(uiPrefab, canvas.transform).transform;
                    healthImage = ui.GetChild(0).GetComponent<Image>();
                    healthText = ui.GetComponentInChildren<Text>();
                    levelGameObject = ui.GetChild(3).gameObject;

                    if (stats.IsHasLevel())
                    {
                        levelGameObject.SetActive(true);
                        Text levelText = levelGameObject.GetComponentInChildren<Text>();
                        levelText.text = stats.GetLevel().ToString();
                    }
                    
                }
            }

            if (target == null)
            {
                target = transform;
            }
            stats.OnHealthChange += SetHealth;
            
            ui.gameObject.SetActive(false);
        }
        
        void SetHealth(object obj, EventArgs args)
        {
            if (ui == null)
            {        
                return;
            }
            
            ui.gameObject.SetActive(true);

            healthImage.fillAmount = stats.GetHealth() / (float) stats.GetMaxHealth();
            healthText.text = stats.GetHealth().ToString();
            if (stats.GetHealth() <= 0)
            {
                Destroy(ui.gameObject);
            }
        }
        
        void FixedUpdate()
        {
            if (ui == null)
            {
                return;
            }

            float scale = transform.localScale.x;
            ui.position = new Vector3(target.position.x, target.position.y + (2.3f * scale), target.position.z);
            ui.forward = -cam.forward;
        }
    }
}
