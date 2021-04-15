using GameSystems;
using UI.MainMenu;
using UnityEngine;

namespace UI.Hud
{
    public class PauseMenu : MonoBehaviour
    {
        public Loading loading;

        public void OnExitButton()
        {
            // TODO hardcode
            Application.Quit();
            gameObject.SetActive(false);
        }
        
        public void Show()
        {
            GameManager.Instance().Pause(true);

            gameObject.SetActive(! gameObject.activeSelf);
        }
        
        public void OnResumeButton()
        {
            GameManager.Instance().Pause(false);
            this.gameObject.SetActive(false);
        }
    }
}
