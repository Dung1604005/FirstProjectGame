using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public enum QuestState {
    InProgress, Complete
}
public class QuestProgress
{
    private int questId;
    public int QuestId => questId;

    private QuestState questState;
    private List<Objective> objectives;

    private List<int> curCount = new List<int>();


    public QuestProgress(int _questId, List<Objective> _objectives)
    {
        this.questId = _questId;
        this.objectives = _objectives;
        this.questState = QuestState.InProgress;
        for (int i = 0; i < _objectives.Count; i++)
        {
            curCount.Add(0);
        }
    }
    public void UpdateProgress(int amount, ObjectiveType type, int _targetId)
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].objectiveType == type && objectives[i].targetId == _targetId)
            {
                curCount[i] = Math.Min(objectives[i].requiredCount, amount);
            }
        }

    }
    public bool checkProgress()
    {
        for (int i = 0; i < objectives.Count; i++)
        {

            if (curCount[i] != objectives[i].requiredCount)
            {
                return false;
            }

        }
        questState = QuestState.Complete;
        return true;
    }
}
