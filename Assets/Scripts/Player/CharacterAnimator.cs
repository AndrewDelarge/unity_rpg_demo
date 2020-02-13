using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class CharacterAnimator : MonoBehaviour
    {
        public AnimationClip replaceableAttackClip;
        public AnimationClip[] defaultAttackAnimSet;
        public string attackLayerName = "Attack";
        
        private NavMeshAgent agent;
        protected CharacterCombat combat;

        protected const float locomotionAnimationSmoothTime = .1f;
        protected Animator animator;
        protected AnimatorOverrideController overrideController;
        protected AnimationClip[] currentAttackAnimSet;

        protected int attackLayerId;

        protected virtual void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            combat = GetComponent<CharacterCombat>();
            
            attackLayerId = animator.GetLayerIndex(attackLayerName);

            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;

            currentAttackAnimSet = defaultAttackAnimSet;

            combat.OnAttack += OnAttack;
            combat.OnAttackEnd += OnAttackEnd;
            combat.stats.OnGetHit += OnGetHit;
        }
        
        void Update()
        {
            float speedPercent = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
        
            animator.SetBool("inCombat", combat.inCombat);
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
