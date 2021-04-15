using Gameplay.Actors.Base;
using Managers;
using UI.Hud;
using UnityEngine;

namespace GameSystems.Input
{
    public class JoystickInput : BaseInput
    {
        [SerializeField]
        private Actor actor;
        [SerializeField]
        private ActionBar actionBar;
        
        private Joystick joystick;
        

        private bool inited = false; 


        public override void Init()
        {
            actionBar = UIManager.Instance().GetActionBar();
            
            actionBar.onActionKeyClick += actor.MeleeAttack;
            actionBar.onSecKeyClick += actor.Dash;
            
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