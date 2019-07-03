using NPC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        public PlayerActor player;

        #region Singleton
        public static PlayerManager instance;

        private void Awake()
        {
            instance = this;
        }
        #endregion


        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
