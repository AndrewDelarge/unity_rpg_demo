using Actors.Base;
using GameSystems;
using GameSystems.Languages;
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
            tutorialFingerHud = GameController.instance.uiManager.GetTutorialFinger();
            tutorialFingerHud.ShowJoestick();
            doing = true;
        }

        public override void Stop()
        {
            tutorialFingerHud.Stop();
        }


        public override void CheckDoing()
        {
            Actor player = GameController.instance.playerManager.GetPlayer();

            if (player.movement.IsMoving())
            {
                Stop();
                doing = false;
            }
        }

    }
}