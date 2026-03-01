
using System.Collections.Generic;

using UnityEngine;


public class DropSystem : MonoBehaviour
{
    [SerializeField] private List<LootTableData> lootTableDataBase;

    

    void Start()
    {

    }
   
    public void DropItem(int index, int amount, Vector2 posSpawn)
    {
        Debug.Log(lootTableDataBase.Count + " " + index);
        ItemData itemData = lootTableDataBase[index].GetRandomItem();
        Debug.Log(itemData.ItemName);
        for (int i = 1; i <= amount; i++)
        {
            LootItem obj = GameManageMent.Instance.PoolManager.LootPool.Spawn(posSpawn);
            obj.SetInfo(itemData.Index, 1);
        }
    }





}
