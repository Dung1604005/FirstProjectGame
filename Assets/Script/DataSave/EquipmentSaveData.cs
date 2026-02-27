using System.Collections.Generic;

[System.Serializable]
public class EquipmentSaveData
{
    public List<ItemSaveData> savedEquipment = new List<ItemSaveData>();

    public EquipmentSaveData(List<ItemSaveData> itemSaveDatas)
    {
        savedEquipment = new List<ItemSaveData>(itemSaveDatas);
    }
}
