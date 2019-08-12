using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class CharacterAnimator : MonoBehaviour
    {

        public AnimationClip replaceableAttackClip;
    
        public AnimationClip[] defaultAttackAnimSet;
        protected AnimationClip[] currentAttackAnimSet;
    
        private const float locomotionAnimationSmoothTime = .1f;
    
        protected Animator animator;
        private NavMeshAgent agent;
        private CharacterCombat combat;
        protected AnimatorOverrideController overrideController;
    
        // Start is called before the first frame update
        protected virtual void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            combat = GetComponent<CharacterCombat>();
        
            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;

            currentAttackAnimSet = defaultAttackAnimSet;

            combat.OnAttack += OnAttack;
            combat.stats.OnGetHit += OnGetHit;
        }

        // Update is called once per frame
        void Update()
        {
            float speedPercent = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
        
            animator.SetBool("inCombat", combat.inCombat);
        }


        protected virtual void OnAttack()
        {
            animator.SetTrigger("attack");
            int animIndedx = Random.Range(0, currentAttackAnimSet.Length);

            overrideController[replaceableAttackClip.name] = currentAttackAnimSet[animIndedx];
        }

        protected virtual void OnGetHit()
        {
            animator.SetTrigger("getHit");
        }
    }
}
