using CoreUtils;
using GameSystems.GameModes;
using Managers;
using UnityEngine;

namespace GameSystems
{
    public class GameManager : SingletonDD<GameManager>
    {
        public GameObject worldUiCanvasGameObject;

        public SceneController sceneController;


        [Header("Develop")]
        [SerializeField] private bool startHere = false;
        
        private void Awake()
        {
            Application.targetFrameRate = 60;

            Init();
        }

        private void Init()
        {
            sceneController.Init();
            UIManager.Instance().Init();
            PlayerManager.Instance().Init();

#if (UNITY_EDITOR)
            if (startHere)
            {
                sceneController.StartCurrentEditorScene();
            
                GameModeManager.Instance().SetGameState(GameModeManager.GameModeState.DEVELOPMENT_SCENE);
                return;
            }
#endif
            GameModeManager.Instance().SetGameState(GameModeManager.GameModeState.TO_MAIN_MENU_FROM_LOADER);
        }

        public void LoadMainMenu()
        {
            sceneController.LoadScene(SceneController.BaseScenes.Indexes.MENU);
            sceneController.OnSceneLoaded += () =>
                GameModeManager.Instance().SetGameState(GameModeManager.GameModeState.IN_MAIN_MENU);
        }

        public void StartScene(int scene, int spawnId = 0)
        {
            sceneController.LoadScene(scene);
            Pause(false);
        }
        
        public void Pause(bool value)
        {
            Time.timeScale = (value) ? 0 : 1;
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
