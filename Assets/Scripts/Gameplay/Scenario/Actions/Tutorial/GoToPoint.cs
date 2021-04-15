using Gameplay.Actors.Base;
using GameSystems;
using GameSystems.Languages;
using Managers;
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
            UIManager.Instance().SetPointTarget(target);
            doing = true;
        }

        public override void Stop()
        {
            UIManager.Instance().SetPointTargetComplete();
            UIManager.Instance().HidePointTarget();
        }

        public override void CheckDoing()
        {
            Actor player = PlayerManager.Instance().GetPlayer();

            if (Vector3.Distance(player.transform.position, target.position) <= 2f)
            {
                Stop();
                doing = false;
            }
        }

    }
}