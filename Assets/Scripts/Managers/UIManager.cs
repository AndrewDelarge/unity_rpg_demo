using CoreUtils;
using Gameplay.Actors.Base.Interface;
using GameSystems;
using UI.Hud;
using UI.MainMenu;
using UnityEngine;

namespace Managers
{
    public class UIManager : SingletonDD<UIManager>
    {
        private bool isInited;
        [SerializeField]
        private UI.Hud.UI uiHud;
        
        public void Init()
        {
            if (isInited)
            {
                return;
            }

            GameManager.Instance().sceneController.OnSceneLoaded += RegisterEvents;
            isInited = true;
            HideHud();
        }
        
        public void SetPlayer(GameObject player)
        {
            uiHud.Init();
            uiHud.healthBar.SetHealthable(player.GetComponent<IHealthable>());
            uiHud.actionBar.SetPlayer(player);
        }


        public GameObject GetHudGameObject()
        {
            return uiHud.gameObject;
        }
        
        public ActionBar GetActionBar()
        {
            return uiHud.actionBar;
        }
        
        public QuestPanel GetQuestPanel()
        {
            return uiHud.questPanel;
        }


        public Loading GetLoadScreen()
        {
            return uiHud.loadingScreen;
        }

        public void HideHud()
        {
            uiHud.HideHud();
        }
        
        public void ShowHud()
        {
            uiHud.ShowHud();
        }

        public void SetPointTarget(Transform target)
        {
            uiHud.uiTarget.SetTarget(target);
        }
        
        public void SetPointTargetComplete()
        {
            uiHud.uiTarget.Done();
        }

        public TutorialFinger GetTutorialFinger()
        {
            return uiHud.tutorialFinger;
        }
        
        public void HidePointTarget()
        {
            uiHud.uiTarget.Hide();
        }

        public void AddDamageFeed(Transform target, string value, bool isCrit)
        {
            uiHud.damageFeed.Add(target, value, isCrit);
        }

        private void RegisterEvents()
        {
            GameManager.Instance().sceneController.LevelController.OnLevelUnload += HidePointTarget;
            GameManager.Instance().sceneController.LevelController.OnLevelUnload += uiHud.tutorialFinger.Stop;
            
            GameManager.Instance().sceneController.OnSceneLoaded -= RegisterEvents;

        }
    }
}