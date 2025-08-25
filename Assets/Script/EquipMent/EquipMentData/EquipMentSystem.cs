using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using UnityEngine;

public class EquipMentSystem
{
    private string warningFull = "EQUIP FULL!";
    [SerializeField] private int sizeEquipMent;
    public int SizeEquipMent => sizeEquipMent;

    [SerializeField] private List<EquipMentSlot> slots ;

    public List<EquipMentSlot> Slots => slots;


    public event Action OnEquipmentChange;

    public EquipMentSystem(int _size)
    {
        slots = new List<EquipMentSlot>();
        sizeEquipMent = _size;
        for (int i = 0; i < sizeEquipMent; i++)
        {
            slots.Add(new EquipMentSlot(null, 0));
        }
    }
    public ItemData GetItemData(int _index)
    {
        return slots[_index].ItemData;
    }
    public bool TryEquip(ItemData item, int amount)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty())
            {

                slots[i] = new EquipMentSlot(item, amount);
                OnEquipmentChange?.Invoke();
                return true;
            }

        }
        UIManageMent.Instance.UpdateWarning(warningFull);
        UIManageMent.Instance.TurnOnWarning();
        return false;


    }
    public void UseSlot(int index, int amount)
    {
        if (slots[index].Count == 0 || slots[index].ItemData == null)
        {
            return;
        }
        slots[index].Remove(amount);
        OnEquipmentChange?.Invoke();
    }
    public void TryUnEquip(int index)
    {
        if (UIManageMent.Instance.InventoryUI.Inven.TryAdd(slots[index].ItemData, slots[index].Count))
        {
            UIManageMent.Instance.InventoryUI.Inven.Add(slots[index].ItemData, slots[index].Count);
            slots[index].Set(null, 0);

            OnEquipmentChange?.Invoke();
        }

    }
    
}
