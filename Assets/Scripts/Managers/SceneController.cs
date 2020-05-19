using System;
using Gameplay;
using Gameplay.Player;
using GameSystems;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneController : MonoBehaviour
    {
        private int MAIN_MENU_SCENE_INDEX = 1;
        
        protected Scene currentScene;
        private AsyncOperation sceneLoadingOperation;
        private Loading loadingScreen;
        private bool isLoading;

        private TriggersManager triggersManager;
        private SceneSettings sceneSettings;
        private SpawnPoint[] spawnPoints;

        
        public event Action OnSceneLoaded;
        
        
        public void Init()
        {
            loadingScreen = GameController.instance.uiManager.GetLoadScreen();
#if (! UNITY_EDITOR)
            SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX, new LoadSceneParameters(LoadSceneMode.Single));
#endif
            
#if (UNITY_EDITOR)
            currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "MainScene")
            {
                SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX, new LoadSceneParameters(LoadSceneMode.Single));
            }

            spawnPoints = FindObjectsOfType<SpawnPoint>();
            sceneSettings = FindObjectOfType<SceneSettings>();
            triggersManager = new TriggersManager(FindObjectsOfType<Trigger>());
#endif

        }


        private void FixedUpdate()
        {
            if (isLoading)
            {
                loadingScreen.SetProgress(sceneLoadingOperation.progress);
            }
        }

        public void LoadScene(int scene)
        {
            isLoading = true;
            loadingScreen.Show();
            sceneLoadingOperation = SceneManager.LoadSceneAsync(scene);
            sceneLoadingOperation.completed += OnLoaded;
        }


        private void OnLoaded(AsyncOperation operation)
        {
            loadingScreen.Hide();
            isLoading = false;
            spawnPoints = FindObjectsOfType<SpawnPoint>();
            sceneSettings = FindObjectOfType<SceneSettings>();
            triggersManager = new TriggersManager(FindObjectsOfType<Trigger>());
            OnSceneLoaded?.Invoke();
        }
        

        public void InitTriggers()
        {
            triggersManager?.Init();
        }


        public void ApplySceneSettings(GameController controller)
        {
            if (sceneSettings != null)
            {
                sceneSettings.Apply(controller);
            }
        }
        
        public GameObject FindSpawnPoint(int spawnPointId)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (spawnPoints[i].id == spawnPointId)
                {
                    return spawnPoints[i].gameObject;
                }
            }

            return null;
        }


        public bool SceneIsPlayable()
        {
            return sceneSettings != null;
        }
    }
}