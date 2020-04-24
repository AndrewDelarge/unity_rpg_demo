using System;
using Actors.Base.Interface;
using Actors.Combat;
using Player;
using UnityEngine;
using Random = System.Random;

namespace Actors.Base
{
    public class Stats : MonoBehaviour, IHealthable
    {
        public event EventHandler<HealthChangeEventArgs> OnHealthChange;
        protected const int STAMINA_COEF = 10;
        protected const int ATTACK_POWER_COEF = 2;

        protected const int STAMINA_TO_LVL_COEF = 5;
        protected const int ATTACK_POWER_TO_LVL_COEF = 4;
        
        protected const float CRIT_CAP = 100;
        protected const float ARMOR_CAP = 100;
        protected const float CRIT_MULTIPLIER = 1.5f;

        
        [SerializeField]
        private int level = 1;
        [SerializeField]
        private int baseMaxHealth = 1;
        [SerializeField]
        private float movementSpeed = 2f;
        [SerializeField]
        private int currentHealth = 0;
        [SerializeField]
        private int currentMaxHealth = 0;
        private bool isDead;
        
        public Stat stamina;
        public Stat armor;
        public Stat attackPower;
        public Stat criticalChancePoints;

        public delegate void OnGetDamage(Damage damage);
        public delegate void OnDied(GameObject diedObject);

        public OnDied onDied;
        public OnGetDamage onGetDamage;

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public virtual void Init()
        {
            // Lvl coefs
            stamina.onChange = null;
            int startStamina = 21 + ((level - 1) * STAMINA_TO_LVL_COEF);
            int startAP = 20 + ((level - 1) * ATTACK_POWER_TO_LVL_COEF);
            stamina.RemoveModifier(startStamina);
            attackPower.RemoveModifier(startAP);
            stamina.AddModifier(startStamina);
            attackPower.AddModifier(startAP);
            
            currentHealth = GetMaxHealth();
            currentMaxHealth = GetMaxHealth();
            stamina.onChange += UpdateCurrentStats;
            
            OnHealthChange?.Invoke(this, new HealthChangeEventArgs());
            if (isDead)
            {
                onDied?.Invoke(gameObject);
            }
        }

        public float GetArmorMultiplier()
        {
            return 1 - armor.GetValue() / ARMOR_CAP;
        }

        public float GetCriticalChance()
        {
            return (criticalChancePoints.GetValue() / CRIT_CAP) * 100;
        }
        
        private int StaminaToHealth(Stat stamina)
        {
            return stamina.GetValue() * STAMINA_COEF;
        }

        public int GetMaxHealth()
        {
            return baseMaxHealth + StaminaToHealth(stamina);
        }

        public float GetMovementSpeed()
        {
            return movementSpeed;
        }
        
        
        protected int ConvertAPToDamage(Stat ap)
        {
            return ap.GetValue() / ATTACK_POWER_COEF;
        }
        
        public virtual int GetDamageValue()
        {
            int damage = ConvertAPToDamage(attackPower);
            int chance = Mathf.FloorToInt(GetCriticalChance());
            int throwed = UnityEngine.Random.Range(0, 99);

            if (throwed <= chance)
            {
                damage = Mathf.FloorToInt(damage * CRIT_MULTIPLIER);
            }

            // Damage Randomising 
            damage = Mathf.FloorToInt(damage * UnityEngine.Random.Range(.9f, 1.1f));

            return damage;
        }

        public bool IsHasLevel()
        {
            return true;
        }

        public int GetLevel()
        {
            return level;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public int GetHealth()
        {
            return currentHealth;
        }
        
        public virtual void TakeDamage(Damage damage)
        {
            int damageValue = Mathf.FloorToInt(damage.GetValue() * GetArmorMultiplier());
            
            damageValue = Mathf.Clamp(damageValue, 0, int.MaxValue);
            currentHealth -= damageValue;
            
            if (currentHealth <= 0)
            {
                Die();
            }
            
            onGetDamage?.Invoke(damage);


            HealthChangeEventArgs args = new HealthChangeEventArgs();
            args.healthChange = - damageValue;
            OnHealthChange?.Invoke(this, args);
        }
        
        protected virtual void Die()
        {
            if (onDied != null && ! IsDead())
            {
                isDead = true;
                onDied.Invoke(gameObject);
            }
        }

        protected void UpdateCurrentStats()
        {
            currentMaxHealth = GetMaxHealth();

            if (currentHealth > currentMaxHealth)
            {
                currentHealth = currentMaxHealth;
            }
            
            HealthChangeEventArgs args = new HealthChangeEventArgs();
            
            OnHealthChange?.Invoke(this, args);
        }
    }
    

}