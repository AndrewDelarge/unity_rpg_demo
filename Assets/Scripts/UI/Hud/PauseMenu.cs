using Player;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Hud
{
    public class PauseMenu : MonoBehaviour
    {
        
        public Loading loading;

        
        private void Awake()
        {
            Debug.Log(SceneManager.sceneCount);
        }

        public void OnExitButton()
        {
            // TODO hardcode
            loading.ShowAndLoad(0);
            PlayerManager.instance.Pause(false);
        }
        
        
        public void Show()
        {
            PlayerManager.instance.Pause(true);

            gameObject.SetActive(! gameObject.activeSelf);
        }
        
        public void OnResumeButton()
        {
            PlayerManager.instance.Pause(false);
            this.gameObject.SetActive(false);
        }
    }
}
