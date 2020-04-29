using System;
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
            OnSceneLoaded?.Invoke();
        }


        public GameObject GetSpawnPoint(int id = 1)
        {
            GameObject[] gos = currentScene.GetRootGameObjects();

            if (gos.Length == 0)
            {
                return null;
            }

            return null;
        }
    }
}