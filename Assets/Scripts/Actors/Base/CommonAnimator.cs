using Actors.Base.Interface;
using Actors.Combat;
using Actors.Player;
using UnityEngine;

namespace Actors.Base
{
    [RequireComponent(typeof(Stats))]
    public abstract class CommonAnimator : MonoBehaviour
    {
        public AnimationClip replaceableAttackClip;
        public AnimationClip[] defaultAttackAnimSet;
        public string attackLayerName = "Attack";


        protected Combat combat;
        protected IControlable movement;
        protected Stats stats;
        protected const float locomotionAnimationSmoothTime = .1f;
        protected Animator animator;
        protected AnimatorOverrideController overrideController;
        protected AnimationClip[] currentAttackAnimSet;


        public virtual void Init(Combat actCombat, IControlable actMovement, Stats actStats)
        {
            combat = actCombat;
            movement = actMovement;
            stats = actStats;
            animator = GetComponentInChildren<Animator>();
            
            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;

            currentAttackAnimSet = defaultAttackAnimSet;

            combat.OnAttack += OnAttack;
            combat.OnAttackEnd += OnAttackEnd;
            stats.onGetDamage += OnGetHit;
        }

        
        void Update()
        {
            float speedPercent = movement.GetCurrentMagnitude() / stats.GetMovementSpeed();
            animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
        
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
    }
}