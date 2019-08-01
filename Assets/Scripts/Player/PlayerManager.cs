using NPC;
using UI.Hud;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        public UI.Hud.UI UI;

        #region Singleton
        public static PlayerManager instance;

        private void Awake()
        {
            instance = this;
        }
        #endregion
        

        public PlayerActor player;


        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        public void Pause(bool value)
        {
            Time.timeScale = (value) ? 0 : 1;
        }

        
    }
}
