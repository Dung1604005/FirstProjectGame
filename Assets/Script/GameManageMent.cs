using System;

using Cinemachine;
using UnityEngine;


[Serializable]
public enum DirType
{
    LEFT, RIGHT, UP, DOWN 
}
public enum GameState
{
    Continue, Pause
}

public class GameManageMent : MonoBehaviour
{
    public static GameManageMent Instance { get; private set; }
    private  GameState gameState;
    public GameState GameState => gameState;

    public WorldManager _WorldManager {get; private set;}


    [SerializeField] private EffectController effectController;
    public EffectController EffectController => effectController;
    [SerializeField] private ItemDataBase itemDataBase;
    
    public ItemDataBase ItemDataBase => itemDataBase;
    [SerializeField] private EnemyDataBase enemyDataBase;
    public EnemyDataBase EnemyDataBase => enemyDataBase;
    

    private PlayerManager playerManager;

    public PlayerManager PlayerManager => playerManager;

    private PoolManager poolManager;
    public PoolManager PoolManager => poolManager;

    [SerializeField] private GridManagement gridManagement;

    public GridManagement GridManagement => gridManagement;

    [Header("Camera")]

    [SerializeField] private GameObject cameraContainer;

    [SerializeField] private CinemachineConfiner2D cinemachineConfiner2D;

    

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    public CinemachineVirtualCamera CinemachineVirtualCamera => cinemachineVirtualCamera;

    [SerializeField] private float heightCamera;

    public float HeightCamera   => heightCamera;

    [SerializeField] private float widthCamera;

    public float WidthCamera => widthCamera;


    [Header("Menu")]

    [SerializeField] private MenuController menuController;
    
      

    [Header("BuildMode")] 
    
    [SerializeField] private BuildManager buildManager;
    public BuildManager BuildManager => buildManager;

    [Header("Inventory And Equipment")]
    [SerializeField] private InventoryAndEquipmentManager inventoryAndEquipmentManager;
    public InventoryAndEquipmentManager InventoryAndEquipmentManager => inventoryAndEquipmentManager;

    [Header("DropSystem")]
    private DropSystem dropSystem;
    public DropSystem DropSystem => dropSystem;

    [Header("Quest System")]
    private QuestManager questManager;
    public QuestManager QuestManager => questManager;

    [Header("Time")]

    private TimeManager timeManager;
    public TimeManager TimeManager => timeManager;

    [Header("Environment")]
    private EnviromentManager enviromentManager;

    public EnviromentManager EnviromentManager => enviromentManager;

    [Header("Cursor")]
    [SerializeField] private Texture2D iconMouse;
    public Texture2D IconMouse => iconMouse;
    [SerializeField] private Texture2D iconMouseInteract;
    public Texture2D IconMouseInteract => iconMouseInteract;
    public Vector2 hotspot = Vector2.zero;  
    public CursorMode cursorMode = CursorMode.Auto;

    private bool interacting = false;
    public bool Interacting => interacting;

    private NPC npcInteracting = null;
    public NPC NpcInteracting => npcInteracting;

    Vector2[] arrayDir = {new Vector2(0, -1),new Vector2(-1, 0), new Vector2(1, 0),  new Vector2(0, 1)};
    
    public void PauseGame()
    {

        gameState = GameState.Pause;

    }
    public void Continue()
    {
        if (!menuController.gameObject.activeInHierarchy && !UIManageMent.Instance.ShopSystem.gameObject.activeInHierarchy)
        {
            gameState = GameState.Continue;
        }
        
        
       

    }
    public void SetGridManageMent(GridManagement _gridManagement){
        gridManagement = _gridManagement;
    }
    public void StartGame(){
        foreach(Transform child in transform){
            if(
             child.gameObject.name == "LightGlobal" ){
                child.gameObject.SetActive(true);
            }
        }
        cameraContainer.SetActive(true);
        playerManager.enabled = true;
        poolManager.enabled = true;
        dropSystem.enabled  = true;
        
        effectController.enabled = true;
        timeManager.enabled =true;
        enviromentManager.OutdoorEnvironment.WeatherSystem.enabled = true;
        enviromentManager.OutdoorEnvironment.enabled = true;
        enviromentManager.IndoorEnvironment.enabled = true;
        enviromentManager.enabled = true;
        
        gameState = GameState.Continue;
        


    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        buildManager = GetComponent<BuildManager>();
        playerManager = GetComponent<PlayerManager>();
        playerManager.enabled = false;
        inventoryAndEquipmentManager = GetComponent<InventoryAndEquipmentManager>();
        inventoryAndEquipmentManager.enabled = false;
        poolManager = GetComponent<PoolManager>();
        poolManager.enabled = false;
        dropSystem = GetComponent<DropSystem>();
        dropSystem.enabled = false;
        questManager = GetComponent<QuestManager>();
        
        effectController = GetComponent<EffectController>();
        effectController.enabled = false;
        timeManager = GetComponent<TimeManager>();
        timeManager.enabled =false;
        enviromentManager = GetComponent<EnviromentManager>();
        enviromentManager.OutdoorEnvironment.WeatherSystem.enabled = false;
        enviromentManager.OutdoorEnvironment.enabled = false;
        enviromentManager.IndoorEnvironment.enabled = false;
        enviromentManager.enabled = false;
        _WorldManager = new WorldManager();
        gameState = GameState.Pause;
        Application.targetFrameRate = 120;
        Cursor.SetCursor(iconMouse, hotspot, cursorMode);
        //Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        Camera mainCamera = Camera.main;
        heightCamera = 2f *mainCamera.orthographicSize;
        widthCamera = heightCamera* mainCamera.aspect;

        foreach(Transform child in transform){
            if( child.gameObject.name == "LightGlobal" ){
                child.gameObject.SetActive(false);
            }
        }
        
    }
    void Start()
    {
       
        
        
    }

    

    public void SetBoundMap(PolygonCollider2D polygonCollider2D)
    {
        
        cinemachineConfiner2D.m_BoundingShape2D = polygonCollider2D;
    }
    public void SetCurSorInteract()
    {
        interacting = true;
        Cursor.SetCursor(iconMouseInteract, hotspot, cursorMode);
    }
    public void SetCurSorNormal()
    {
        interacting = false;
        Cursor.SetCursor(iconMouse, hotspot, cursorMode);
    }
    public void SetNpcInteracting(NPC other)
    {
        npcInteracting = other;
    }
    public void ControlMenu()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (menuController.gameObject.activeInHierarchy)
            {
                menuController.SwitchTab();

            }
        }
        //Mo Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (!menuController.gameObject.activeInHierarchy)
            {
                menuController.OpenMenu();
                PauseGame();
            }
            else
            {

                menuController.CloseMenu();
                Continue();
            }

        }
    }
    
    
        public DirType CalculateDirType(float dirX, float dirY)
    {
        
        Vector2 a = new Vector2(dirX, dirY).normalized;
        
        
        float distanceDir = float.MaxValue;
        int animDir = 0;
        for(int i = 0; i < arrayDir.Length; i++)
        {
            float distance = (arrayDir[i] - a).sqrMagnitude;
            if(distance < distanceDir)
            {
                distanceDir = distance;
                animDir = i;
            }
        }
        if (animDir == 0)
        {
            return DirType.DOWN;
        }
        else if(animDir == 1)
        {
            return DirType.LEFT;
        }
        else if(animDir == 2)
        {
            return DirType.RIGHT;
        }
        else 
        {
            return DirType.UP;
        }
    }
    void Update()
    {
        //Mo Menu
        ControlMenu();

        
      
        
       

    }

}
