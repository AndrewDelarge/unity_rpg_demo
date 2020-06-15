using System;
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
        [HideInInspector] 
        public WorldUiCanvas worldUiCanvas;
        [HideInInspector] 
        public CameraController cameraController;
        public event Action OnSceneLoaded;

        
        protected Scene currentScene;

        
        private int MAIN_MENU_SCENE_INDEX = 1;

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