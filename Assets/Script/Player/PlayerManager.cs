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
    
    [SerializeField] private int cur_ShotgunBullet;
    public int Cur_ShotgunBullet => cur_ShotgunBullet;

    [SerializeField] private int pistolBullet;   
    public int PistolBullet => pistolBullet;
    [SerializeField] private int cur_PistolBullet;
    public int Cur_PistolBullet => cur_PistolBullet;

   [SerializeField] private int gunBullet;    
    public int GunBullet => gunBullet;  
    [SerializeField] private int cur_GunBullet;
    public int Cur_GunBullet => cur_GunBullet;

    [SerializeField] private Sprite playerAvatar;

    public Sprite PlayerAvatar => playerAvatar;
    

    public Vector2 GetDirFromMouseToPlayer()
    {
        Vector2 playerPos = playerController.getPos();
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = (mousePos - playerPos).normalized;
        return dir;
    }

    public void AddBullet(string bulletName, int amount)
    {
        switch(bulletName)
        {
            case "ShotGun_Bullet":
                shotgunBullet += amount;
                break;
            case "Pistol_Bullet":
                pistolBullet += amount;
                break;
            case "Gun_Bullet":
                gunBullet += amount;
                break;
        }
        
    }

    public void UpdateCurrentBullet(string GunName, int amount)
    {
        switch(GunName)
        {
            case "ShotGun":
                cur_ShotgunBullet = amount;
                break;
            case "Pistol":
                cur_PistolBullet = amount;
                break;
            case "Gun":
                cur_GunBullet = amount;
                break;
        }
    }
    public void UpdateTotalBullet(string GunName, int amount)
    {
        switch(GunName)
        {
            case "ShotGun":
                shotgunBullet = amount;
                break;
            case "Pistol":
                pistolBullet = amount;
                break;
            case "Gun":
                gunBullet = amount;
                break;
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
        health.SetMaxHp(stat.MaxHP, true);
    }
}
