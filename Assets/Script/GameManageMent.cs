using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

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

    [SerializeField] private CinemachineConfiner2D cinemachineConfiner2D;

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
        inventoryAndEquipmentManager = GetComponent<InventoryAndEquipmentManager>();
        poolManager = GetComponent<PoolManager>();
        dropSystem = GetComponent<DropSystem>();
        questManager = GetComponent<QuestManager>();
        effectController = GetComponent<EffectController>();
        timeManager = GetComponent<TimeManager>();
        enviromentManager = GetComponent<EnviromentManager>();
        gameState = GameState.Continue;
        Application.targetFrameRate = 120;
        Cursor.SetCursor(iconMouse, hotspot, cursorMode);
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);
        Camera mainCamera = Camera.main;
        heightCamera = 2f *mainCamera.orthographicSize;
        widthCamera = heightCamera* mainCamera.aspect;
        
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
    
    
    public void OpenShop()
    {
         if (Input.GetKeyDown(KeyCode.Q))
        {
            if (UIManageMent.Instance.ShopSystem.gameObject.activeInHierarchy)
            {
                UIManageMent.Instance.ShopSystem.TurnOff();
                Continue();
            }
            else
            {
                PauseGame();
                UIManageMent.Instance.ShopSystem.TurnOn();
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

        OpenShop();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (UIManageMent.Instance.InventoryUI.Inven.TryAdd(itemDataBase.ItemDatas[0], 1))
            {
                UIManageMent.Instance.InventoryUI.Inven.Add(itemDataBase.ItemDatas[0], 1);
            }

        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (UIManageMent.Instance.InventoryUI.Inven.TryAdd(itemDataBase.ItemDatas[1], 1))
            {
                UIManageMent.Instance.InventoryUI.Inven.Add(itemDataBase.ItemDatas[1], 1);
            }

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (UIManageMent.Instance.InventoryUI.Inven.TryAdd(itemDataBase.ItemDatas[2], 1))
            {
                UIManageMent.Instance.InventoryUI.Inven.Add(itemDataBase.ItemDatas[2], 1);
            }

        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (UIManageMent.Instance.InventoryUI.Inven.TryAdd(itemDataBase.ItemDatas[3], 1))
            {
                UIManageMent.Instance.InventoryUI.Inven.Add(itemDataBase.ItemDatas[3], 1);
            }

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (UIManageMent.Instance.InventoryUI.Inven.TryAdd(itemDataBase.ItemDatas[6], 1))
            {
                UIManageMent.Instance.InventoryUI.Inven.Add(itemDataBase.ItemDatas[6], 1);
            }

        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (UIManageMent.Instance.InventoryUI.Inven.TryAdd(itemDataBase.ItemDatas[7], 1))
            {
                UIManageMent.Instance.InventoryUI.Inven.Add(itemDataBase.ItemDatas[7], 1);
            }

        }
      
        
       

    }

}
