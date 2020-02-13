using System.Collections;
using System.Collections.Generic;
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
        public float attackDelay = 1f;
        public float combatCooldown = 6;
        public bool inCombat = false;
        public event System.Action OnAttack;
        public event System.Action OnAttackEnd;
        public event System.Action TargetDied;

        protected float lastAttackTime;

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
        
        
        
        public void Attack(CharacterStats targetActor, float attackRadius)
        {
            if (attackCooldown > 0)
            {
                return;
            }
            
            InvokeOnAttack();

            StartCoroutine(DoDamage(targetActor, attackDelay, attackRadius));

            EnterCombat();
            attackCooldown = attackSpeed;
            lastAttackTime = Time.time;
        }
        
        public void Attack(List<CharacterStats> targetActors, float attackRadius)
        {
            if (attackCooldown > 0)
            {
                return;
            }
            
            InvokeOnAttack();
            if (targetActors.Count > 0)
            {
                for (int i = 0; i < targetActors.Count; i++)
                {
                    StartCoroutine(DoDamage(targetActors[i], attackDelay, attackRadius));
                }
                
                EnterCombat();
            }

            attackCooldown = attackSpeed;
            lastAttackTime = Time.time;
        }

        protected void InvokeOnAttack()
        {
            if (OnAttack != null)
            {
                OnAttack();
            }
        }
        
        protected IEnumerator DoDamage(CharacterStats targetStats, float delay, float attackRadius)
        {
            yield return new WaitForSeconds(delay);

            if (Vector3.Distance(transform.position, targetStats.transform.position) <= attackRadius)
            {
                if (!stats.IsDead())
                {
                    targetStats.TakeDamage(stats.damage.GetValue());
                }
            }
            
            if (targetStats.currentHealth <= 0)
            {
                ExitCombat();
                if (TargetDied != null)
                {
                    TargetDied();
                }
            }

            if (OnAttackEnd != null)
            {
                OnAttackEnd();
            }
        }

        protected void EnterCombat()
        {
            inCombat = true;
        }
        
        protected void ExitCombat()
        {
//            Debug.Log(name + " Out from combat");
            inCombat = false;
        }
    }
}
