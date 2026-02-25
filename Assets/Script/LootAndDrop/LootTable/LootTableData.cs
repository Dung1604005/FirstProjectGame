using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Loot/LootTable")]
public class LootTableData : ScriptableObject
{
    [SerializeField] private List<ItemData> entries;
    [SerializeField] private List<int> weights;

    [SerializeField] private int sumWeight;

    public ItemData GetRandomItem()
    {
        
        int val = UnityEngine.Random.Range(0, sumWeight + 1);
        ItemData item = null;
        int currentWeight = 0;
        for (int i = 0; i < weights.Count; i++)
        {
            currentWeight += weights[i];
            if (val <= currentWeight)
            {
                item = entries[i];
                break;
            }
        }
        Debug.Log(val);
        return item;
    }

}
