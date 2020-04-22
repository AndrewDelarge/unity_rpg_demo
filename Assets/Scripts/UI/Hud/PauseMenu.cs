using Player;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Hud
{
    public class PauseMenu : MonoBehaviour
    {
        public Loading loading;

        public void OnExitButton()
        {
            // TODO hardcode
            GameController.instance.StartScene(1);
            gameObject.SetActive(false);
        }
        
        public void Show()
        {
            GameController.instance.Pause(true);

            gameObject.SetActive(! gameObject.activeSelf);
        }
        
        public void OnResumeButton()
        {
            GameController.instance.Pause(false);
            this.gameObject.SetActive(false);
        }
    }
}
