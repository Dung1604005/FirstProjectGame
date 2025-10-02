using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;


public enum GameState
{
    Continue, Pause
}
public class GameManageMent : MonoBehaviour
{
    public static GameManageMent Instance { get; private set; }
    private  GameState gameState;
    public GameState GameState => gameState;
    [SerializeField] private ItemDataBase itemDataBase;
    public ItemDataBase ItemDataBase => itemDataBase;

    private PlayerManager playerManager;

    public PlayerManager PlayerManager => playerManager;

    private PoolManager poolManager;
    public PoolManager PoolManager => poolManager;
    
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
        
    }
    void Start()
    {

        gameState = GameState.Continue;
        Application.targetFrameRate = 120;
        
        

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
            if (UIManageMent.Instance.InventoryUI.Inven.TryAdd(itemDataBase.ItemDatas[4], 1))
            {
                UIManageMent.Instance.InventoryUI.Inven.Add(itemDataBase.ItemDatas[4], 1);
            }

        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (UIManageMent.Instance.InventoryUI.Inven.TryAdd(itemDataBase.ItemDatas[5], 1))
            {
                UIManageMent.Instance.InventoryUI.Inven.Add(itemDataBase.ItemDatas[5], 1);
            }

        }
      
        
       

    }
}
