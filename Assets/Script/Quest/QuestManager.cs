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

        OnQuestChange?.Invoke();


    }
    public void Complete(int id)
    {


        curQuestDefinitions.RemoveAt(id);
        questProgresses.RemoveAt(id);
        OnQuestChange?.Invoke();


    }

    public void UpdateProgress(int amount, int id, int idQuest, ObjectiveType objectiveType)
    {

        questProgresses[idQuest].UpdateProgress(amount, objectiveType, id);
        if (questProgresses[idQuest].checkProgress())
        {
            Complete(idQuest);
        }
    }
    public void UpdateProgressAllQuest(int amount, int id, ObjectiveType objectiveType)
    {
        for (int i = curQuestDefinitions.Count - 1; i >= 0; i--)
        {
            UpdateProgress(amount, id, i, objectiveType);
        }
    }
    void Start()
    {
        
    }
    
}
