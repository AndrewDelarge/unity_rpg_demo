using System.Collections;
using Animation;
using UnityEngine;

namespace Gameplay.Scenario.Actions.Common
{
    public class PlayAnimationAction : ScenarioAction
    {
        public GameObject animatedObject;
        public AnimationClip clip;

        public int loops = 1;


        public bool loop;
        private BaseAnimator animator;
        public override void Do()
        {
            base.Do();
            animator = animatedObject.GetComponent<BaseAnimator>();


            if (animator != null)
            {
                StartCoroutine(PlayAnimation());
            }
            
        }


        IEnumerator PlayAnimation()
        {
            int currentLoop = 0;
            while (currentLoop < loops)
            {
                currentLoop++;
                animator.PlayAnimation(clip);
                yield return new WaitForSeconds(clip.length);
            }
        }
        

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }

        public override void CheckDoing()
        {
            return;
        }
    }
}

