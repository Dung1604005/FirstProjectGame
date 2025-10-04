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
    public void SetInfo(int index )
    {
        
    }
}
