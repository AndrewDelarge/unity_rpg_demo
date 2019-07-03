using System.Collections;
using NPC;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterStats))]
    public class CharacterCombat : MonoBehaviour
    {
        private CharacterStats stats;
        
        public float attackSpeed = 1f;
        public float attackCooldown = 0f;
        public float attackDelay = .6f;
        public float combatCooldown = 6;
        private float lastAttackTime;
        
        public bool inCombat = false;


        public event System.Action OnAttack;
        private void Start()
        {
            stats = GetComponent<CharacterStats>();
        }

        private void Update()
        {
            attackCooldown -= Time.deltaTime;

            if (Time.time - lastAttackTime > combatCooldown)
            {
                inCombat = false;
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

            inCombat = true;
            attackCooldown = 1f / attackSpeed;
            lastAttackTime = Time.time;
        }


        IEnumerator DoDamage(CharacterStats targetStats, float delay)
        {
            yield return new WaitForSeconds(delay);

            targetStats.TakeDamage(stats.damage.GetValue());
            if (targetStats.currentHealth <= 0)
            {
                inCombat = false;
            }
        }
    }
}
