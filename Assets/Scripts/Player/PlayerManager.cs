using NPC;
using Quests;
using UI.Hud;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        public UI.Hud.UI UI;
        [HideInInspector]
        public PlayerActor player;
        
        public GameObject playerGameObject;
        
        private Quest currentQuest;
        public UnityEvent onGameStart;
        
        #region Singleton
        public static PlayerManager instance;

        private void Awake()
        {
            instance = this;

            player = playerGameObject.GetComponent<PlayerActor>();
        }
        #endregion


        private void Start()
        {
            if (onGameStart != null)
            {
                onGameStart.Invoke();
            }
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        public void Pause(bool value)
        {
            Time.timeScale = (value) ? 0 : 1;
        }

        public bool StartQuest(Quest quest)
        {
            if (currentQuest != null)
            {
                return false;
            }

            currentQuest = quest;
            return true;
        }


        public void StopQuest(Quest quest)
        {
            currentQuest = null;
        }
        
    }
}
