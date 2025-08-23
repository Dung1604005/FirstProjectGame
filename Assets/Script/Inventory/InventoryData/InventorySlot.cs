using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InventorySlot: Slot
{
    
    public InventorySlot(ItemData _itemData, int _amount)
    {
        itemData = _itemData;
        count = _amount;
    }
    
    public void Add(int amount)
    {
        count += amount;
    }
    
}
