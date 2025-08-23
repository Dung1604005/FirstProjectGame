using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExpSystem : MonoBehaviour
{
    [SerializeField] private int lv;
    public int Lv => lv;
    [SerializeField] private float expToLvUp;
    public float ExpToLvUp => expToLvUp;

    [SerializeField] private float offSetNextExpLvUp;
    [SerializeField] private float curExp;
    public float CurExp => curExp;

    private int pointStat;
    public int PointStat => pointStat;

    [SerializeField] private int pointPerLvUp;

    public UnityEvent<float, float> OnExpChange = new UnityEvent<float, float>();

    
    public void UsePoint()
    {
        pointStat -= 1;
        UIManageMent.Instance.ExpStatSystemUI.UpdatePointStatUI(pointStat.ToString());
    }
    public void LvUp()
    {
        while (curExp > expToLvUp)
        {
            pointStat += pointPerLvUp;
            curExp -= expToLvUp;
            expToLvUp += offSetNextExpLvUp;
            lv += 1;
        }
        UIManageMent.Instance.ExpStatSystemUI.UpdateLvUI(lv.ToString());
        UIManageMent.Instance.ExpStatSystemUI.UpdatePointStatUI(pointStat.ToString());

    }
    public void GainExp(float addExp)
    {
        curExp += addExp;
        LvUp();
    }
    void Start()
    {
        lv = 1;
        curExp = 0;
        pointStat = 0;
        UIManageMent.Instance.ExpStatSystemUI.UpdateLvUI(lv.ToString());
        UIManageMent.Instance.ExpStatSystemUI.UpdatePointStatUI(pointStat.ToString());
    }

}
