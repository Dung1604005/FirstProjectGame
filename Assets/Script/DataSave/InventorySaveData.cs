


using System.Collections.Generic;

[System.Serializable]
public class InventorySaveData
{
    public List<ItemSaveData> savedItems = new List<ItemSaveData>();

    public InventorySaveData() { }

    public InventorySaveData(List<ItemSaveData> itemSaveDatas)
    {
        savedItems = new List<ItemSaveData>(itemSaveDatas);
    }
}


[System.Serializable]

public class ItemSaveData
{
    public int itemId;

    public int count;

    public ItemSaveData() { }

    public ItemSaveData(int id, int _count)
    {
        itemId = id;
        count = _count;
    }
}



