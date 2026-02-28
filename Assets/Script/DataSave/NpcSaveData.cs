[System.Serializable]

public class NpcSaveData
{
    public int npcId; 
    public NpcDataValue data; 

    public NpcSaveData()
    {
        data = new NpcDataValue();
    }

    public NpcSaveData(int id, NpcDataValue npcData)
    {
        npcId = id;
        data = npcData;
    }
}
