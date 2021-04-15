using Gameplay.Actors.Base;
using GameSystems;
using GameSystems.Languages;
using Managers;
using UI.Hud;
using UnityEngine;

namespace Gameplay.Scenario.Actions.Tutorial
{
    public class MovementShow : ScenarioAction
    {
        private TutorialFinger tutorialFingerHud;
        public Text message;
        
        
        public override void Do()
        {
            base.Do();
            tutorialFingerHud = UIManager.Instance().GetTutorialFinger();
            tutorialFingerHud.ShowJoystick();
            doing = true;
        }

        public override void Stop()
        {
            tutorialFingerHud.Stop();
        }


        public override void CheckDoing()
        {
            Actor player = PlayerManager.Instance().GetPlayer();

            if (player.movement.IsMoving())
            {
                Stop();
                doing = false;
            }
        }

    }
}