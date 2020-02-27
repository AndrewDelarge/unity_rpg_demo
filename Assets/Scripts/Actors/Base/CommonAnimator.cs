using UnityEngine;

namespace Actors.Base
{
    public class CommonAnimator : MonoBehaviour
    {
        public AnimationClip replaceableAttackClip;
        public AnimationClip[] defaultAttackAnimSet;
        public string attackLayerName = "Attack";


        private Combat combat;
        private IControlable movement;
        private Stats stats;
        protected const float locomotionAnimationSmoothTime = .1f;
        protected UnityEngine.Animator animator;
        protected AnimatorOverrideController overrideController;
        protected AnimationClip[] currentAttackAnimSet;

        protected int attackLayerId;


        protected void Init(Combat actCombat, IControlable actMovement, Stats actStats)
        {
            combat = actCombat;
            movement = actMovement;
            stats = actStats;
            animator = GetComponentInChildren<UnityEngine.Animator>();
            
            attackLayerId = animator.GetLayerIndex(attackLayerName);

            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;

            currentAttackAnimSet = defaultAttackAnimSet;

            combat.OnAttack += OnAttack;
            combat.OnAttackEnd += OnAttackEnd;
            stats.OnGetHit += OnGetHit;
        }
        
        protected virtual void Start()
        {
            
        }
        
        void Update()
        {
            float speedPercent = movement.GetCurrentMagnitude() / movement.GetSpeed();
            animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
        
            animator.SetBool("inCombat", combat.InCombat());
        }

        protected virtual void OnAttack()
        {
            animator.SetTrigger("attack");
//            animator.SetLayerWeight(attackLayerId, 1);
            int animIndedx = Random.Range(0, currentAttackAnimSet.Length);

            overrideController[replaceableAttackClip.name] = currentAttackAnimSet[animIndedx];
        }

        protected virtual void OnAttackEnd()
        {
//            animator.SetLayerWeight(attackLayerId, 0);
        }

        protected virtual void OnGetHit()
        {
            animator.SetTrigger("getHit");
        }
    }
}