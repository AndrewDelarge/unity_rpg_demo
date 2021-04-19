using System;
using System.Collections.Generic;
using CoreUtils;
using Gameplay.Actors.Base.Interface;
using GameSystems;
using UI.Base;
using UI.Hud;
using UI.Inventory;
using UI.MainMenu;
using UI.Windows;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    [Serializable]
    public struct UITemplates
    {
        public GameObject damageTextFeed;
    }
    public class UIManager : SingletonDD<UIManager>
    {
        public enum UIScreens
        {
            None,
            InGameHud,
            MainMenu
        }
        
        public enum UIWindows
        {
            Pause,
            Inventory,
            LevelComplete
        }

        private Dictionary<UIScreens, UIView> uiScreens = new Dictionary<UIScreens, UIView>();
        private Dictionary<UIWindows, UIWindow> uiWindows = new Dictionary<UIWindows, UIWindow>();

        private UIView currentScreen;
        private List<UIWindow> currentWindows = new List<UIWindow>();
        
        private bool isInited;
        
        [Header("Game screens")]
        [SerializeField] private UIHudScreen uiScreenHud;
        [SerializeField] private UIMainMenu uiScreenMainMenu;
        
        [Header("Game windows")]
        [SerializeField] private PauseMenu uiPauseMenu;
        [SerializeField] private Inventory uiInventory;
        [SerializeField] private UILevelComplete uiLevelComplete;
        
        [Header("LoadingScreen")]
        [SerializeField]
        private Loading loading;
        
        [Header("UI Templates")]
        [SerializeField]
        public UITemplates uiTemplates;
        
        public void Init()
        {
            if (isInited)
                return;

            InitViews();
            RegisterViewsEvents();
            
            InitScreensDictionary();
            InitWindowsDictionary();

            SetScreen(UIScreens.None);
            isInited = true;
        }

        private void RegisterViewsEvents() {}

        private void InitViews()
        {
            uiInventory.Init();
        }

        private void InitScreensDictionary()
        {
            uiScreens.Add(UIScreens.InGameHud, uiScreenHud);
            uiScreens.Add(UIScreens.MainMenu, uiScreenMainMenu);
        }
        
        private void InitWindowsDictionary()
        {
            uiWindows.Add(UIWindows.Pause, uiPauseMenu);
            uiWindows.Add(UIWindows.Inventory, uiInventory);
            uiWindows.Add(UIWindows.LevelComplete, uiLevelComplete);
        }

        public void SetScreen(UIScreens screen)
        {
            if (screen == UIScreens.None)
                if (currentScreen != null)
                {
                    currentScreen.Close();
                    return;
                }
            
            if (!uiScreens.ContainsKey(screen))
            {
                Debug.Log($" # -UI- # Screen not defined for screen type: `{screen}`");
                return;
            }

            var newScreen = uiScreens[screen];
            
            if (currentScreen == newScreen)
                return;
            
            if (currentScreen != null)
                currentScreen.Close();

            currentScreen = newScreen;
            currentScreen.Open();
        }

        public void OpenWindow(UIWindows windowType)
        {
            if (! IsWindowExists(windowType))
            {
                Debug.Log($" # -UI- # Window not defined for window type: `{windowType}`");
                return;
            }

            if (IsWindowOpened(windowType))
            {
                Debug.Log($" # -UI- # Window `{windowType}` already opened!");
                return;
            }
            
            var newWindow = uiWindows[windowType];
            
            if (newWindow.CloseOthers)
                CloseAllWindows();
            
            
            newWindow.Open();
            newWindow.transform.SetAsLastSibling();
            currentWindows.Add(newWindow);
        }

        public bool IsWindowExists(UIWindows type)
        {
            return uiWindows.ContainsKey(type); 
        }
        
        public bool IsWindowOpened(UIWindows type)
        {
            if (!IsWindowExists(type))
                return false;
            
            return currentWindows.Exists(x => x == uiWindows[type]);
        }

        public void CloseWindow(UIWindows type)
        {
            if (!IsWindowExists(type))
                return;

            var window = currentWindows.Find(x => x == uiWindows[type]);
            currentWindows.Remove(window);
            window.Close();
        }
        
        private void CloseAllWindows()
        {
            foreach (var window in currentWindows)
                window.Close();
        }

        public GameObject GetHudGameObject()
        {
            return uiScreenHud.gameObject;
        }
        
        public ActionBar GetActionBar()
        {
            return uiScreenHud.actionBar;
        }
        
        public QuestPanel GetQuestPanel()
        {
            return uiInventory.questPanel;
        }

        public Loading GetLoadScreen()
        {
            return loading;
        }

        public void SetPointTarget(Transform target)
        {
            uiScreenHud.uiTarget.SetTarget(target);
        }
        
        public void SetPointTargetComplete()
        {
            uiScreenHud.uiTarget.Done();
        }

        public TutorialFinger GetTutorialFinger()
        {
            return uiScreenHud.tutorialFinger;
        }
        
        public void HidePointTarget()
        {
            uiScreenHud.uiTarget.Hide();
        }

    }
}