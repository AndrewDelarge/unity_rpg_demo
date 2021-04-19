using System;
using GameSystems;
using Managers.Scenes;
using UI;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneController : MonoBehaviour
    {
        public static class BaseScenes
        {
            [Serializable]
            public static class Indexes
            {
                public static int ROOT = 0;
                public static int MENU = 1;
                public static int GAME = 2;
            }
            
            public static class Titles
            {
                public static string ROOT = "_root";
                public static string MENU = "menu";
                public static string GAME = "game";
            }
        }
        [SerializeField]
        private LevelController levelController;
        
        [HideInInspector] 
        public WorldUiCanvas worldUiCanvas;
        public event Action OnSceneLoaded;
        public LevelController LevelController => levelController;

        private int currentLevel;
        private bool isLoading;
        

        private AsyncOperation sceneLoadingOperation;
        private SceneSettings sceneSettings;
        private Loading loadingScreen;

        public void Init()
        {
            loadingScreen = UIManager.Instance().GetLoadScreen();
            levelController.Init();
        }
        
        private void FixedUpdate()
        {
            if (isLoading)
                loadingScreen.SetProgress(sceneLoadingOperation.progress);
        }

        public void LoadScene(int scene)
        {
            isLoading = true;
            worldUiCanvas = null;
            loadingScreen.Show();
            sceneLoadingOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            sceneLoadingOperation.completed += OnLoaded;
        }
        
        public void ApplySceneSettings()
        {
            if (sceneSettings != null)
                sceneSettings.Apply();
        }

        public void NextLevel()
        {
            currentLevel++;

            GameObject nextLevel = sceneSettings.GetLevel(currentLevel);
            
            if (nextLevel != null)
                levelController.LoadLevel(nextLevel);
        }

        public void StartCurrentEditorScene()
        {
            GameManager.Instance().sceneController.SpawnWorldUiCanvas();
            PrepareScene();
        }
        
        public int GetStartSpawnPoint()
        {
            return levelController.GetStartSpawnId();
        }
        
        public AIActorsManager GetActorsManager()
        {
            return levelController.actorsManager;
        }

        public GameObject FindSpawnPoint(int pointId)
        {
            return levelController.FindSpawnPoint(pointId);
        }

        private void OnLoaded(AsyncOperation operation)
        {
            PrepareScene();
        }
        
        public void SpawnWorldUiCanvas()
        {
            if (worldUiCanvas == null)
                worldUiCanvas = Instantiate(GameManager.Instance().worldUiCanvasGameObject).GetComponent<WorldUiCanvas>();
        }

        private void PrepareScene()
        {
            sceneSettings = FindObjectOfType<SceneSettings>();
#if (UNITY_EDITOR)
            SpawnWorldUiCanvas();
            levelController.StartCurrentEditorLevel();
            if (sceneSettings != null && sceneSettings.levels != null)
            {
                currentLevel = sceneSettings.startLevel;
                levelController.LoadLevel(sceneSettings.GetLevel(currentLevel));
            }
#else
            if (sceneSettings != null) 
            {
                SpawnWorldUiCanvas();
                currentLevel = sceneSettings.startLevel;
                levelController.LoadLevel(sceneSettings.GetLevel(currentLevel));
            }
#endif
            loadingScreen.Hide();
            isLoading = false;
            OnSceneLoaded?.Invoke();
            OnSceneLoaded = null;
        }
        
        
    }
}