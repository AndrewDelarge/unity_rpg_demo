using System;
using Actors.Base.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class HealthBar : MonoBehaviour
    {
        [HideInInspector]
        public IHealthable healthable;
        
        protected Image value;


        public void SetHealthable(IHealthable target)
        {
            if (healthable != null)
            {
                healthable.OnHealthChange -= ChangeHeathBarValue;
            }
            
            healthable = target;
            healthable.OnHealthChange += ChangeHeathBarValue;
            SetHealth(healthable.GetHealth());
        }
        
        public void Init()
        {
            value = transform.Find("Value").gameObject.GetComponent<Image>();
            
            if (healthable != null)
            {
                SetHealth(healthable.GetHealth());
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Visible(bool value)
        {
            gameObject.SetActive(value);
        }
        
        public void SetHealth(int health)
        {
            value.fillAmount = GetWidthFromHealth(health);
        }
        
        private float GetWidthFromHealth(int health)
        {
            return health / (float) healthable.GetMaxHealth();
        }
        
        private void ChangeHeathBarValue(object healthable, EventArgs args)
        {
            SetHealth(this.healthable.GetHealth());
        }
    }
}
