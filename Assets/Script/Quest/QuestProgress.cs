using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public enum QuestState
{
    InProgress, Complete
}

[System.Serializable]
public class QuestProgress
{
    private int questId;
    public int QuestId => questId;

    private QuestState questState;
    public QuestState QuestState => questState;
    [SerializeField] private List<Objective> objectives = new List<Objective>();

    public List<Objective> Objectives => objectives;


    [SerializeField] private List<int> curCount = new List<int>();

    public List<int> CurCount => curCount;

    private float progressRatio;
    public float ProgressRatio => progressRatio;

    [SerializeField] private Objective firstObjectiveNotCompleted;

    public Objective FirstObjectiveNotCompleted => firstObjectiveNotCompleted;

    public void RestoreFromSave(List<int> savedCounts)
    {
        curCount = new List<int>(savedCounts);

        UpdateProgressRatio();
        checkProgress();
    }


    public QuestProgress(int _questId, List<Objective> _objectives)
    {
        
        this.questId = _questId;
        this.objectives = _objectives;
        this.questState = QuestState.InProgress;
        for (int i = 0; i < _objectives.Count; i++)
        {
            curCount.Add(0);
            
        }
        firstObjectiveNotCompleted = _objectives[0];
    }
    public void UpdateCollectProgress()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].objectiveType == ObjectiveType.Collect)
            {
                if(GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.ItemCount.TryGetValue(objectives[i].targetId, out var cur))
                {
                    curCount[i] = Math.Min(objectives[i].requiredCount, cur);
                }
            }
        }
        UpdateProgressRatio();

    }
    public void UpdateProgressKill(int amount, ObjectiveType type, int _targetId)
    {
        if(type != ObjectiveType.Kill)
        {
            
            return;
        }
        for (int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].objectiveType == type && objectives[i].targetId == _targetId)
            {
                curCount[i] = Math.Min(objectives[i].requiredCount, amount);
            }
        }
        UpdateProgressRatio();

    }
    public void UpdateProgressTalkToNpc(int npcId)
    {
        
        for(int i  =  0; i < objectives.Count; i++)
        {
            if(objectives[i].objectiveType == ObjectiveType.TalkToNpc && objectives[i].targetId == npcId)
            {
                curCount[i] = Math.Min(objectives[i].requiredCount, 1);
            }
        }
        UpdateProgressRatio();
    }
    public bool checkProgress()
    {
        for (int i = 0; i < objectives.Count; i++)
        {

            if (curCount[i] < objectives[i].requiredCount)
            {
                return false;
            }

        }
        questState = QuestState.Complete;
        return true;
    }
    public void UpdateProgressRatio()
    {
        float ratio = 0f;
        int tempId = 99;
        for(int i = 0; i < curCount.Count && i < objectives.Count; i++)
        {
            if(curCount[i] < objectives[i].requiredCount)
            {
                // Lay id cua Objective be nhat chua hoan thanh 
                tempId = Math.Min(i, tempId);
            }
            float ratioObjective = (float)curCount[i]/objectives[i].requiredCount;
            ratioObjective /= curCount.Count;
            ratio += ratioObjective;
        }
        progressRatio = ratio;
        if(tempId < 99)
        {
            firstObjectiveNotCompleted = objectives[tempId];
        }
        else
        {
            firstObjectiveNotCompleted = null;
        }
    }
}
