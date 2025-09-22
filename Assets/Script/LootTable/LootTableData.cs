using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot/LootTable")]
public class LootTableData : ScriptableObject
{
    [SerializeField] private List<ItemData> entries;
    [SerializeField] private List<int> weights;

    public ItemData GetRandomItem()
    {
        int val = UnityEngine.Random.Range(1, 101);
        ItemData item = null;
        for (int i = 0; i < weights.Count; i++)
        {
            if (val <= weights[i])
            {
                item = entries[i];
            }
        }
        return item;
    }

}
