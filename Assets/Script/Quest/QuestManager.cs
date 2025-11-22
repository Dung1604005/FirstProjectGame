using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestDefinition> curQuestDefinitions = new List<QuestDefinition>();

    public List<QuestDefinition> CurQuestDefinitons => curQuestDefinitions;

    [SerializeField] private List<QuestProgress> questProgresses = new List<QuestProgress>();

    public List<QuestProgress> QuestProgresses => questProgresses;

    [SerializeField] private bool onQuest;

    public bool OnQuest => onQuest;


    public event Action OnQuestChange;
    public event Action OnCompleteQuest;


    public void AcceptQuest(QuestDefinition quest)
    {

        curQuestDefinitions.Add(quest);
        questProgresses.Add(new QuestProgress(quest.Id, quest.Objectives));
        Debug.Log("here");

        OnQuestChange?.Invoke();
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
    void Start()
    {
        GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.OnChangeInventory += UpdateProgressAllQuestCollect;
    }

}
