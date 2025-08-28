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
    
    [Header("Inventory")]

    [SerializeField] private InventoryUI inventoryUI;
    
    public void PauseGame()
    {
        
        gameState = GameState.Pause;
        
    }
    public void Continue()
    {
        if (!inventoryUI.gameObject.activeInHierarchy && !  UIManageMent.Instance.ExpStatSystemUI.gameObject.activeInHierarchy && !UIManageMent.Instance.ShopSystem.gameObject.activeInHierarchy)
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

        
    }
    void Start()
    {
        
        gameState = GameState.Continue;
       

    }
    public void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.gameObject.activeInHierarchy)
            {
                inventoryUI.TurnOff();
                Continue();
            }
            else
            {
                inventoryUI.TurnOn();
                PauseGame();
            }
            
        }
    }
    public void OpenStatMenu()
    {
         if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (UIManageMent.Instance.ExpStatSystemUI.gameObject.activeInHierarchy)
            {
                UIManageMent.Instance.ExpStatSystemUI.TurnOff();
                Continue();
            }
            else
            {
                 PauseGame();
                UIManageMent.Instance.ExpStatSystemUI.TurnOn();
            }
           
        }
    }
    
    public void OpenShop()
    {
         if (Input.GetKeyDown(KeyCode.E))
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
        // Mo inventory
        OpenInventory();
        // Mo bang stat
        OpenStatMenu();

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIManageMent.Instance.ShopSystem.Refresh();
        }
        
       

    }
}
