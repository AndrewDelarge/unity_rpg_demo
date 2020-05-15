using UnityEngine;

namespace Animation
{
    public class BaseAnimator : MonoBehaviour
    {

        protected Animator animator;
        protected AnimatorOverrideController overrideController;

        public void Init()
        {
            animator = GetComponentInChildren<Animator>();
            
           
        }
    }
}