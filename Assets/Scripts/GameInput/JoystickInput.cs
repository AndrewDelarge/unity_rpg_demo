using Actors.Base;
using UI.Hud;
using UnityEngine;

namespace GameInput
{
    public class JoystickInput : BaseInput
    {
        [SerializeField]
        private ActionBar actionBar;
        
        private Joystick joystick;
        

        private bool inited = false; 

        private Actor actor;
        public override void Init(Actor actor)
        {
            this.actor = actor;
            actionBar = GameController.instance.uiManager.GetActionBar();
            
            actionBar.onActionKeyClick += actor.MeleeAttack;
            
            inited = true;
        }

        void FixedUpdate()
        {
            if (! inited)
            {
                return;
            }
            
            horizontal = actionBar.joystick.Horizontal;
            vertical = actionBar.joystick.Vertical;
        }


    }
}