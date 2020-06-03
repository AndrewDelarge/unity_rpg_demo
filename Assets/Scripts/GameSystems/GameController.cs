using Exceptions.Game.Player;
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
        public CameraController cameraController;
        [HideInInspector] 
        public WorldUiCanvas worldUiCanvas;
        

        public AIActorsManager actorsManager;
    
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
//            SpawnWorldUiCanvas();
            StartGame();
#endif
        }


        public void StartScene(int scene, int spawnId = 0)
        {
            sceneController.LoadScene(scene);
            Pause(false);
        }

        private void StartGame()
        {
            SpawnCamera();
            SpawnWorldUiCanvas();
            actorsManager = new AIActorsManager();
            playerManager.Init();
            
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
                playerManager.equipmentManager.Reequip();
                PrepareCamera(player);
            }
            
            uiManager.ShowHud();
            sceneController.InitTriggers();
            sceneController.ApplySceneSettings(this);
        }
    
    
    
        void SpawnCamera()
        {
            if (cameraController != null)
            {
                Debug.Log("Camera already exists");
                return;
            }
            
            cameraController = Instantiate(cameraPrefab).GetComponent<CameraController>();
            
            if (cameraController == null)
            {
                // Assigned camera dont have controller
                throw new CameraControllerNotFound();
            }
        }


        void SpawnWorldUiCanvas()
        {
            worldUiCanvas = Instantiate(worldUiCanvasGameObject).GetComponent<WorldUiCanvas>();
        }
        
        void PrepareCamera(GameObject player)
        {
            CameraController cameraController = this.cameraController.GetComponent<CameraController>();
            cameraController.target = player.transform;
        }

        
        public void Pause(bool value)
        {
            Time.timeScale = (value) ? 0 : 1;
        }
    }
}
