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

    void Awake()
    {
        lootPool = new ObjectPool<LootItem>(lootItemPrefab, lootPoolSize, lootPoolMaxSize, lootRoot);
        floatingTextPool = new ObjectPool<FloatingText>(floatingTextPrefab, floatingTextPoolSize, floatingTextPoolMaxSize, floatingTextRoot);
    }
}
