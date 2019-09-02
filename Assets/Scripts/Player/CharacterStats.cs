using UnityEngine;

namespace Player
{
    public class CharacterStats : MonoBehaviour
    {

        public int maxHealth = 100;
        public int currentHealth { get; private set; }
        public Stat armor;
        public Stat damage;
        public delegate void OnDied(GameObject diedObject);
        public OnDied onDied;
        public delegate void OnHealthChange(int value, int health);
        public OnHealthChange onHealthChange;
        public event System.Action OnGetHit;
        
        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            damage -= armor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
            currentHealth -= damage;

            Debug.Log(transform.name + " take damage " + damage);
            
            if (currentHealth <= 0)
            {
                Die();
            }
            
            if (OnGetHit != null)
            {
                OnGetHit();
            }

            if (onHealthChange != null)
            {
                onHealthChange.Invoke(- damage, currentHealth);
            }
        }

        public void Heal(int heal)
        {
            heal = Mathf.Clamp(heal, 0, (maxHealth - currentHealth));

            currentHealth += heal;
            
            if (onHealthChange != null)
            {
                onHealthChange.Invoke(heal, currentHealth);
            }
        }

        public virtual void Die()
        {
            if (onDied != null)
            {
                onDied.Invoke(gameObject);
            }
            Debug.Log(transform.name + " died");
        }

        public bool IsDead()
        {
            return currentHealth <= 0;
        }
        
        
    }
}
