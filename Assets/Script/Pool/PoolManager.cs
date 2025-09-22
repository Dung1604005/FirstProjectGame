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

    void Awake()
    {
        lootPool = new ObjectPool<LootItem>(lootItemPrefab, lootPoolSize, lootPoolMaxSize, lootRoot);
    }
}
