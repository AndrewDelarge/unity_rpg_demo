using NPC;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class HealthBar : MonoBehaviour
    {
        [HideInInspector]
        public PlayerActor player;
        
        protected Image value;
        protected GameObject healthBar;
        protected float maxWidhth;

        public void Awake()
        {
//            Debug.Log("Health bar Awake");
            value = transform.Find("Value").gameObject.GetComponent<Image>();
        }

        private void Start()
        {
//            Debug.Log("Health bar Start");
            player = PlayerManager.instance.player;
            
            player.characterStats.onHealthChange += ChangeHeathBarValue;
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
            return health / (float) player.combat.stats.maxHealth;
        }
        
        private void ChangeHeathBarValue(int value, int health)
        {
            Debug.Log("player gets " + value);
            SetHealth(health);
        }
    }
}
