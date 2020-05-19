using Actors.Base;
using GameSystems;
using GameSystems.Languages;
using UI.Hud;
using UnityEngine;

namespace Gameplay.Scenario.Actions.Tutorial
{
    public class GoToPoint : ScenarioAction
    {
        public Transform target;
        public Text message;
        
        
        
        public override void Do()
        {
            base.Do();
            GameController.instance.uiManager.SetPointTarget(target);
            doing = true;
        }

        public override void Stop()
        {
            GameController.instance.uiManager.SetPointTargetComplete();
            GameController.instance.uiManager.HidePointTarget();
        }

        public override void CheckDoing()
        {
            Actor player = GameController.instance.playerManager.GetPlayer();

            if (Vector3.Distance(player.transform.position, target.position) <= 2f)
            {
                Stop();
                doing = false;
            }
        }

    }
}