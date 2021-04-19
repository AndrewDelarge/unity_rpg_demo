using System.Collections;
using System.Collections.Generic;
using Gameplay.Actors.Base.Interface;
using GameSystems.Input;
using UnityEngine;

namespace Gameplay.Actors.Base
{
    public class Combat : MonoBehaviour
    {
        [SerializeField]
        protected float combatCooldown = 6;
        [SerializeField]
        protected Vision vision;
        [SerializeField]
        protected Stats stats;
        
        [Header("Default settings")]
        public float meleeAttackSpeed = 1f;
        public float meleeAttackDelay = 0f;
        public float meleeAttackRaduis = 2f;
        public float meleeAttackDamageMultiplier = 1f;
        public float commonCombatSpeedMultiplier = 1f;
        public float rangeAttackCooldown = .4f;

        public float aimTime { get; protected set; }

        protected float lastAttackTime;
        protected float lastRangeAttackTime;
        protected float successAttackRowTime = 1f;
        
        protected int successAttackInRow = 0;
        protected int maxSuccessAttackInRow = 3;
        
        protected float curMAttackDuration;
        protected float curMAttackDelay;
        protected float curMAttackRadius;
        protected float curMAttackDamageMultiplier;
        
        private bool inCombat = false;
        
        public event System.Action OnAttack;
        public event System.Action OnAttackEnd;
        public delegate void OnAimStart();
        public delegate void OnAimEnd();
        public delegate void OnAimBreak();

        public OnAimStart onAimStart;
        public OnAimEnd onAimEnd;
        public OnAimBreak onAimBreak;
        
        public virtual void Init()
        {
            curMAttackDuration = meleeAttackSpeed;
            curMAttackDelay = meleeAttackDelay;
            curMAttackRadius = meleeAttackRaduis;
            curMAttackDamageMultiplier = meleeAttackDamageMultiplier;
            
            OnAttack = null;
            OnAttackEnd = null;
            enabled = true;
        }
        
        protected virtual void FixedUpdate()
        {
            float lastAttackDelta = Time.time - lastAttackTime;
            float lastRangeAttackDelta = Time.time - lastRangeAttackTime;
            
            if (lastAttackDelta > combatCooldown && lastRangeAttackDelta > rangeAttackCooldown && inCombat)
                ExitCombat();
            
            if (lastAttackDelta > (successAttackRowTime + GetCurrentMeleeAttackDuration()))
                successAttackInRow = 0;
        }
        
        public virtual void MeleeAttack(List<IHealthable> targetStats)
        {
            if (IsMeleeAttacking())
                return;
            
            EnterCombat();

            lastAttackTime = Time.time;
            InvokeOnAttack();
            StartCoroutine(DoMeleeDamage(targetStats));
        }

        public virtual void Aim()
        {
            if (aimTime == 0)
                onAimStart?.Invoke();
            
            aimTime += Time.deltaTime;
        }
        
        public virtual void RangeAttack(Vector3 point)
        {
            if (Time.time - lastRangeAttackTime < rangeAttackCooldown)
                return;

            aimTime = 0;
            onAimEnd?.Invoke();
        }
        
        protected virtual IEnumerator DoMeleeDamage(List<IHealthable> targetStats)
        {
            yield return new WaitForSeconds(curMAttackDelay / commonCombatSpeedMultiplier);
            
            for (int i = 0; i < targetStats.Count; i++)
            {
                if (! InMeleeZone(targetStats[i].GetTransform()))
                    continue;
                
                
                if (!stats.IsDead())
                    targetStats[i].TakeDamage(stats.GetDamageValue(true, true, curMAttackDamageMultiplier));
            }

            successAttackInRow++;

            if (successAttackInRow == maxSuccessAttackInRow)
                successAttackInRow = 0;
            
            OnAttackEnd?.Invoke();
        }

        protected bool InMeleeZone(Transform transform)
        {
            return InMeleeRange(transform) && vision.IsInViewAngle(transform);
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

        public int GetCurrentSuccessAttack() => successAttackInRow;
        
        public int GetMaxSuccessAttack()
        {
            return maxSuccessAttackInRow;
        }

        public bool IsLastCombatAttack()
        {
            return successAttackInRow == maxSuccessAttackInRow - 1;
        }
        
        public float GetCurrentMeleeAttackDuration()
        {
            return curMAttackDuration / commonCombatSpeedMultiplier;
        }

        public bool InMeleeRange(Vector3 targetPosition)
        {
            return Vector3.Distance(transform.position, targetPosition) <= curMAttackRadius;
        }
        
        public bool InMeleeRange(Transform targetTransform)
        {
            return InMeleeRange(targetTransform.position);
        }
        
        public bool IsMeleeAttacking()
        {
            return Time.time - lastAttackTime < GetCurrentMeleeAttackDuration();
        }

        public bool IsRangeCooldown()
        {
            return Time.time - lastRangeAttackTime <= rangeAttackCooldown;
        }

        public bool InCombat() => inCombat;
        
        public float GetRangeCooldown() => Time.time - lastRangeAttackTime;
    }
}