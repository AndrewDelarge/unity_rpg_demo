using System;
using Actors.Base.Interface;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthUI : WorldspaceUi
    {
        private GameObject levelGameObject;
        private Image healthImage;
        private Text healthText;
        private IHealthable stats;
        private bool showed = false;
        
        public override void Init()
        {
            type = WorldUiType.Healthbars;
            base.Init();
            stats = GetComponent<IHealthable>();

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
        
        void SetHealth(object obj, EventArgs args)
        {
            if (!showed)
            {
                Show();
                showed = true;
            }
            
            if (curElement == null)
            {        
                return;
            }
            
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
