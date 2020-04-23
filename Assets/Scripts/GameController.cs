using GameCamera;
using Managers;
using UnityEngine;


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
    [SerializeField]
    private GameObject cameraPrefab;
    [HideInInspector]
    public PlayerManager playerManager; 
    [HideInInspector]
    public UIManager uiManager;
    [HideInInspector] 
    public SceneController sceneController;
    [HideInInspector] 
    public Camera camera;

    public AIActorsManager actorsManager;
    protected int spawnPointId = 0;
    
    private void Start()
    {
        playerManager = GetComponentInChildren<PlayerManager>();
        uiManager = GetComponent<UIManager>();
        actorsManager = new AIActorsManager();
        sceneController = GetComponentInChildren<SceneController>();
        sceneController.OnSceneLoaded += StartGame;

        uiManager.Spawn(transform);
        uiManager.HideHud();
        sceneController.Init();
#if (UNITY_EDITOR)
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
            camera = Camera.main;
            return;
        }
        camera = Instantiate(cameraPrefab).GetComponent<Camera>();
    }
    
    void PrepareCamera(GameObject player)
    {
        CameraController cameraController = camera.GetComponent<CameraController>();
        cameraController.target = player.transform;
    }


    public void Pause(bool value)
    {
        Time.timeScale = (value) ? 0 : 1;
    }
}
