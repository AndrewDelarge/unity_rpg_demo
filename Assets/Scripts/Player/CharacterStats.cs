using System.Numerics;
using UnityEngine;

namespace Player
{
    public class CharacterStats : MonoBehaviour
    {

        public int maxHealth = 100;

        public int currentHealth { get; private set; }

        public Stat armor;
        public Stat damage;

       
        private void Awake()
        {
            currentHealth = maxHealth;
        }


        public void TakeDamage(int damage)
        {
            damage -= armor.GetValue();
            currentHealth -= damage;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
            
            Debug.Log(transform.name + " take damage " + damage);

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            Debug.Log(transform.name + " died");

            //Character die
        }
        
        
    }
}
