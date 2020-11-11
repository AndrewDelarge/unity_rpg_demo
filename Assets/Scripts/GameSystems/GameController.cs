using Managers;
using UnityEngine;

namespace GameSystems
{
    [RequireComponent(typeof(PlayerManager))]
    public class GameController : MonoBehaviour
    {
        #region Singleton
        public static GameController instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
        
            instance = this;
            DontDestroyOnLoad(this);
        }
    
        #endregion
        public GameObject cameraPrefab;
        public GameObject worldUiCanvasGameObject;
        public GameObject UIPrefab;

        
        [HideInInspector]
        public PlayerManager playerManager; 
        [HideInInspector]
        public UIManager uiManager;
        [HideInInspector] 
        public SceneController sceneController;

        
    
        private void Start()
        {
            playerManager = GetComponentInChildren<PlayerManager>();
            uiManager = new UIManager();
            sceneController = GetComponentInChildren<SceneController>();
            sceneController.OnSceneLoaded += StartGame;

            uiManager.Init(transform);
            uiManager.HideHud();
            sceneController.Init();
            playerManager.Init();

            
#if (UNITY_EDITOR)
            sceneController.StartCurrentEditorScene();
#endif
        }

        private void StartGame()
        {
            if (! sceneController.SceneIsPlayable())
            {
                Debug.Log("Loaded not playable scene");
                uiManager.HideHud();
                return;
            }

            if (sceneController.IsNeedToSpawnPlayer())
            {
                GameObject player = playerManager.SpawnPlayer(sceneController.GetStartSpawnPoint());
                uiManager.SetPlayer(player);
                playerManager.InitPlayer();
                PrepareCamera(player);
            }
            
            uiManager.ShowHud();
            sceneController.ApplySceneSettings(this);
        }
    
        private void PrepareCamera(GameObject player)
        {
            CameraController cameraController = GetCameraController();
            cameraController.target = player.transform;
        }

        public void StartScene(int scene, int spawnId = 0)
        {
            sceneController.LoadScene(scene);
            Pause(false);
        }
        
        
        public CameraController GetCameraController()
        {
            return sceneController.cameraController;
        }
        
        public void Pause(bool value)
        {
            Time.timeScale = (value) ? 0 : 1;
        }
    }
}
