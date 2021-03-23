using System;
using System.Runtime.Serialization;
using Exceptions.Game.Player;
using Gameplay;
using Gameplay.Player;
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
        
        
        [HideInInspector] 
        public WorldUiCanvas worldUiCanvas;
        [HideInInspector] 
        public CameraController cameraController;
        public event Action OnSceneLoaded;
        
        public LevelController LevelController => levelController;

        
        protected Scene currentScene;

        private int currentLevel;
        private bool isLoading;
        private LevelController levelController;
        private AsyncOperation sceneLoadingOperation;
        private SceneSettings sceneSettings;
        private Loading loadingScreen;

        public void Init()
        {
            loadingScreen = GameController.instance.uiManager.GetLoadScreen();
            levelController = gameObject.AddComponent<LevelController>();
            levelController.Init();

#if (! UNITY_EDITOR)
            SceneManager.LoadScene(BaseScenes.Indexes.ROOT, new LoadSceneParameters(LoadSceneMode.Single));
#endif
            
#if (UNITY_EDITOR)
            currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == BaseScenes.Titles.ROOT)
            {
                SceneManager.LoadScene(BaseScenes.Indexes.MENU, new LoadSceneParameters(LoadSceneMode.Single));
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
        
        public void ApplySceneSettings(GameController controller)
        {
            if (sceneSettings != null)
            {
                sceneSettings.Apply(controller);
            }
        }

        public void NextLevel()
        {
            currentLevel++;

            GameObject nextLevel = sceneSettings.GetLevel(currentLevel);
            if (nextLevel != null)
            {
                levelController.LoadLevel(nextLevel);
                return;
            }
        }

        public void StartCurrentEditorScene()
        {
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
        

        public bool SceneIsPlayable()
        {
            return sceneSettings != null;
        }
        
        public bool IsNeedToSpawnPlayer()
        {
            return sceneSettings.spawnPlayer;
        }

        
        
        // When Scene Is loaded
        private void OnLoaded(AsyncOperation operation)
        {
            PrepareScene();
        }
        
        private void SpawnWorldUiCanvas()
        {
            worldUiCanvas = Instantiate(GameController.instance.worldUiCanvasGameObject).GetComponent<WorldUiCanvas>();
        }

        private void SpawnCamera()
        {
            if (cameraController != null)
            {
                Debug.Log("Camera already exists");
                return;
            }
            
            cameraController = Instantiate(GameController.instance.cameraPrefab).GetComponent<CameraController>();
            cameraController.Init();
            if (cameraController == null)
            {
                // Assigned camera dont have controller
                throw new CameraControllerNotFound();
            }
        }
        
        private void PrepareScene()
        {
            SpawnCamera();
            SpawnWorldUiCanvas();
            sceneSettings = FindObjectOfType<SceneSettings>();
#if (UNITY_EDITOR)
            levelController.StartCurrentEditorLevel();
            if (sceneSettings != null && sceneSettings.levels != null)
            {
                currentLevel = sceneSettings.startLevel;
                levelController.LoadLevel(sceneSettings.GetLevel(currentLevel));
            }
#else
            currentLevel = sceneSettings.startLevel;
            levelController.LoadLevel(sceneSettings.GetLevel(currentLevel));
#endif
            loadingScreen.Hide();
            isLoading = false;
            OnSceneLoaded?.Invoke();
        }
        
        
    }
}