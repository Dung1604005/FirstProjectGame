using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<QuestDefinition> curQuestDefinitions = new List<QuestDefinition>();
    [SerializeField] private List<QuestProgress> questProgresses = new List<QuestProgress>();

    [SerializeField] private bool onQuest;

    public bool OnQuest => onQuest;

    private List<int> completedQuest = new List<int>();

    public event Action OnAcceptingQuest;


    public void AcceptQuest(QuestDefinition quest)
    {

        curQuestDefinitions.Add(quest);
        questProgresses.Add(new QuestProgress(quest.Id, quest.Objectives));

        OnAcceptingQuest?.Invoke();


    }
    public void Complete(int id)
    {

    }
    public void CancelQuest(int id)
    {
        curQuestDefinitions.RemoveAt(id);
        questProgresses.RemoveAt(id);
    }
    public void UpdateProgress(int amount, int id, int idQuest, ObjectiveType objectiveType)
    {

        questProgresses[idQuest].UpdateProgress(amount, objectiveType, id);

        if (questProgresses[idQuest].checkProgress())
        {
            Complete(idQuest);
            curQuestDefinitions.RemoveAt(idQuest);
            questProgresses.RemoveAt(idQuest);

        }
    }
    public void UpdateProgressAllQuest(int amount, int id, ObjectiveType objectiveType)
    {
        completedQuest.Clear();
        for (int i = curQuestDefinitions.Count - 1; i >= 0; i--) {
            UpdateProgress(amount, id, i, objectiveType);
        }
        
    }
    
}
