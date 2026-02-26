using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    [SerializeField] private QuestDataBase questDataBase;

    [SerializeField] private List<QuestDefinition> completedQuest = new List<QuestDefinition>();

    public List<QuestDefinition> CompletedQuest => completedQuest;
    [SerializeField] private List<QuestDefinition> curQuestDefinitions = new List<QuestDefinition>();

    public List<QuestDefinition> CurQuestDefinitons => curQuestDefinitions;

    [SerializeField] private List<QuestProgress> questProgresses = new List<QuestProgress>();

    public List<QuestProgress> QuestProgresses => questProgresses;

    [SerializeField] private bool onQuest;

    public bool OnQuest => onQuest;

    [SerializeField] private ArrowQuest arrowQuest;

    public ArrowQuest ArrowQuest => arrowQuest;

    private Dictionary<int, NpcDataValue> npcData;


    public event Action OnQuestChange;
    public event Action OnCompleteQuest;

    public List<NpcSaveData> GetNpcSaveData()
    {
        List<NpcSaveData> listSave = new List<NpcSaveData>();

        // Duyệt qua từng phần tử trong Dictionary
        foreach (KeyValuePair<int, NpcDataValue> item in npcData)
        {
            
            NpcSaveData saveItem = new NpcSaveData(item.Key, item.Value);
            listSave.Add(saveItem);
        }

        return listSave;
    }

    public void LoadNpcData(List<NpcSaveData> savedNpcList)
    {
        
        npcData.Clear();

        
        if (savedNpcList == null) return;

        
        foreach (NpcSaveData savedItem in savedNpcList)
        {
            
            npcData.Add(savedItem.npcId, savedItem.data);
        }
    }

    
    public List<int> GetCompletedQuestIDs()
    {
        List<int> listIDs = new List<int>();
        foreach (QuestDefinition quest in completedQuest)
        {
            listIDs.Add(quest.Id);
        }
        return listIDs;
    }
    

    
    public void LoadCompletedQuests(List<int> savedCompletedIDs)
    {
        completedQuest.Clear();
        foreach (int id in savedCompletedIDs)
        {
            QuestDefinition originalQuest = questDataBase.GetQuestByID(id);
            if (originalQuest != null)
            {
                completedQuest.Add(originalQuest);
            }
        }
    }

    public List<QuestProgressSaveData> GetQuestProgressSaveData()
    {
        List<QuestProgressSaveData> listSave = new List<QuestProgressSaveData>();

        foreach (QuestProgress progress in questProgresses)
        {
            QuestProgressSaveData saveData = new QuestProgressSaveData(progress.QuestId, progress.CurCount);
            listSave.Add(saveData);
        }

        return listSave;
    }

    public void LoadQuestProgress(List<QuestProgressSaveData> savedProgressList)
    {
        // Xóa sạch nhiệm vụ đang làm hiện tại
        curQuestDefinitions.Clear();
        questProgresses.Clear();

        foreach (QuestProgressSaveData savedData in savedProgressList)
        {

            QuestDefinition originalQuest = questDataBase.GetQuestByID(savedData.questId);
            curQuestDefinitions.Add(originalQuest);


            QuestProgress newProgress = new QuestProgress(originalQuest.Id, originalQuest.Objectives);


            newProgress.RestoreFromSave(savedData.curCount);


            questProgresses.Add(newProgress);
        }


        OnQuestChange?.Invoke();
    }


    public void AcceptQuest(QuestDefinition quest)
    {

        curQuestDefinitions.Add(quest);
        questProgresses.Add(new QuestProgress(quest.Id, quest.Objectives));
        Debug.Log("here");

        OnQuestChange?.Invoke();
        if (curQuestDefinitions.Count == 1)
        {
            UIManageMent.Instance.QuestUI.QuestViewInfo.SetInfo(0);
        }
    }
    public bool Complete(int id)
    {
        if (!questProgresses[id].checkProgress())
        {
            return false;
        }
        List<ItemStack> rewards = curQuestDefinitions[id].ItemIdReward;
        if (rewards != null)
        {
            for (int i = 0; i < rewards.Count; i++)
            {
                LootItem obj = GameManageMent.Instance.PoolManager.LootPool.Spawn(GameManageMent.Instance.PlayerManager.PlayerController.getPos());
                obj.SetInfo(rewards[i].ItemId, rewards[i].Count);
            }
        }
        completedQuest.Add(curQuestDefinitions[id]);
        OnCompleteQuest?.Invoke();

        GameManageMent.Instance.PlayerManager.Gold.AddGold(curQuestDefinitions[id].GoldReward);
        GameManageMent.Instance.PlayerManager.ExpSystem.GainExp(curQuestDefinitions[id].ExpReward);
        curQuestDefinitions.RemoveAt(id);
        questProgresses.RemoveAt(id);
        OnQuestChange?.Invoke();
        if (curQuestDefinitions.Count > 0)
        {
            UIManageMent.Instance.QuestUI.QuestViewInfo.SetInfo(0);
        }
        return true;


    }

    public void UpdateProgressKill(int amount, int id, int idQuest, ObjectiveType objectiveType)
    {

        questProgresses[idQuest].UpdateProgressKill(amount, objectiveType, id);
        // if (questProgresses[idQuest].checkProgress())
        // {
        //     Complete(idQuest);
        // }
    }
    public void UpdateProgressCollect(int idQuest)
    {
        questProgresses[idQuest].UpdateCollectProgress();
        // if (questProgresses[idQuest].checkProgress())
        // {
        //     Complete(idQuest);
        // }
    }
    public void UpdateProgressTalkToNpc(int idQuest, int npcId)
    {
        questProgresses[idQuest].UpdateProgressTalkToNpc(npcId);
    }
    public void UpdateProgressAllQuestKill(int amount, int id, ObjectiveType objectiveType)
    {
        for (int i = curQuestDefinitions.Count - 1; i >= 0; i--)
        {
            UpdateProgressKill(amount, id, i, objectiveType);
        }
        OnQuestChange?.Invoke();
    }
    public void UpdateProgressAllQuestCollect()
    {

        for (int i = curQuestDefinitions.Count - 1; i >= 0; i--)
        {
            UpdateProgressCollect(i);
        }
        OnQuestChange?.Invoke();
    }
    public void UpdateProgressAllQuestTalkToNpc(int npcId)
    {

        for (int i = curQuestDefinitions.Count - 1; i >= 0; i--)
        {
            UpdateProgressTalkToNpc(i, npcId);
        }
        OnQuestChange?.Invoke();
    }
    public void UpdateNpcData(int indexNpc, bool onQuest, int curNpcDialogues, int indexDialogue, bool canContinueDialogue)
    {
        NpcDataValue npcDataValue = new NpcDataValue(onQuest, curNpcDialogues, indexDialogue, canContinueDialogue);
        if (npcData.ContainsKey(indexNpc))
        {
            npcData[indexNpc] = npcDataValue;

        }
        else
        {
            npcData.Add(indexNpc, npcDataValue);
        }
    }
    public NpcDataValue GetNpcData(int indexNpc)
    {
        NpcDataValue npcDataValue = new NpcDataValue(false, 0, 0, true);
        if (npcData.ContainsKey(indexNpc))
        {
            return npcData[indexNpc];
        }
        else
        {
            return npcDataValue;
        }
    }
    void Start()
    {
        npcData = new Dictionary<int, NpcDataValue>();
        GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.OnChangeInventory += UpdateProgressAllQuestCollect;
        UIManageMent.Instance.QuestUI.Init();
        UIManageMent.Instance.QuestUI.QuestViewInfo.Init();
    }

}
[System.Serializable]
public class NpcDataValue
{
    public bool onQuest ;
    public int curNpcDialogues ;

    public int indexDialogue ;

    public bool canContinueDialogue ;

    public NpcDataValue(bool _onQuest, int _curNpcDialogues, int _indexDialogue, bool _canContinueDialogue)
    {
        onQuest = _onQuest;
        curNpcDialogues = _curNpcDialogues;
        indexDialogue = _indexDialogue;
        canContinueDialogue = _canContinueDialogue;
    }
}
