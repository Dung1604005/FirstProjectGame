using System;

using System.Collections.Generic;


public class InventorySystem
{
    private Dictionary<int, int> itemCount = new Dictionary<int, int>();
    public Dictionary<int, int> ItemCount => itemCount;

    // List chua du lieu cac slot trong inven
    private List<InventorySlot> slots;

    public List<InventorySlot> All_Slots => slots;

    // Mot event de thong bao inventory bi thay doi
    public event Action OnChangeInventory;

    public List<ItemSaveData> GetSaveInventoryData()
    {
        List<ItemSaveData> saveDatas = new List<ItemSaveData>();
        
        for(int i = 0 ; i < slots.Count; i++)
        {
            if(slots[i].ItemData != null && slots[i].Count > 0)
            {
                saveDatas.Add(new ItemSaveData(slots[i].ItemData.Index, slots[i].Count));
            }
            
        }
        return saveDatas;
        
    }
    public void LoadInventoryData(List<ItemSaveData> itemSaveDatas)
    {
        itemCount.Clear();
        if (itemSaveDatas == null) return;
        for(int i = 0; i < slots.Count; i++)
        {
            slots[i] = new InventorySlot(null, 0);
            
        }
        OnChangeInventory?.Invoke();
        for(int i = 0;  i < itemSaveDatas.Count; i++)
        {
            
            
            Add(GameManageMent.Instance.ItemDataBase.ItemDatas[itemSaveDatas[i].itemId], itemSaveDatas[i].count, true);
            
        }
    }

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
        if(item.Type == ItemType.Bullet)
        {
             
             return true;
        }
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
                if (item.Index == slot.ItemData.Index)
                {
                    if (item.Stackable && slot.ItemData.MaxStack >= slot.Count + amount)
                    {

                        return true;
                    }
                }
            }
        }

        UIManageMent.Instance.UpdateWarning(GameConfig.INVENTORY_FULL_WARNING);
        UIManageMent.Instance.TurnOnWarning();
        return false;
    }
    public void Add(ItemData item, int amount, bool force = false)
    {
        if(item.Type == ItemType.Bullet)
        {
             GameManageMent.Instance.PlayerManager.AddBullet(item.ItemName, amount*(item as BulletData).Amount);
             return;
        }
        foreach (var slot in slots)
        {
            //Neu rong thi push vao
            if (slot.ItemData == null)
            {
                continue;
            }
            else
            {
                // Neu gap cung loai
                if (item.Index == slot.ItemData.Index)
                {
                    if (item.Stackable && slot.ItemData.MaxStack >= slot.Count + amount)
                    {

                        if (itemCount.TryGetValue(item.Index, out var cur))
                            itemCount[item.Index] = cur + amount;
                        else
                            itemCount[item.Index] = amount;

                        slot.Add(amount);
                        OnChangeInventory?.Invoke();
                        if (force == false)
                        {
                            UIManageMent.Instance.AddItemToQueue(item, amount);
                        }


                        return;

                    }
                }
            }
        }
        foreach (var slot in slots)
        {
            //Neu rong thi push vao
            if (slot.ItemData == null)
            {

                if (item.Stackable)
                {

                    if (itemCount.TryGetValue(item.Index, out var cur))
                        itemCount[item.Index] = cur + amount;
                    else
                        itemCount[item.Index] = amount;


                    slot.Set(item, amount);
                }
                else
                {

                    if (itemCount.TryGetValue(item.Index, out var cur))
                        itemCount[item.Index] = cur + 1;
                    else
                        itemCount[item.Index] = 1;


                    slot.Set(item, 1);
                }


                OnChangeInventory?.Invoke();
                if (force == false)
                    UIManageMent.Instance.AddItemToQueue(item, amount);
                return;


            }
            else
            {
                continue;
            }
        }

    }

    public void RemoveByIndex(int index, int amount, bool force = false)
    {

        if (index < slots.Count)
        {

            if (slots[index].ItemData != null)
            {
                // Xoa 1 vat pham
                
                if (itemCount.TryGetValue(slots[index].ItemData.Index, out var cur))
                    itemCount[slots[index].ItemData.Index] = cur - amount;
                
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
    public bool TryRemoveItem(ItemData itemData, int amount)
    {
        if (GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.ItemCount.TryGetValue(itemData.Index, out var cur))
        {
            if (cur < amount)
            {
                return false;
            }
            return true;
        }
        return false;
    }
    public void RemoveItem(ItemData itemData, int amount)
    {
        if (!TryRemoveItem(itemData, amount))
        {
            UIManageMent.Instance.UpdateWarning(GameConfig.NOT_ENOUGH_ITEM_WARNING);
            UIManageMent.Instance.TurnOnWarning();
            return;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if(slots[i].IsEmpty() || slots[i].ItemData == null)
                continue;
            if (slots[i].ItemData.Index == itemData.Index)
            {

                if (slots[i].Count >= amount)
                {
                
                    RemoveByIndex(i, amount);
                    amount = 0;
                    return;
                }
                else
                {
                    amount -= slots[i].Count;
                    RemoveByIndex(i, slots[i].Count);

                }
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
