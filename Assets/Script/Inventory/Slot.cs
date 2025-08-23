using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Slot
{
    protected ItemData itemData;
    public ItemData ItemData => itemData;
    protected int count;
    public int Count => count;

    public void Set(ItemData _itemData, int _amount)
    {
        itemData = _itemData;
        count = _amount;
    }
    public void Remove(int amount)
    {
        count -= amount;
        if (count < 0)
        {
            count = 0;
            Clear();
        }
    }

    public bool IsEmpty()
    {
        if (count == 0 || itemData == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Clear()
    {
        count = 0;
        itemData = null;
    }
}
