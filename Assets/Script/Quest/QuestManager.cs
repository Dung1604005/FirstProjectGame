using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private QuestDefinition curQuestDefinition;
    [SerializeField] private QuestProgress questProgress;

    [SerializeField] private bool onQuest;

    public bool OnQuest => onQuest;

    public void AcceptQuest(QuestDefinition quest)
    {
        if (!onQuest)
        {
            curQuestDefinition = quest;
            questProgress = new QuestProgress(quest.Id, quest.Objectives);
        }

    }
    public void Complete()
    {

    }
    public void CancelQuest()
    {
        onQuest = false;
        curQuestDefinition = null;
        questProgress = null;
    }
    public void UpdateKillProgress(int amount, int id)
    {
        if (onQuest == false)
        {
            return;
        }
        questProgress.UpdateProgress(amount, ObjectiveType.Kill, id);

        if (questProgress.checkProgress())
        {
            Complete(); 
        }
    }
    public void UpdateCollectProgress(int amount, int id)
    {
        questProgress.UpdateProgress(amount, ObjectiveType.Collect, id);
        if (questProgress.checkProgress())
        {
            Complete(); 
        }
    }
    public void UpdateReachProgress(int id)
    {
        questProgress.UpdateProgress(1, ObjectiveType.Reach, id);
        if (questProgress.checkProgress())
        {
            Complete(); 
        }
    }
}
