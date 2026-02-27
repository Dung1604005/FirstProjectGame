using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExpSystem : MonoBehaviour
{
    [SerializeField] private int lv;
    public int Lv => lv;
    [SerializeField] private int expToLvUp;
    public int ExpToLvUp => expToLvUp;

    [SerializeField] private int offSetNextExpLvUp;
    [SerializeField] private int curExp;
    public int CurExp => curExp;

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
        OnExpChange?.Invoke(curExp, expToLvUp);

    }
    public void GainExp(int addExp)
    {
       
        curExp += addExp;
         var gradient = new TMPro.VertexGradient(
    new Color32(0xD6, 0xFF, 0xF7, 0xFF), // top-left   (#D6FFF7)
    new Color32(0xD6, 0xFF, 0xF7, 0xFF), // top-right  (#D6FFF7)
    new Color32(0x18, 0xE6, 0xFF, 0xFF), // bottom-left (#18E6FF)
    new Color32(0x5B, 0xFF, 0xB4, 0xFF)  // bottom-right (#5BFFB4)
);
            int randomOffsetY = Random.Range(0, 6);
            int randomOffsetX = Random.Range(-3,3);

        
        GameManageMent.Instance.PoolManager.FloatingTextPool.Spawn(Camera.main.WorldToScreenPoint((Vector3)GameManageMent.Instance.PlayerManager.PlayerController.getPos()+ new Vector3(randomOffsetX/2f, randomOffsetY/2, 0f) )).SetUp("+" + addExp.ToString() + " EXP", Color.white, gradient);   
        
        LvUp();
    }
    public void LoadData(int savedLv, int savedExpToLvUp, int savedPointStat)
    {
        lv = savedLv;
        expToLvUp = savedExpToLvUp;
        pointStat = savedPointStat;
        UIManageMent.Instance.ExpStatSystemUI.UpdateLvUI(lv.ToString());
        UIManageMent.Instance.ExpStatSystemUI.UpdatePointStatUI(pointStat.ToString());
        OnExpChange?.Invoke(curExp, expToLvUp);
    }

    void Start()
    {
        lv = 1;
        curExp = 0;
        pointStat = 0;
        UIManageMent.Instance.ExpStatSystemUI.UpdateLvUI(lv.ToString());
        UIManageMent.Instance.ExpStatSystemUI.UpdatePointStatUI(pointStat.ToString());
        UIManageMent.Instance.SetExpBar(0f, 1f);
    }

}
