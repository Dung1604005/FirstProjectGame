using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem
{
    [SerializeField] private string warningFull= "INVENTORY FULL!";
    // List chua du lieu cac slot trong inven
    private List<InventorySlot> slots;

    public List<InventorySlot> All_Slots => slots;

    // Mot event de thong bao inventory bi thay doi
    public event Action OnChangeInventory;

    // Khoi tao inven
    public InventorySystem(int size)
    {
        slots = new List<InventorySlot>();
        for (int i = 0; i < size; i++)
        {
            slots.Add(new InventorySlot(null, 0));
        }
    }
    //Kiem tra xem co the add khong
    public bool TryAdd(ItemData item, int amount)
    {
        foreach (var slot in slots)
        {
            //Neu rong thi push vao
            if (slot.ItemData == null)
            {
                return true;

            }
            else
            {
                // Neu gap cung loai
                if (item.Type == slot.ItemData.Type)
                {
                    if (item.Stackable && slot.ItemData.MaxStack >= slot.Count + amount)
                    {

                        return true;
                    }
                }
            }
        }
        
        UIManageMent.Instance.UpdateWarning(warningFull);
        UIManageMent.Instance.TurnOnWarning();
        return false;
    }
    public void Add(ItemData item, int amount)
    {
        foreach (var slot in slots)
        {
            //Neu rong thi push vao
            if (slot.ItemData == null)
            {

                if (item.Stackable)
                {
                    slot.Set(item, amount);

                }
                else
                {
                    slot.Set(item, 1);
                }

                OnChangeInventory?.Invoke();
                return;


            }
            else
            {
                // Neu gap cung loai
                if (item.Type == slot.ItemData.Type)
                {
                    if (item.Stackable && slot.ItemData.MaxStack >= slot.Count + amount)
                    {

                        slot.Add(amount);
                        OnChangeInventory?.Invoke();
                        return;

                    }
                }
            }
        }

    }

    public void Remove(int index, int amount)
    {
        if (index < slots.Count)
        {
            if (slots[index].ItemData != null)
            {
                // Xoa 1 vat pham

                slots[index].Add(-amount);
                if (slots[index].Count == 0)
                {
                    slots[index].Set(null, 0);
                }
                OnChangeInventory?.Invoke();
                return;

            }

        }

    }

    public void Swap(int index1, int index2)
    {

        InventorySlot cmp = slots[index1];
        slots[index1] = slots[index2];
        slots[index2] = cmp;
        OnChangeInventory?.Invoke();

    }

    // Lay data
    public InventorySlot GetSlotData(int index)
    {
        return slots[index];
    }
}
