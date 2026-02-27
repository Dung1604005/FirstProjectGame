using System.Collections.Generic;
[System.Serializable]
public class QuestData
{
    public List<int> completedQuestId;

    public List<QuestProgressSaveData> questProgressSaveDatas;

    public List<NpcSaveData> savedNpcList;

    public QuestData(List<int> _completedQuestId, List<QuestProgressSaveData> _questProgressSaveDatas,  List<NpcSaveData> _savedNpcList)
    {
        completedQuestId = new List<int>(_completedQuestId);
        questProgressSaveDatas = new List<QuestProgressSaveData>(_questProgressSaveDatas);
        savedNpcList = new List<NpcSaveData>(_savedNpcList);
    }


}
