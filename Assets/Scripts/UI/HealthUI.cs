using System;
using Gameplay.Actors.Base.Interface;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    
    // TODO Split this into 2 comp
    // First WorldspaceUI component
    // Second - Actor Component 
    public class HealthUI : WorldspaceUi
    {
        [Header("Health")]
        [SerializeField] private Image healthImage;
        [SerializeField] private Text healthText;
        
        [Header("Level")]
        [SerializeField] private GameObject levelGameObject;
        [SerializeField] private Text levelText;
        
        
        private IHealthable stats;
        private bool showed = false;
        
        public override void Init()
        {
            type = WorldUiType.Healthbars;
            stats = GetComponent<IHealthable>();
            
            base.Init();
            
            healthImage = curElement.transform.GetChild(0).GetComponent<Image>();
            healthText = curElement.GetComponentInChildren<Text>();
            levelGameObject = curElement.transform.GetChild(3).gameObject;

            if (stats.IsHasLevel())
            {
                levelGameObject.SetActive(true);
                Text levelText = levelGameObject.GetComponentInChildren<Text>();
                levelText.text = stats.GetLevel().ToString();
            }

            stats.OnHealthChange += SetHealth;
        }
        
        private void SetHealth(object obj, EventArgs args)
        {
            if (!showed)
            {
                Show();
                showed = true;
            }
            
            if (curElement == null)
                return;
            
            curElement.SetActive(true);
            healthImage.fillAmount = stats.GetHealth() / (float) stats.GetMaxHealth();
            healthText.text = stats.GetHealth().ToString();
            if (stats.GetHealth() <= 0)
            {
                this.enabled = false;
                Destroy(curElement);
            }
        }
    }
}
