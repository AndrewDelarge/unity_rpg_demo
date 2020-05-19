using UnityEngine;

namespace Animation
{
    public class BaseAnimator : MonoBehaviour
    {
        public string customStateName = "Custom";
        public string defaultAnimName = "Default";
        
        protected Animator animator;
        protected AnimatorOverrideController overrideController;

        private AnimationClip defaultAnimation;
        public void Init()
        {
            animator = GetComponentInChildren<Animator>();
            overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;
        }
        public void PlayAnimation(AnimationClip clip)
        {
            defaultAnimation = overrideController[defaultAnimName];
            overrideController[defaultAnimName] = clip;
            animator.Play(customStateName);
        }
    }
}