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


    public void AcceptQuest(QuestDefinition quest)
    {

        curQuestDefinitions.Add(quest);
        questProgresses.Add(new QuestProgress(quest.Id, quest.Objectives));
        Debug.Log("here"); 

        OnQuestChange?.Invoke();


    }
    public void Complete(int id)
    {


        curQuestDefinitions.RemoveAt(id);
        questProgresses.RemoveAt(id);
        OnQuestChange?.Invoke();


    }

    public void UpdateProgressKillOrReach(int amount, int id, int idQuest, ObjectiveType objectiveType)
    {

        questProgresses[idQuest].UpdateProgressKillOrReach(amount, objectiveType, id);
        if (questProgresses[idQuest].checkProgress())
        {
            Complete(idQuest);
        }
    }
    public void UpdateProgressCollect(int idQuest)
    {
        questProgresses[idQuest].UpdateCollectProgress();
        if (questProgresses[idQuest].checkProgress())
        {
            Complete(idQuest);
        }
    }
    public void UpdateProgressAllQuestKillOrReach(int amount, int id, ObjectiveType objectiveType)
    {
        for (int i = curQuestDefinitions.Count - 1; i >= 0; i--)
        {
            UpdateProgressKillOrReach(amount, id, i, objectiveType);
        }
    }
    public void UpdateProgressAllQuestCollect()
    {
            
        for (int i = 0; i < curQuestDefinitions.Count; i++)
        {
            UpdateProgressCollect(i);
        }
    }
    void Start()
    {
        GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.OnChangeInventory += UpdateProgressAllQuestCollect;
    }
    
}
