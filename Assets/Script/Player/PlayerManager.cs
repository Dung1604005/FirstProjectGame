using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private PlayerController playerController;
    public PlayerController PlayerController => playerController;

    [Header("PLAYER STAT")]
    [SerializeField] private ExpSystem expSystem;
    public ExpSystem ExpSystem => expSystem;

    [SerializeField] private GoldPlayer gold;
    public GoldPlayer Gold => gold;

    [SerializeField] private StatPlayer stat;
    public StatPlayer Stat => stat;

    [SerializeField] private Health health;
    public Health Health => health;

    [Header("OTHER")]
    [SerializeField] private int shotgunBullet;
    public int ShotgunBullet => shotgunBullet;  

    [SerializeField]private int pistolBullet;   
    public int PistolBullet => pistolBullet;

   [SerializeField] private int gunBullet;    
    public int GunBullet => gunBullet;  

    public Vector2 GetDirFromMouseToPlayer()
    {
        Vector2 playerPos = playerController.getPos();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = (mousePos - playerPos).normalized;
        return dir;
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
        health.SetMaxHp(stat.MaxHP, true);
    }
}
