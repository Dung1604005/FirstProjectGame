using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestDefinition> curQuestDefinitions = new List<QuestDefinition>();

    public List<QuestDefinition> CurQuestDefinitons => curQuestDefinitions;

    [SerializeField] private List<QuestProgress> questProgresses = new List<QuestProgress>();

    public List<QuestProgress> QuestProgresses => questProgresses;

    [SerializeField] private bool onQuest;

    public bool OnQuest => onQuest;

    [SerializeField] private ArrowQuest arrowQuest;

    public ArrowQuest ArrowQuest => arrowQuest;

    Dictionary<int, NpcDataValue> npcData;


    public event Action OnQuestChange;
    public event Action OnCompleteQuest;


    public void AcceptQuest(QuestDefinition quest)
    {

        curQuestDefinitions.Add(quest);
        questProgresses.Add(new QuestProgress(quest.Id, quest.Objectives));
        Debug.Log("here");

        OnQuestChange?.Invoke();
        if(curQuestDefinitions.Count == 1)
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
        OnCompleteQuest?.Invoke();
        
        GameManageMent.Instance.PlayerManager.Gold.AddGold(curQuestDefinitions[id].GoldReward);
        GameManageMent.Instance.PlayerManager.ExpSystem.GainExp(curQuestDefinitions[id].ExpReward);
        curQuestDefinitions.RemoveAt(id);
        questProgresses.RemoveAt(id);
        OnQuestChange?.Invoke();
        if(curQuestDefinitions.Count > 0)
        {
            UIManageMent.Instance.QuestUI.QuestViewInfo.SetInfo(0);
        }
        return true;


    }

    public void UpdateProgressKillOrReach(int amount, int id, int idQuest, ObjectiveType objectiveType)
    {

        questProgresses[idQuest].UpdateProgressKillOrReach(amount, objectiveType, id);
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
    public void UpdateProgressAllQuestKillOrReach(int amount, int id, ObjectiveType objectiveType)
    {
        for (int i = curQuestDefinitions.Count - 1; i >= 0; i--)
        {
            UpdateProgressKillOrReach(amount, id, i, objectiveType);
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
    public void UpdateNpcData(int indexNpc, bool onQuest, int curNpcDialogues, int indexDialogue, bool canContinueDialogue)
    {
        NpcDataValue npcDataValue = new NpcDataValue(onQuest, curNpcDialogues, indexDialogue, canContinueDialogue);
        if (npcData.ContainsKey(indexNpc))
        {
            npcData[indexNpc] =  npcDataValue;
            
        }
        else
        {
            npcData.Add(indexNpc, npcDataValue);
        }
    }
    public NpcDataValue  GetNpcData(int indexNpc)
    {
        NpcDataValue npcDataValue = new NpcDataValue(false, 0, 0, true);
        if(npcData.ContainsKey(indexNpc))
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

public  class NpcDataValue
{
    public bool onQuest {get; private set;}
    public int curNpcDialogues {get; private set;}

    public int indexDialogue {get; private set;}

    public bool canContinueDialogue {get; private set;}

    public NpcDataValue(bool _onQuest, int _curNpcDialogues, int _indexDialogue,bool _canContinueDialogue)
    {
        onQuest = _onQuest;
        curNpcDialogues = _curNpcDialogues;
        indexDialogue = _indexDialogue;
        canContinueDialogue = _canContinueDialogue;
    }
}
