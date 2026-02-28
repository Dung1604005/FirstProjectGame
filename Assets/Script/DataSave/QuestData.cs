using System.Collections.Generic;
[System.Serializable]
public class QuestData
{
    public List<int> completedQuestId = new List<int>();

    public List<QuestProgressSaveData> questProgressSaveDatas = new List<QuestProgressSaveData>();

    public List<NpcSaveData> savedNpcList = new List<NpcSaveData>();

    public QuestData()
    {
        
    }

    public QuestData(List<int> _completedQuestId, List<QuestProgressSaveData> _questProgressSaveDatas,  List<NpcSaveData> _savedNpcList)
    {
        completedQuestId = new List<int>(_completedQuestId);
        questProgressSaveDatas = new List<QuestProgressSaveData>(_questProgressSaveDatas);
        savedNpcList = new List<NpcSaveData>(_savedNpcList);
    }


}
