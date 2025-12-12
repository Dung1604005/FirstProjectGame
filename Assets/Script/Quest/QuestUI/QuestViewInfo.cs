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
    
    private int indexQuest = -1;

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
        this.indexQuest = index;
        String _nameQuest = GameManageMent.Instance.QuestManager.CurQuestDefinitons[index].NameQuest;
        nameQuest.text = _nameQuest;
        description.text = GameManageMent.Instance.QuestManager.CurQuestDefinitons[index].Description;
        objectives.text = "Objectives:";
        String objectiveText = "";
        for(int i = 0; i < GameManageMent.Instance.QuestManager.QuestProgresses[index].Objectives.Count; i++)
        {
            int id = GameManageMent.Instance.QuestManager.QuestProgresses[index].Objectives[i].targetId;    
            String nameTarget = GameManageMent.Instance.ItemDataBase.ItemDatas[id].ItemName;  
            objectives.text += "\n" + GameManageMent.Instance.QuestManager.QuestProgresses[index].Objectives[i].objectiveType.ToString() + " " + nameTarget + " " + "[" +
                GameManageMent.Instance.QuestManager.QuestProgresses[index].CurCount[i]   + "/"  +
                GameManageMent.Instance.QuestManager.QuestProgresses[index].Objectives[i].requiredCount + "]" ;
            if(i == 0)
            {
                objectiveText += GameManageMent.Instance.QuestManager.QuestProgresses[index].Objectives[i].objectiveType.ToString() + " " + nameTarget + " " + "[" +
                GameManageMent.Instance.QuestManager.QuestProgresses[index].CurCount[i]   + "/"  +
                GameManageMent.Instance.QuestManager.QuestProgresses[index].Objectives[i].requiredCount + "]" ;
            }
            else
            {
                objectiveText += "\n" + GameManageMent.Instance.QuestManager.QuestProgresses[index].Objectives[i].objectiveType.ToString() + " " + nameTarget + " " + "[" +
                GameManageMent.Instance.QuestManager.QuestProgresses[index].CurCount[i]   + "/"  +
                GameManageMent.Instance.QuestManager.QuestProgresses[index].Objectives[i].requiredCount + "]" ;
            }
        }
        rewards.text = "Rewards:";
        rewards.text += "\n+" + GameManageMent.Instance.QuestManager.CurQuestDefinitons[index].ExpReward + " Exp, +" + GameManageMent.Instance.QuestManager.CurQuestDefinitons[index].GoldReward + " Gold";
        for(int i = 0; i < GameManageMent.Instance.QuestManager.CurQuestDefinitons[index].ItemIdReward.Count; i++)
        {
            int idItem =  GameManageMent.Instance.QuestManager.CurQuestDefinitons[index].ItemIdReward[i].ItemId;
            rewards.text += "\n+" + GameManageMent.Instance.QuestManager.CurQuestDefinitons[index].ItemIdReward[i].Count + " " + GameManageMent.Instance.ItemDataBase.ItemDatas[idItem].ItemName;
        }
        UIManageMent.Instance.QuestUI.QuestProgressUI.TurnOn();
        UIManageMent.Instance.QuestUI.QuestProgressUI.SetInfo(_nameQuest, objectiveText, GameManageMent.Instance.QuestManager.QuestProgresses[index].ProgressRatio);
    }

    public void UpdateProgress()
    {
        
        if(indexQuest < 0)
        {
            return;
        }
         objectives.text = "Objectives:";
         String objectiveText = "";
         for(int i = 0; i < GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].Objectives.Count; i++)
        {
            int id = GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].Objectives[i].targetId;    
            String nameTarget = GameManageMent.Instance.ItemDataBase.ItemDatas[id].ItemName;  
            objectives.text += "\n" + GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].Objectives[i].objectiveType.ToString() + " " + nameTarget + " " + "[" +
                GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].CurCount[i]   + "/"  +
                GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].Objectives[i].requiredCount + "]" ;
            if(i == 0)
            {
                objectiveText += GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].Objectives[i].objectiveType.ToString() + " " + nameTarget + " " + "[" +
                GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].CurCount[i]   + "/"  +
                GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].Objectives[i].requiredCount + "]" ;
            }
            else
            {
                objectiveText += "\n" + GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].Objectives[i].objectiveType.ToString() + " " + nameTarget + " " + "[" +
                GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].CurCount[i]   + "/"  +
                GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].Objectives[i].requiredCount + "]" ;
            }
        }
        UIManageMent.Instance.QuestUI.QuestProgressUI.TurnOn();
        UIManageMent.Instance.QuestUI.QuestProgressUI.SetInfo(nameQuest.text, objectiveText, GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].ProgressRatio);
    }
    public void ResetQuestView()
    {
        if(indexQuest < 0)
        {
            indexQuest = -1;
            nameQuest.text = "";
            description.text = "YOU HAVE NO QUEST NOW";
            objectives.text = "";
            rewards.text = "";
            UIManageMent.Instance.QuestUI.QuestProgressUI.TurnOff();
            return;
        }
        if(GameManageMent.Instance.QuestManager.QuestProgresses[indexQuest].QuestState != QuestState.Complete)
        {
            return;
        }
        indexQuest = -1;
        nameQuest.text = "";
        description.text = "YOU HAVE NO QUEST NOW";
        objectives.text = "";
        rewards.text = "";
        UIManageMent.Instance.QuestUI.QuestProgressUI.TurnOff();

    }
    public void Init()
    {
        GameManageMent.Instance.QuestManager.OnQuestChange += UpdateProgress;
        GameManageMent.Instance.QuestManager.OnCompleteQuest += ResetQuestView;
        ResetQuestView();
    }
    void Start()
    {
        
        
        
        
    }
}
