using System.Collections;
using NPC;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterStats))]
    public class CharacterCombat : MonoBehaviour
    {
        public CharacterStats stats;
        
        public float attackSpeed = 1f;
        public float attackCooldown = 0f;
        public float attackDelay = .6f;
        public float combatCooldown = 6;
        private float lastAttackTime;
        
        public bool inCombat = false;


        public event System.Action OnAttack;
        public event System.Action TargetDied;
        private void Awake()
        {
            stats = GetComponent<CharacterStats>();
        }

        private void Update()
        {
            attackCooldown -= Time.deltaTime;

            if (Time.time - lastAttackTime > combatCooldown && inCombat)
            {
                ExitCombat();
            }
        }

        public void Attack(CharacterStats targetActor)
        {
            if (attackCooldown > 0)
            {
                return;
            }
            
            
            StartCoroutine(DoDamage(targetActor, attackDelay));

            if (OnAttack != null)
            {
                OnAttack();
            }

            EnterCombat();
            attackCooldown = 1f / attackSpeed;
            lastAttackTime = Time.time;
        }


        IEnumerator DoDamage(CharacterStats targetStats, float delay)
        {
            yield return new WaitForSeconds(delay);

            targetStats.TakeDamage(stats.damage.GetValue());
            if (targetStats.currentHealth <= 0)
            {
                ExitCombat();
                if (TargetDied != null)
                {
                    TargetDied();
                }
            }
        }

        void EnterCombat()
        {
            inCombat = true;
        }
        
        void ExitCombat()
        {
            Debug.Log(name + " Out from combat");
            inCombat = false;
        }
    }
}
