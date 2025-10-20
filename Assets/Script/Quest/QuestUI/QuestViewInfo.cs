using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestViewInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameQuest;

    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private TextMeshProUGUI objectives;

    [SerializeField] private TextMeshProUGUI rewards;

    public void TurnOn()
    {
        this.gameObject.SetActive(true);
    }
    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
    public void SetInfo(int index)
    {
        nameQuest.text = GameManageMent.Instance.QuestManager.CurQuestDefinitons[index].NameQuest;
        description.text = "Description:\n" + GameManageMent.Instance.QuestManager.CurQuestDefinitons[index].Description;
        objectives.text = "Objectives:\n";
        for(int i = 0; i < GameManageMent.Instance.QuestManager.QuestProgresses[index].Objectives.Count; i++)
        {
            int id = GameManageMent.Instance.QuestManager.QuestProgresses[index].Objectives[i].targetId;    
            String nameTarget = GameManageMent.Instance.ItemDataBase.ItemDatas[id].ItemName;  
            objectives.text += "\n" + GameManageMent.Instance.QuestManager.QuestProgresses[index].Objectives[i].objectiveType.ToString() + " " + nameTarget + " " + "[" +
                GameManageMent.Instance.QuestManager.QuestProgresses[index].CurCount[i]   + "/"  +
                GameManageMent.Instance.QuestManager.QuestProgresses[index].Objectives[i].requiredCount + "]" ;
        }
        
        
    }
}
