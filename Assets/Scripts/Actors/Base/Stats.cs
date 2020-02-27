using System;
using Actors.Combat;
using Player;
using UnityEngine;
using Random = System.Random;

namespace Actors.Base
{
    public class Stats : MonoBehaviour
    {
        private const int STAMINA_COST = 2;
        private const int DAMAGE_COST = 3;
        private const int CRIT_CAP = 100;
        private const int ARMOR_CAP = 100;
        private const float CRIT_MULTIPLIER = 1.5f;
        
        [SerializeField]
        private int baseHealth = 100;
        [SerializeField]
        private float movementSpeed = 2f;
        private int currentHealth = 0;
        private bool isDead;

        
        /// <summary>
        ///     <para>Incrase health in: (stamina * STAMINA_COST)</para>
        /// </summary>
        public Stat stamina;
        /// <summary>
        ///     <para>Reduce taking damage in: (armor / ARMOR_CAP * 100) %</para>
        /// </summary>
        public Stat armor;
        public Stat attackPower;
        public Stat criticalChancePoints;

        public delegate void OnHealthChange(int value, int health);
        public delegate void OnDied(GameObject diedObject);

        public OnHealthChange onHealthChange;
        public OnDied onDied;
        public event System.Action OnGetHit;


        public void Init()
        {
            currentHealth = GetMaxHealth();
            isDead = false;
        }

        public float GetArmorMultiplier()
        {
            return armor.GetValue() / ARMOR_CAP;
        }

        public float GetCriticalChance()
        {
            return (criticalChancePoints.GetValue() / CRIT_CAP) * 100;
        }
        
        private int StaminaToHealth(Stat stamina)
        {
            return stamina.GetValue() * STAMINA_COST;
        }

        public int GetMaxHealth()
        {
            return baseHealth + StaminaToHealth(stamina);
        }

        public float GetMovementSpeed()
        {
            return movementSpeed;
        }
        
        public bool IsDead()
        {
            return isDead;
        }

        private int ConvertAPToDamage(Stat ap)
        {
            return ap.GetValue() * DAMAGE_COST;
        }
        
        public virtual int GetDamageValue()
        {
            int damage = ConvertAPToDamage(attackPower);
            int chance = Mathf.FloorToInt(GetCriticalChance());
            int throwed = UnityEngine.Random.Range(0, 99);
            
            
            if (throwed <= chance)
            {
                damage = Mathf.FloorToInt(attackPower.GetValue() * CRIT_MULTIPLIER);
            }

            // Damage Randomising 
            damage *= Mathf.FloorToInt(UnityEngine.Random.Range(.9f, 1.1f));

            return damage;
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
            
            if (OnGetHit != null)
            {
                OnGetHit();
            }

            if (onHealthChange != null)
            {
                onHealthChange.Invoke(- damageValue, currentHealth);
            }
        }
        
        public virtual void Die()
        {
            if (onDied != null && ! IsDead())
            {
                onDied.Invoke(gameObject);
            }
        }
    }
}