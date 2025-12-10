using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestProgressUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameQuest;

    [SerializeField] private Image progressSlider;

    [SerializeField] private TextMeshProUGUI objectiveText;

    private float expectedProgress;

    public void SetInfo(String _nameQuest, String _objectiveText, float progress)
    {
        nameQuest.text = _nameQuest;
        objectiveText.text = _objectiveText;
        expectedProgress = progress;
    }

    
    void Update()
    {
        
        if(progressSlider.fillAmount != expectedProgress)
        {
            progressSlider.fillAmount = Mathf.Lerp(progressSlider.fillAmount, expectedProgress, 0.1f);
        }
    }

    public void TurnOn()
    {
        this.gameObject.SetActive(true);
    }
    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
    void Start()
    {
        TurnOff();
    }
}
