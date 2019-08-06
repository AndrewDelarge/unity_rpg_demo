using Player;
using UnityEngine;

namespace UI.Hud
{
    public class PauseMenu : MonoBehaviour
    {
        
        

        public void OnExitButton()
        {
            Application.Quit();
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
