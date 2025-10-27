using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.WSA;

public class DropSystem : MonoBehaviour
{
    [SerializeField] private List<LootTableData> lootTableDataBase;

    

    void Start()
    {

    }
   
    public void DropItem(int index, int amount, Vector2 posSpawn)
    {
        ItemData itemData = lootTableDataBase[index].GetRandomItem();
        for (int i = 1; i <= amount; i++)
        {
            LootItem obj = GameManageMent.Instance.PoolManager.LootPool.Spawn(posSpawn);
            obj.SetInfo(itemData.Index, 1);
        }
    }





}
