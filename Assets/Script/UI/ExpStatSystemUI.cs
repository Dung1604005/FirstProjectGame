using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpStatSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthStatUI;
    [SerializeField] private TextMeshProUGUI atkStatUI;
    [SerializeField] private TextMeshProUGUI speedStatUI;

    [SerializeField] private TextMeshProUGUI lvUI;

    [SerializeField] private TextMeshProUGUI pointStatUI;
    public void TurnOn()
    {
        
        this.gameObject.SetActive(true);
    }
    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
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
        healthStatUI.text = content;
    }
    public void UpdateAtkStatUI(string content)
    {
        atkStatUI.text = content;
    }
    public void UpdateSpeedStatUI(string content)
    {
        speedStatUI.text = content;
    }
}
