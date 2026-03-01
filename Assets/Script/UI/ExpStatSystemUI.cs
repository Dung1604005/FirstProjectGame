using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExpStatSystemUI : MenuLayOutUI
{
    [SerializeField] private TextMeshProUGUI healthStatUI;
    [SerializeField] private TextMeshProUGUI atkStatUI;
    [SerializeField] private TextMeshProUGUI critRateStatUI;

    [SerializeField] private TextMeshProUGUI lvUI;

    [SerializeField] private TextMeshProUGUI pointStatUI;

    [SerializeField] private Button hpButton;

    


    [SerializeField] private Button atkButton;

    

    [SerializeField] private Button critButton;

    


    
    public void UpdateLvUI(string content)
    {
        lvUI.text ="LV:"+content;
    }
    public void UpdatePointStatUI(string content)
    {
        pointStatUI.text = "POINT:"+content;
    }

    public void UpdateHealthStatUI(string content)
    {
        healthStatUI.text = "Health: "+content;
    }
    public void UpdateAtkStatUI(string content)
    {
        atkStatUI.text = "Attack: " + content;
    }
    public void UpdateCritRateStatUI(string content)
    {
        critRateStatUI.text = "Critical: "+content;
    }

    public void ClearEventButton()
    {
        hpButton.onClick.RemoveAllListeners();
        atkButton.onClick.RemoveAllListeners();
        critButton.onClick.RemoveAllListeners();
    }

    public void SetActionHpButton(Action action)
    {
        hpButton.onClick.RemoveAllListeners();
        
        hpButton.onClick.AddListener(() =>
        {
            if(action != null)
            {
                action.Invoke();
            }
        });
    }

    public void SetActionAtkButton(Action action)
    {
        atkButton.onClick.RemoveAllListeners();
        
        atkButton.onClick.AddListener(() =>
        {
            if(action != null)
            {
                action.Invoke();
            }
        });
    }

    public void SetActionCritButton(Action action)
    {
        critButton.onClick.RemoveAllListeners();
        
        critButton.onClick.AddListener(() =>
        {
            if(action != null)
            {
                action.Invoke();
            }
        });
    }


    
}
