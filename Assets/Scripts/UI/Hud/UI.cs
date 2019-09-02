using UI.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class UI : MonoBehaviour
    {
        public ActionBar actionBar;
        public HealthBar healthBar;
        public UpperPanel upperPanel;
        public QuestPanel questPanel;
        public GameObject endScreen;
        public Text FPSTracker;

        private float deltaTime;
        
        void Update () {

            if (FPSTracker != null)
            {
                deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
                float fps = 1.0f / deltaTime;
                FPSTracker.text = Mathf.Ceil (fps).ToString();
            }
        }

        public void ShowEndScreen()
        {
            endScreen.SetActive(true);
        }
    }
}
