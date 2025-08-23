using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipMentSlot : Slot
{
    public EquipMentSlot(ItemData _itemData, int _amount)
    {
        itemData = _itemData;
        count = _amount;

    }


}
