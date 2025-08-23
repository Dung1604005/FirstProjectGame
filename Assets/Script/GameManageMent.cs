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

    
    [Header("Inventory")]

    [SerializeField] private InventoryUI inventoryUI;
    
    public void PauseGame()
    {
        
        gameState = GameState.Pause;
        
    }
    public void Continue()
    {
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

        
    }
    void Start()
    {
        inventoryUI.TurnOff();
        UIManageMent.Instance.ExpStatSystemUI.TurnOff();
        gameState = GameState.Continue;
       

    }
    void Update()
    {
        // Mo inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (gameState == GameState.Continue)
            {
                inventoryUI.TurnOn();
                PauseGame();
            }
            else if (gameState == GameState.Pause)
            {
                Continue();
                inventoryUI.TurnOff();
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (gameState == GameState.Continue)
            {
                PauseGame();
                UIManageMent.Instance.ExpStatSystemUI.TurnOn();
            }
            else if (gameState == GameState.Pause)
            {
                Continue();
                UIManageMent.Instance.ExpStatSystemUI.TurnOff();
            }
        }

    }
}
