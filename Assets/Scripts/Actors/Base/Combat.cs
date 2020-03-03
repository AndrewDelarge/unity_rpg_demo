using System.Collections;
using System.Collections.Generic;
using Actors.Combat;
using GameInput;
using Player;
using UnityEngine;

namespace Actors.Base
{
    
    [RequireComponent(typeof(Stats))]
    public class Combat : MonoBehaviour
    {
        [SerializeField]
        private float combatCooldown = 6;
        private float lastAttackTime;
        private float currentMeleeAttackCooldown = 0f;
        
        private float meleeAttackSpeed = 1f;
        private float meleeAttackDamageDelay = 0f;
        private float meleeAttackDamageRaduis = 2f;
        
        private Stats stats;
        private bool inCombat = false;
        
        
        public event System.Action OnAttack;
        public event System.Action OnAttackEnd;
        public event System.Action TargetDied;

        public void Init(Stats actorStats, BaseInput input)
        {
            stats = actorStats;
        }

        public bool InCombat()
        {
            return inCombat;
        }
        
        private void Update()
        {
            currentMeleeAttackCooldown -= Time.deltaTime;

            if (Time.time - lastAttackTime > combatCooldown && inCombat)
            {
                ExitCombat();
            }
        }
        
        
        public void MeleeAttack(List<Stats> targetStats)
        {
            if (currentMeleeAttackCooldown > 0)
            {
                return;
            }
            
            InvokeOnAttack();

            StartCoroutine(DoDamage(targetStats));

            EnterCombat();
            currentMeleeAttackCooldown = meleeAttackSpeed;
            lastAttackTime = Time.time;
        }
        
        protected IEnumerator DoDamage(List<Stats> targetStats)
        {
            yield return new WaitForSeconds(meleeAttackDamageDelay);

            for (int i = 0; i < targetStats.Count; i++)
            {
                if (Vector3.Distance(transform.position, targetStats[i].transform.position) <= meleeAttackDamageRaduis)
                {
                    if (!stats.IsDead())
                    {
                        targetStats[i].TakeDamage(new Damage(stats.GetDamageValue()));
                    }
                }
            
                if (targetStats[i].IsDead())
                {
                    ExitCombat();
                    if (TargetDied != null)
                    {
                        TargetDied();
                    }
                }
            }
            

            if (OnAttackEnd != null)
            {
                OnAttackEnd();
            }
        }
        
        protected void InvokeOnAttack()
        {
            OnAttack?.Invoke();
        }
        
        protected void EnterCombat()
        {
            inCombat = true;
        }
        
        protected void ExitCombat()
        {
            inCombat = false;
        }
    }
}