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

    

    public void Init()
    {
        lootPool = new ObjectPool<LootItem>(lootItemPrefab, lootPoolSize, lootPoolMaxSize, lootRoot);
        floatingTextPool = new ObjectPool<FloatingText>(floatingTextPrefab, floatingTextPoolSize, floatingTextPoolMaxSize, floatingTextRoot);
        bulletPoolsList = new List<ObjectPool<BulletController>>();
        for(int i =  0; i < bulletPrefabList.Count; i++)
        {
            bulletPoolsList.Add(new ObjectPool<BulletController>(bulletPrefabList[i], bulletPoolSize, bulletPoolMaxSize, bulletRoot));
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
