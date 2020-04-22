using System.Collections.Generic;
using Actors;
using Actors.Base;
using GameCamera;
using UI.Hud;
using UnityEngine;

namespace GameInput
{
    public class Keyboard : BaseInput
    {

        public KeyCode actionKey = KeyCode.E;
        public KeyCode secondKey = KeyCode.Q;
        public KeyCode jumpKey = KeyCode.Space;
        private ActionBar actionBar;


        private Actor actor;

        private bool inited = false;
        public override void Init(Actor actor)
        {
            this.actor = actor;
            actionBar = GameController.instance.uiManager.GetActionBar();
            actionBar.onActionKeyClick += actor.MeleeAttack;

            inited = true;
        }

        private void FixedUpdate()
        {
            if (! inited)
            {
                return;
            }
            
            horizontal = Input.GetAxis("Horizontal");
            vertical   = Input.GetAxis("Vertical");

            if (!IsKeyboard())
            {
                horizontal = actionBar.joystick.Horizontal;
                vertical = actionBar.joystick.Vertical;
            }
            
            if (Input.GetKeyDown(actionKey))
            {
                actor.MeleeAttack();
            }
            
            if (Input.GetKeyDown(jumpKey))
            {
                actor.movement.Jump();
            }
        }

        bool IsKeyboard()
        {
            return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        }
    }
    
    
    
}