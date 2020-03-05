using System.Collections;
using System.Collections.Generic;
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
        private float lastAttackTime;
        private float currentMeleeAttackCooldown = 0f;
        
        private float meleeAttackSpeed = 1f;
        private float meleeAttackDelay = 0f;
        private float meleeAttackRaduis = 2f;

        protected Actor targetActor;
        protected Stats stats;
        protected BaseInput input;
        private bool inCombat = false;
        private Actor actor;
        
        public event System.Action OnAttack;
        public event System.Action OnAttackEnd;

        public delegate void OnTargetChange(Actor target);
        

        public virtual void Init(Stats actorStats, BaseInput baseInput)
        {
            stats = actorStats;
            SetInput(baseInput);
            
            // TODO Rework 
            actor = GetComponent<Actor>();
        }


        public virtual void SetInput(BaseInput baseInput)
        {
            if (input != null)
            {
                input.OnAttackPressed -= InputAttack;
            }
            
            input = baseInput;
            input.OnAttackPressed += InputAttack;
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
        
        private void Update()
        {
            currentMeleeAttackCooldown -= Time.deltaTime;

            if (Time.time - lastAttackTime > combatCooldown && inCombat)
            {
                ExitCombat();
            }
        }

        protected virtual void InputAttack()
        {
            Debug.Log("Base");
        }
        
        protected virtual void MeleeAttack(List<Stats> targetStats)
        {
            EnterCombat();

            if (currentMeleeAttackCooldown > 0)
            {
                return;
            }

            StartCoroutine(DoDamage(targetStats));

            currentMeleeAttackCooldown = meleeAttackSpeed;
            lastAttackTime = Time.time;
        }

        public bool InMeleeRange(Transform target)
        {
            return Vector3.Distance(transform.position, target.position) <= meleeAttackRaduis;
        }
        
        
        protected IEnumerator DoDamage(List<Stats> targetStats)
        {
            InvokeOnAttack();

            yield return new WaitForSeconds(meleeAttackDelay);

            for (int i = 0; i < targetStats.Count; i++)
            {
                if (InMeleeRange(targetStats[i].transform))
                {
                    if (!stats.IsDead())
                    {
                        Damage damage = new Damage(stats.GetDamageValue(), actor);
                        targetStats[i].TakeDamage(damage);
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