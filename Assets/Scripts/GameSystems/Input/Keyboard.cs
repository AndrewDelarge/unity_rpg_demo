using Gameplay.Actors.Base;
using UI.Hud;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystems.Input
{
    public class Keyboard : BaseInput
    {

        public KeyCode actionKey = KeyCode.E;
        public KeyCode secondKey = KeyCode.Q;
        public KeyCode jumpKey = KeyCode.Space;
        private ActionBar actionBar;


        private Actor actor;

        private bool inited = false;

        private float bowPulledTime;
        
        public override void Init(Actor actor)
        {
            this.actor = actor;
            actionBar = GameController.instance.uiManager.GetActionBar();
            actionBar.onActionKeyClick += actor.MeleeAttack;
            actionBar.onSecKeyClick += actor.Dash;
            
            inited = true;
        }
        
        private void FixedUpdate()
        {
            if (! inited)
            {
                return;
            }
            
            horizontal = UnityEngine.Input.GetAxis("Horizontal");
            vertical   = UnityEngine.Input.GetAxis("Vertical");

            if (! IsKeyboard())
            {
                horizontal = actionBar.joystick.Horizontal;
                vertical = actionBar.joystick.Vertical;
            }

            if (! inited)
            {
                return;
            }
            
            Vector3 bowAimDir = GetBowStickValues();
            
            
            if (bowAimDir == Vector3.zero)
            {
                actor.StopAim();
            }
            else
            {
                actor.Aim(bowAimDir);
            }
            
            
            if (UnityEngine.Input.GetKeyDown(actionKey))
            {
                actor.MeleeAttack();
            }
            
            if (UnityEngine.Input.GetKeyDown(secondKey))
            {
                actor.Dash();
            }
            
            if (UnityEngine.Input.GetKeyDown(jumpKey))
            {
                actor.movement.Jump();
            }
        }

        bool IsKeyboard()
        {
            return UnityEngine.Input.GetAxis("Horizontal") != 0 || UnityEngine.Input.GetAxis("Vertical") != 0;
        }


        Vector3 GetBowStickValues()
        {
            return new Vector3(actionBar.aimControlStick.Horizontal, 0, actionBar.aimControlStick.Vertical);
        }
    }
    
    
    
}