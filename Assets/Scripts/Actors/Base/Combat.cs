using System.Collections;
using System.Collections.Generic;
using Actors.Base.Interface;
using Actors.Combat;
using GameInput;
using UnityEngine;

namespace Actors.Base
{
    
    [RequireComponent(typeof(Stats))]
    public class Combat : MonoBehaviour
    {
        [SerializeField]
        private float combatCooldown = 6;
        protected float lastAttackTime;
        
        protected int successAttackInRow = 0;
        protected int maxSuccessAttackInRow = 3;
        protected float successAttackRowTime = 1f;
        
        public float meleeAttackSpeed = 1f;
        public float meleeAttackDelay = 0f;
        public float meleeAttackRaduis = 2f;
        public float meleeAttackDamageMultiplier = 1f;


        protected float curMAttackSpeed;
        protected float curMAttackDelay;
        protected float curMAttackRadius;
        protected float curMAttackDamageMultiplier;
        protected Actor targetActor;
        protected Actor actor;
        protected Stats stats;
        private bool inCombat = false;

        public event System.Action OnAttack;
        public event System.Action OnAttackEnd;

        public delegate void OnTargetChange(Actor target);

        private void Awake()
        {
            enabled = false;
        }

        public virtual void Init(Stats actorStats, BaseInput baseInput)
        {
            stats = actorStats;
            curMAttackSpeed = meleeAttackSpeed;
            curMAttackDelay = meleeAttackDelay;
            curMAttackRadius = meleeAttackRaduis;
            curMAttackDamageMultiplier = meleeAttackDamageMultiplier;
            OnAttack = null;
            OnAttackEnd = null;
            // TODO Rework 
            actor = GetComponent<Actor>();
            enabled = true;
        }


        public void SetTarget(Actor target)
        {
            targetActor = target;
        }
        
        public Actor GetTarget()
        {
            return targetActor;
        }
        
        public bool InCombat()
        {
            return inCombat;
        }
        
        protected virtual void Update()
        {
            float lastAttackDelta = Time.time - lastAttackTime; 
            
            if (lastAttackDelta > combatCooldown && inCombat)
            {
                ExitCombat();
            }
            
            if (lastAttackDelta > (successAttackRowTime + curMAttackSpeed))
            {
                successAttackInRow = 0;
            }
        }
        
        public virtual void MeleeAttack(List<IHealthable> targetStats)
        {
            EnterCombat();

            if (Time.time - lastAttackTime < curMAttackSpeed)
            {
                return;
            }

            StartCoroutine(DoMeleeDamage(targetStats));

            lastAttackTime = Time.time;
            successAttackInRow++;

            if (successAttackInRow == maxSuccessAttackInRow)
            {
                successAttackInRow = 0;
            }
        }

        public bool InMeleeRange(Vector3 targetPosition)
        {
            return Vector3.Distance(transform.position, targetPosition) <= curMAttackRadius;
        }
        
        
        protected virtual IEnumerator DoMeleeDamage(List<IHealthable> targetStats)
        {
            InvokeOnAttack();

            yield return new WaitForSeconds(curMAttackDelay);

            for (int i = 0; i < targetStats.Count; i++)
            {
                if (InMeleeRange(targetStats[i].GetPosition()) && actor.vision.IsInViewAngle(targetStats[i].GetPosition()))
                {
                    if (!stats.IsDead())
                    {
                        Damage damage = new Damage(Mathf.FloorToInt(stats.GetDamageValue() * curMAttackDamageMultiplier), actor);

//                        Debug.Log($"Attack in row :{successAttackInRow} damage: {damage.GetValue()}" );
                        targetStats[i].TakeDamage(damage);
                    }
                }
            }

            OnAttackEnd?.Invoke();
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

        public int GetCurrentSuccessAttack()
        {
            return successAttackInRow;
        }
        
        public int GetMaxSuccessAttack()
        {
            return maxSuccessAttackInRow;
        }

        public float GetCurrentMeleeAttackSpeed()
        {
            return curMAttackSpeed;
        }
    }
}