using System;
using GameSystems;
using Managers;
using UI.Base;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    

    public class UIHudScreen : UIView
    {
        public ActionBar actionBar;
        public HealthBar healthBar;
        public UpperPanel upperPanel;
        public UIDamageFeed uiDamageFeed;

        public Text FPSTracker;
        public UITarget uiTarget;
        public TutorialFinger tutorialFinger;
        
        private float deltaTime;

        public override void Open()
        {
            healthBar.Init();
            actionBar.Init();
            uiTarget.Init();
            tutorialFinger.Init();
            uiDamageFeed.Init();
            
            PlayerManager.Instance().onPlayerInited += SetPlayer;
            base.Open();
        }

        private void SetPlayer()
        {
            var player = PlayerManager.Instance().CurrentPlayer;
            healthBar.SetHealthable(player.stats);
            actionBar.SetPlayer(player);
        }

//        void Update ()
//        {
//            if (FPSTracker != null)
//                UpdateFpsTracker();
//        }

        private void UpdateFpsTracker()
        {
            if (Time.timeScale != 1)
                return;
            
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            FPSTracker.text = Mathf.Ceil(fps).ToString();
        }
    }
}
