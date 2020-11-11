using System;
using System.Collections;
using Gameplay.Actors.Base.Interface;
using Gameplay.Actors.Base.StatsStuff;
using UnityEngine;

namespace Gameplay.Objects
{
    public class Destroyable : MonoBehaviour, IHealthable
    {
        public event EventHandler<HealthChangeEventArgs> OnHealthChange;

        public int health = 1;
        public GameObject aliveObject;
        public GameObject deadObject;
        private int currentHealth;
        private Collider mainCollider;
        
        
        private void Start()
        {
            aliveObject.SetActive(true);
            deadObject.SetActive(false);
            currentHealth = health;
            OnHealthChange += OnGetDamage;
            mainCollider = GetComponent<Collider>();
        }


        void OnGetDamage(object obj, HealthChangeEventArgs arg)
        {
            if (IsDead())
            {
                mainCollider.enabled = false;
                aliveObject.SetActive(false);
                deadObject.SetActive(true);
            }
        }
        
        public bool IsDead()
        {
            return currentHealth <= 0;
        }

        public void TakeDamage(Damage damage)
        {
            if (IsDead())
            {
                return;
            }

            currentHealth -= damage.GetValue();
            OnHealthChange?.Invoke(this, new HealthChangeEventArgs());
        }

        public void Heal(Heal heal)
        {
            if (IsDead())
            {
                return;
            }

            currentHealth += heal.GetValue();
        }

        public int GetHealth()
        {
            return currentHealth;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public int GetMaxHealth()
        {
            return health;
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