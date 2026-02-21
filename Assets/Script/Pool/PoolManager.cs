using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("LootPool")]
    [SerializeField] private LootItem lootItemPrefab;

    [SerializeField] private Transform lootRoot;
    [SerializeField] private int lootPoolSize;
    [SerializeField] private int lootPoolMaxSize;

    private ObjectPool<LootItem> lootPool;

    public ObjectPool<LootItem> LootPool => lootPool;

    [Header("FloatingTextPool")]
    [SerializeField] private FloatingText floatingTextPrefab;

    [SerializeField] private Transform floatingTextRoot;
    [SerializeField] private int floatingTextPoolSize;
    [SerializeField] private int floatingTextPoolMaxSize;

    private ObjectPool<FloatingText> floatingTextPool;

    public ObjectPool<FloatingText> FloatingTextPool => floatingTextPool;

    [Header("BulletPool")]

    [SerializeField] private List<BulletController> bulletPrefabList;

    [SerializeField] private Transform bulletRoot;

    [SerializeField] private int bulletPoolSize;

    [SerializeField] private int bulletPoolMaxSize;

    private List<ObjectPool<BulletController>> bulletPoolsList;
    public List<ObjectPool<BulletController>> BulletPoolsList => bulletPoolsList;

    [Header("Skill3GhostKingPool")]

    [SerializeField] private SkillBoss skill3GhostKingPrefab;

    [SerializeField] private Transform skill3GhostKingRoot;

    [SerializeField] private int skill3GhostKingPoolSize;

    [SerializeField] private int skill3GhostKingPoolMaxSize;

    private ObjectPool<SkillBoss> skill3GhostKingPool;
    public ObjectPool<SkillBoss> Skill3GhostKingPool => skill3GhostKingPool;

    [Header("EnemyPool")]

    [SerializeField] private List<EnemyBase> enemyPrefabList;

    [SerializeField] private Transform enemyRoot;

    [SerializeField] private int enemyPoolSize;

    [SerializeField] private int enemyPoolMaxSize;

    private List<ObjectPool<EnemyBase>> enemyPoolsList;
    public List<ObjectPool<EnemyBase>> EnemytPoolsList => enemyPoolsList;


    [Header("Ghost Sprite Pool")]

    [SerializeField] private GhostSprite ghostSpritePrefab;

    [SerializeField] private Transform ghostSpriteRoot;

    [SerializeField] private int ghostSpritePoolSize;

    [SerializeField] private int ghostSpritePoolMaxSize;

    private ObjectPool<GhostSprite> ghostSpritePools;
    public ObjectPool<GhostSprite> GhostSpritePools => ghostSpritePools;

    

    public void Init()
    {
        lootPool = new ObjectPool<LootItem>(lootItemPrefab, lootPoolSize, lootPoolMaxSize, lootRoot);
        floatingTextPool = new ObjectPool<FloatingText>(floatingTextPrefab, floatingTextPoolSize, floatingTextPoolMaxSize, floatingTextRoot);
        bulletPoolsList = new List<ObjectPool<BulletController>>();
        ghostSpritePools = new ObjectPool<GhostSprite>(ghostSpritePrefab, ghostSpritePoolSize, ghostSpritePoolMaxSize, ghostSpriteRoot);
        for(int i =  0; i < bulletPrefabList.Count; i++)
        {
            bulletPoolsList.Add(new ObjectPool<BulletController>(bulletPrefabList[i], bulletPoolSize, bulletPoolMaxSize, bulletRoot));
        }
        enemyPoolsList = new  List<ObjectPool<EnemyBase>>();

        for(int i = 0; i < enemyPrefabList.Count; i++)
        {
            enemyPoolsList.Add(new ObjectPool<EnemyBase>(enemyPrefabList[i], enemyPoolSize, enemyPoolMaxSize, enemyRoot));
        }


    }
    public void InitSkillGhostKing()
    {
        skill3GhostKingPool  = new ObjectPool<SkillBoss>(skill3GhostKingPrefab, skill3GhostKingPoolSize, skill3GhostKingPoolMaxSize, skill3GhostKingRoot);
    
    }

    

    void Awake()
    {
        Init();
    
    }
}
