using NPC;
using Player;
using UnityEngine;

namespace UI.Hud
{
    public class HealthBar : MonoBehaviour
    {
        [HideInInspector]
        public PlayerActor player;
        
        protected RectTransform value;
        protected GameObject healthBar;
        protected Vector2 size;

        protected float maxWidhth;


        public void Awake()
        {
//            Debug.Log("Health bar Awake");

            value = transform.Find("Value").gameObject.GetComponent<RectTransform>();
            size = value.sizeDelta;
            maxWidhth = size.x;
        }

        private void Start()
        {
//            Debug.Log("Health bar Start");

            player = PlayerManager.instance.player;
            player.combat.stats.onHealthChange += ChangeHeathBarValue;
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
            size.x = GetWidthFromHealth(health);
            value.sizeDelta = size;
        }

        
        private float GetWidthFromHealth(int health)
        {
            return (maxWidhth * health) / player.combat.stats.maxHealth;
        }
        
        private void ChangeHeathBarValue(int value, int health)
        {
            Debug.Log("player gets " + value);
            SetHealth(health);
        }
    }
}
