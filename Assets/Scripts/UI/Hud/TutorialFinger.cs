using UI.Base;
using UnityEngine;

namespace UI.Hud
{
    public class TutorialFinger : HideableUi
    {
        private const string SHOW_JOYSTICK_STATE = "FingerShowJoestick";
        private const string SHOW_ATTACK_STATE = "AttackButtonClick";
        private const string SHOW_NONE_STATE = "None";
        
        private Animator animator;
        
        
        public override void Init()
        {
            animator = GetComponentInChildren<Animator>();
            animator.gameObject.SetActive(false);
            curElement = animator.gameObject;
            inited = true;
            Hide(true);
        }

        public void ShowJoystick()
        {
            if (!inited)
                return;
            Show();
            animator.gameObject.SetActive(true);
            animator.Play(SHOW_JOYSTICK_STATE);
        }
        
        public void ShowAttack()
        {
            if (!inited)
                return;
            Show();
            animator.gameObject.SetActive(true);
            animator.Play(SHOW_ATTACK_STATE);
        }

        public void Stop()
        {
            if (!inited)
                return;
            animator.Play(SHOW_NONE_STATE);
            animator.gameObject.SetActive(false);
            Hide();
        }
    }

}