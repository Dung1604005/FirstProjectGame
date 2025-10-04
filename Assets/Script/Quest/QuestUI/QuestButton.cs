using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class QuestButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameQuest;
    [SerializeField] private int index;

    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
    }

    public void SetInfo(string _name, int _index)
    {
        this.nameQuest.text= _name;
        this.index = _index; ;
    }
    public void ViewQuestInfo()
    {
        UIManageMent.Instance.QuestUI.QuestViewInfo.SetInfo(index);
        UIManageMent.Instance.QuestUI.QuestViewInfo.SetInfo(index);
    }
}
