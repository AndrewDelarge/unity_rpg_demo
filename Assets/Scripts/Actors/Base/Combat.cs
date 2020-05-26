using System.Collections;
using System.Collections.Generic;
using Actors.Base.Interface;
using Actors.Base.StatsStuff;
using Gameplay.Projectile;
using GameSystems.Input;
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
        public float commonCombatSpeedMultiplier = 1f;
        public float aimTime;


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
        public event System.Action OnAimStart;
        public event System.Action OnAimEnd;

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
        
        protected virtual void FixedUpdate()
        {
            float lastAttackDelta = Time.time - lastAttackTime; 
            
            if (lastAttackDelta > combatCooldown && inCombat)
            {
                ExitCombat();
            }
            
            if (lastAttackDelta > (successAttackRowTime + GetCurrentMeleeAttackSpeed()))
            {
                successAttackInRow = 0;
            }
        }
        
        public virtual void MeleeAttack(List<IHealthable> targetStats)
        {
            EnterCombat();

            if (Time.time - lastAttackTime < GetCurrentMeleeAttackSpeed())
            {
                return;
            }
            
            lastAttackTime = Time.time;
            StartCoroutine(DoMeleeDamage(targetStats));

            successAttackInRow++;

            if (successAttackInRow == maxSuccessAttackInRow)
            {
                successAttackInRow = 0;
            }
        }

        public virtual void Aim()
        {
            if (aimTime == 0)
            {
                OnAimStart?.Invoke();
            }
            aimTime += Time.deltaTime;
        }
        
        public virtual void RangeAttack(Vector3 point)
        {
            //TODO rework
            // Get range weapon
            // Get weapon projectile
            OnAimEnd?.Invoke();
            float rangeWeaponDamage = 14f;

            aimTime = Mathf.Min(aimTime, 1);
            Vector3 pos = transform.position;
            pos.y += 1;
            GameObject gameObject = (GameObject) Instantiate(Resources.Load("Projectiles/Arrow"), pos, Quaternion.identity);
            gameObject.transform.LookAt(point);
            BaseProjectile projectile = gameObject.GetComponent<BaseProjectile>();
            
            Damage damage = new Damage(Mathf.CeilToInt(rangeWeaponDamage * aimTime), actor, false);
            
            projectile.angleSpeed = 1 - aimTime;
            projectile.ignorePlayer = true;
            projectile.Launch(damage);
            aimTime = 0;
        }

        
        
        protected virtual IEnumerator DoMeleeDamage(List<IHealthable> targetStats)
        {
            InvokeOnAttack();

            yield return new WaitForSeconds(curMAttackDelay / commonCombatSpeedMultiplier);

            for (int i = 0; i < targetStats.Count; i++)
            {
                if (InMeleeRange(targetStats[i].GetTransform()) && actor.vision.IsInViewAngle(targetStats[i].GetTransform()))
                {
                    if (!stats.IsDead())
                    {
                        Damage tmpDmg = stats.GetDamageValue();
                        Damage damage = new Damage(Mathf.FloorToInt(tmpDmg.GetValue() * curMAttackDamageMultiplier), actor, tmpDmg.IsCrit());

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
            return curMAttackSpeed / commonCombatSpeedMultiplier;
        }

        public bool InMeleeRange(Vector3 targetPosition)
        {
            return Vector3.Distance(transform.position, targetPosition) <= curMAttackRadius;
        }
        
        public bool InMeleeRange(Transform targetTransform)
        {
            return InMeleeRange(targetTransform.position);
        }
        
        public bool IsAttacking()
        {
            return Time.time - lastAttackTime <= GetCurrentMeleeAttackSpeed();
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
    }
}