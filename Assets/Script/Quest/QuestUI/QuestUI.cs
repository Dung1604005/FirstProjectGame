using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class QuestUI : MenuLayOutUI
{
    [SerializeField] private List<QuestButton> questButtons = new List<QuestButton>();
    [SerializeField] private QuestViewInfo questViewInfo;

    public QuestViewInfo QuestViewInfo => questViewInfo;
    void Start()
    {
        for (int i = 0; i < questButtons.Count; i++)
        {

            questButtons[i].TurnOff();
        }
    }

    public void RefreshQuestUI()
    {
        
        
    }






}
