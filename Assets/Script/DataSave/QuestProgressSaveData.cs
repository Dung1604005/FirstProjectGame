using System.Collections.Generic;
[System.Serializable]
public class QuestProgressSaveData
{
    public int questId; 

    
    public List<int> curCount; 

    
    public QuestProgressSaveData()
    {
        curCount = new List<int>();
    }

    public QuestProgressSaveData(int id, List<int> counts)
    {
        questId = id;
        curCount = new List<int>(counts);
    }
}
