using System;
using Actors.Base;
using Actors.Base.Interface;
using GameSystems;
using UnityEngine;

namespace Gameplay.Scenario.Actions.AIActors
{
    public class ActionMoving : ScenarioAction
    {
        public GameObject controlableGameObject;
        public Transform targetPoint;

        public bool targetIsPlayer;

        private IControlable controlable;
        public override void Do()
        {
            base.Do();
            controlable = controlableGameObject.GetComponent<IControlable>();
            if (controlable == null)
            {
                onComplete?.Invoke();
                Debug.Log($"{name} action havent controllable entity");
                return;
            }

            if (targetIsPlayer)
            {
                Actor player = GameController.instance.playerManager.GetPlayer();
                if (player == null)
                {
                    doing = false;
                    return;
                }
                targetPoint = player.transform;
            }

            startTime = Time.time;
            doing = true;
            controlable.Follow(targetPoint, 1.2f);
        }


        public override void CheckDoing()
        {
            if (Vector3.Distance(controlable.GetTransform().position, targetPoint.position) <= 2f)
            {
                controlable.StopFollow();
                doing = false;
            }
        }


        public override void Stop()
        {
            controlable.StopFollow();
        }
    }
}