using Actors.Base.Interface;
using Actors.Base.StatsStuff;
using Animation;
using UnityEngine;

namespace Actors.Base
{
    [RequireComponent(typeof(Stats))]
    public abstract class CommonAnimator : BaseAnimator
    {
        public AnimationClip replaceableAttackClip;
        public AnimationClip[] defaultAttackAnimSet;
        public string attackLayerName = "Attack";
        public string attackInRunLayerName = "AttackInRun";


        protected Combat combat;
        protected IControlable movement;
        protected Stats stats;
        protected const float locomotionAnimationSmoothTime = .1f;
        protected AnimationClip[] currentAttackAnimSet;
        
        private void Awake()
        {
            enabled = false;
        }

        public virtual void Init(Combat actCombat, IControlable actMovement, Stats actStats)
        {
            base.Init();
            combat = actCombat;
            movement = actMovement;
            stats = actStats;
            currentAttackAnimSet = defaultAttackAnimSet;
            combat.OnAttack += OnAttack;
            combat.OnAttackEnd += OnAttackEnd;
            stats.onGetDamage += OnGetHit;
            enabled = true;
        }

        
        void Update()
        {
            float speedPercent = movement.GetCurrentMagnitude() / stats.GetMovementSpeed();
            animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
            animator.SetFloat("movementMultiplier", movement.GetSpeedMultiplier());

            ToggleAttackLayers(speedPercent);
            animator.SetBool("inCombat", combat.InCombat());
        }

        public void Disable()
        {
            animator.enabled = false;
        }

        public void Enable()
        {
            animator.enabled = true;
        }

        protected virtual void OnAttack()
        {
            animator.SetTrigger("attack");
            
            int animIndex = GetCurrentAttackAnimationIndex();

            overrideController[replaceableAttackClip.name] = currentAttackAnimSet[animIndex];
        }

        protected virtual void OnAttackEnd()
        {
        }

        protected virtual void OnGetHit(Damage damage)
        {
            animator.SetTrigger("getHit");
        }

        protected int GetCurrentAttackAnimationIndex()
        {
            int animIndex = combat.GetCurrentSuccessAttack();

            if (animIndex < currentAttackAnimSet.Length)
            {
                return animIndex;
            }
            
            return Random.Range(0, currentAttackAnimSet.Length);
        }


        public void Trigger(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }

        void ToggleAttackLayers(float speed)
        {
            int index = animator.GetLayerIndex(attackLayerName);
            int indexRun = animator.GetLayerIndex(attackInRunLayerName);

            if (indexRun == -1 || index == -1)
            {
                return;
            }

            if (speed > 0)
            {
                animator.SetLayerWeight(index, 0f);
                animator.SetLayerWeight(indexRun, 1f);
                return;
            }
            
            animator.SetLayerWeight(index, 1f);
            animator.SetLayerWeight(indexRun, 0f);
        }
        
    }
}