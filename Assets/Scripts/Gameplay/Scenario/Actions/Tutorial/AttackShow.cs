using Gameplay.Actors.Base.Interface;
using GameSystems;
using GameSystems.Languages;
using UI.Hud;
using UnityEngine;

namespace Gameplay.Scenario.Actions.Tutorial
{
    public class AttackShow : ScenarioAction
    {
        public GameObject objectTohit;
        private TutorialFinger tutorialFingerHud;
        private IHealthable healthable;
        public Text message;
        
        
        public override void Do()
        {
            base.Do();
            tutorialFingerHud = GameController.instance.uiManager.GetTutorialFinger();
            tutorialFingerHud.ShowAttack();
         
            doing = true;
            healthable = objectTohit.GetComponent<IHealthable>();
            healthable.OnHealthChange += OnGetHit;
        }

        public override void Stop()
        {
            tutorialFingerHud.Stop();
        }


        private void OnGetHit(object obj, HealthChangeEventArgs eventArgs)
        {
            Stop();
            doing = false;
            healthable.OnHealthChange -= OnGetHit;
        }

        public override void CheckDoing()
        {
            if (healthable.IsDead())
            {
                Stop();
                doing = false;
                healthable.OnHealthChange -= OnGetHit;
            }
        }

    }
}