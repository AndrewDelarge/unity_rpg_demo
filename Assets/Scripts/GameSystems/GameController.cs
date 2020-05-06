using Managers;
using UI;
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
            SpawnCamera();
        }
    
        #endregion
        public GameObject cameraPrefab;
        public GameObject worldUiCanvasGameObject;
        [HideInInspector]
        public PlayerManager playerManager; 
        [HideInInspector]
        public UIManager uiManager;
        [HideInInspector] 
        public SceneController sceneController;
        [HideInInspector] 
        public CameraController mainCamera;
        [HideInInspector] 
        public WorldUiCanvas worldUiCanvas;
        

        public AIActorsManager actorsManager;
        protected int spawnPointId = 0;
    
        private void Start()
        {
            playerManager = GetComponentInChildren<PlayerManager>();
            uiManager = GetComponent<UIManager>();
            sceneController = GetComponentInChildren<SceneController>();
            sceneController.OnSceneLoaded += StartGame;

            uiManager.Spawn(transform);
            uiManager.HideHud();
            sceneController.Init();
        
#if (UNITY_EDITOR)
            SpawnWorldUiCanvas();
            StartGame();
#endif
        
        }


        public void StartScene(int scene, int spawnId = 0)
        {
            spawnPointId = spawnId;
            sceneController.LoadScene(scene);
            Pause(false);
        }

        private void StartGame()
        {
            SpawnCamera();
            SpawnWorldUiCanvas();
            actorsManager = new AIActorsManager();
            playerManager.Init();
            GameObject player = playerManager.SpawnPlayer(spawnPointId);

            if (player == null)
            {
                uiManager.HideHud();
                return;
            }
        
            uiManager.SetPlayer(player);
            playerManager.InitPlayer();
            playerManager.equipmentManager.Reequip();
            PrepareCamera(player);
            uiManager.ShowHud();
        }
    
    
    
        void SpawnCamera()
        {
            if (Camera.main != null)
            {
                mainCamera = Camera.main.GetComponent<CameraController>();
                return;
            }
            mainCamera = Instantiate(cameraPrefab).GetComponent<CameraController>();
        }


        void SpawnWorldUiCanvas()
        {
            worldUiCanvas = Instantiate(worldUiCanvasGameObject).GetComponent<WorldUiCanvas>();
        }
        
        void PrepareCamera(GameObject player)
        {
            CameraController cameraController = mainCamera.GetComponent<CameraController>();
            cameraController.target = player.transform;
        }


        public void Pause(bool value)
        {
            Time.timeScale = (value) ? 0 : 1;
        }
    }
}
