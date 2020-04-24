using System;
using System.Linq.Expressions;
using Actors.Base.Interface;
using Actors.Combat;
using UnityEngine;

namespace Player
{
    public class CharacterStats : MonoBehaviour, IHealthable
    {
        public int maxHealth = 100;
        public int currentHealth { get; private set; }
        public Stat armor;
        public Stat damage;
        public delegate void OnDied(GameObject diedObject);
        public OnDied onDied;
        public event System.Action OnGetHit;
        
        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public event EventHandler<HealthChangeEventArgs> OnHealthChange;

        public void TakeDamage(Damage damage)
        {
            TakeDamage(damage.GetValue());
        }

        public int GetHealth()
        {
            return this.currentHealth;
        }

        public void TakeDamage(int damage)
        {
            damage -= armor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
            currentHealth -= damage;

//            Debug.Log(transform.name + " take damage " + damage);
            
            if (currentHealth <= 0)
            {
                Die();
            }
            
            if (OnGetHit != null)
            {
                OnGetHit();
            }

            OnHealthChange?.Invoke(this, new HealthChangeEventArgs());

        }

        public void Heal(int heal)
        {
            heal = Mathf.Clamp(heal, 0, (maxHealth - currentHealth));

            currentHealth += heal;
            
            OnHealthChange?.Invoke(this, new HealthChangeEventArgs());
        }

        public virtual void Die()
        {
            if (onDied != null)
            {
                onDied.Invoke(gameObject);
            }

            onDied = null;
//            Debug.Log(transform.name + " died");
        }

        public bool IsDead()
        {
            return currentHealth <= 0;
        }

        public bool IsHasLevel()
        {
            return false;
        }

        public int GetLevel()
        {
            return -1;
        }
    }
}
