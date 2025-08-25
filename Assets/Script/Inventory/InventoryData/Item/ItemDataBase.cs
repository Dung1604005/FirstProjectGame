using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Script/ItemDataBase", fileName = "ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField] List<ItemData> itemDatas = new List<ItemData>(18);
    public List<ItemData> ItemDatas => itemDatas;
    
}
