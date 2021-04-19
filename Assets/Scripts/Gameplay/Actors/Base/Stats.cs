using System;
using Gameplay.Actors.Base.Interface;
using Gameplay.Actors.Base.StatsStuff;
using UnityEngine;

namespace Gameplay.Actors.Base
{
    public class Stats : MonoBehaviour, IHealthable
    {
        protected const int STAMINA_COEF = 2;
        protected const int ATTACK_POWER_COEF = 10;

        protected const int STAMINA_TO_LVL_COEF = 5;
        protected const int ATTACK_POWER_TO_LVL_COEF = 15;
        
        protected const float CRIT_CAP = 100;
        protected const float ARMOR_CAP = 100;
        protected const float CRIT_MULTIPLIER = 2f;
        
        [SerializeField]
        protected Actor actor;
        
        [Header("Base stats")]
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
        
        [Header("Advanced stats")]
        public Stat stamina;
        public Stat armor;
        public Stat attackPower;
        public Stat criticalChancePoints;

        public delegate void OnGetDamage(Damage damage);
        public delegate void OnDied(GameObject diedObject);

        public OnDied onDied;
        public OnGetDamage onGetDamage;
        public Damage lastDamage;
        
        public event EventHandler<HealthChangeEventArgs> OnHealthChange;

        public virtual void Init()
        {
            int startStamina = 21 + ((level - 1) * STAMINA_TO_LVL_COEF);
            int startAP = 20 + ((level - 1) * ATTACK_POWER_TO_LVL_COEF);
            
            stamina.onChange = null;
            
            stamina.RemoveModifier(startStamina);
            stamina.AddModifier(startStamina);

            attackPower.RemoveModifier(startAP);
            attackPower.AddModifier(startAP);
            
            currentHealth = GetMaxHealth();
            currentMaxHealth = GetMaxHealth();
            
            stamina.onChange += UpdateCurrentStats;
            
            if (isDead)
                onDied?.Invoke(gameObject);
        }
        
        public virtual Damage GetDamageValue(bool throwCrit = true, bool randomize = true, float multiplier = 1f)
        {
            int damage = ConvertAPToDamage(attackPower);
            int chance = Mathf.FloorToInt(GetCriticalChance());
            int throwed = UnityEngine.Random.Range(0, 99);

            if (throwed <= chance && throwCrit)
                damage = Mathf.FloorToInt(damage * CRIT_MULTIPLIER);

            // Damage Randomising 
            if (randomize)
                damage = Mathf.FloorToInt(damage * UnityEngine.Random.Range(.9f, 1.1f));
            
            damage = Mathf.FloorToInt(damage * multiplier);

            return new Damage(damage, actor, throwed <= chance);
        }
        
        public virtual void TakeDamage(Damage damage)
        {
            if (currentHealth <= 0)
                return;
            
            int damageValue = Mathf.FloorToInt(damage.GetValue() * GetArmorMultiplier());
            damageValue = Mathf.Clamp(damageValue, 0, int.MaxValue);
            
            currentHealth -= damageValue;
            
            var finalDamage = new Damage(damageValue, damage.GetOwner(), damage.IsCrit());

            lastDamage = finalDamage;
            
            if (currentHealth <= 0)
                Die();
            
            onGetDamage?.Invoke(finalDamage);

            HealthChangeEventArgs args = new HealthChangeEventArgs
            {
                healthChange = -damageValue, actor = actor, modifier = finalDamage
            };

            OnHealthChange?.Invoke(this, args);
        }
        
        public void Heal(Heal heal)
        {
            float healAmount = heal.GetValue() / 100f * GetMaxHealth();
            currentHealth += Mathf.FloorToInt(healAmount);

            if (currentHealth > currentMaxHealth)
                currentHealth = currentMaxHealth;
            
            HealthChangeEventArgs args = new HealthChangeEventArgs();
            args.healthChange = Mathf.FloorToInt(healAmount);
            args.modifier = heal;
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
                currentHealth = currentMaxHealth;
            
            HealthChangeEventArgs args = new HealthChangeEventArgs();
            
            OnHealthChange?.Invoke(this, args);
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
        
        public void SetMovementSpeed(float speed)
        {
            movementSpeed = speed;
        }
        
        public Transform GetTransform()
        {
            return transform;
        }

    }
    

}