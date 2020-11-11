using System;
using GameSystems;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    [Serializable]
    public struct UITemplates
    {
        public GameObject damageTextFeed;
    }
    
    public class UI : MonoBehaviour
    {
        public ActionBar actionBar;
        public HealthBar healthBar;
        public UpperPanel upperPanel;
        public QuestPanel questPanel;
        public Inventory.Inventory inventory;
        public GameObject endScreen;
        public Text FPSTracker;
        public Loading loadingScreen;
        public UITarget uiTarget;
        public TutorialFinger tutorialFinger;
        public DamageFeed damageFeed;
        
        public UITemplates uiTemplates;
        
        private float deltaTime;

        public void Init()
        {
            healthBar.Init();
            inventory.Init();
            actionBar.Init();
            uiTarget.Init();
            damageFeed.Init(uiTemplates.damageTextFeed);
            tutorialFinger.Init();
            upperPanel.onInventoryButtonClick = null;
            upperPanel.onInventoryButtonClick += inventory.ToggleInventory;
        }
        
        void Update () {

            if (FPSTracker != null)
            {
                deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
                float fps = 1.0f / deltaTime;
                FPSTracker.text = Mathf.Ceil (fps).ToString();
            }
        }

        public void HideHud()
        {
            actionBar.gameObject.SetActive(false);
            actionBar.joystick.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(false);
            upperPanel.gameObject.SetActive(false);
            questPanel.gameObject.SetActive(false);
            inventory.itemReceived.gameObject.SetActive(false);
        }
        
        
        public void ShowHud()
        {
            actionBar.gameObject.SetActive(true);
            actionBar.joystick.gameObject.SetActive(true);
            healthBar.gameObject.SetActive(true);
            upperPanel.gameObject.SetActive(true);
//            questPanel.gameObject.SetActive(true);
        }
        
        public void ShowEndScreen()
        {
            endScreen.SetActive(true);
        }
    }
}
