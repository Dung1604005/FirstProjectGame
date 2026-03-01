using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class StatPlayer : MonoBehaviour
{

    [SerializeField] private float maxHP;
    public float MaxHP => maxHP;
    [SerializeField] private float healthGrowth;
    public float HealthGrowth => healthGrowth;

    private int pointMaxHp = 0;
    public int PointMaxHp => pointMaxHp;
    [SerializeField] private float atk;
    public float Atk => atk;

    private int pointAtk = 0;
    public int PointAtk => pointAtk;
    [SerializeField] private float atkGrowth;
    public float AtkGrowth => atkGrowth;
    [SerializeField] private float speed;
    public float Speed => speed;

    [SerializeField] private int critRate;

    public int CritRate => critRate;

    private int pointCritRate = 0;
    public int PointCritRate => pointCritRate;

    [SerializeField] private int critRateGrowth;

    public int CritRateGrowth => critRateGrowth;

    [SerializeField] private float critDamagePercentage;
    public float CritDamagePercentage => critDamagePercentage;

    public bool CheckEnoughPoint()
    {
        if (GameManageMent.Instance.PlayerManager.ExpSystem.PointStat > 0)
        {
            return true;

        }
        else
        {
            return false;

        }
    }
    public void UpgradeHP()
    {
        if (CheckEnoughPoint())
        {
            GameManageMent.Instance.PlayerManager.ExpSystem.UsePoint();
            maxHP += healthGrowth;
            pointMaxHp += 1;
            UIManageMent.Instance.ExpStatSystemUI.UpdateHealthStatUI(maxHP.ToString());
            GameManageMent.Instance.PlayerManager.Health.SetMaxHp(maxHP);
        }

    }
    public void UpgradeAtk()
    {
        if (CheckEnoughPoint())
        {
            GameManageMent.Instance.PlayerManager.ExpSystem.UsePoint();
            atk += atkGrowth;
            pointAtk += 1;
            UIManageMent.Instance.ExpStatSystemUI.UpdateAtkStatUI(atk.ToString());
        }

    }
    public void UpgradeCritRate()
    {
        if (CheckEnoughPoint())
        {
            GameManageMent.Instance.PlayerManager.ExpSystem.UsePoint();
            critRate += critRateGrowth;
            pointCritRate += 1;
            UIManageMent.Instance.ExpStatSystemUI.UpdateCritRateStatUI(critRate.ToString());
        }

    }
    public void LoadData(int savedPointMaxHp, int savedPointAtk, int savedPointCritRate)
    {
        maxHP += savedPointMaxHp * healthGrowth;
        pointMaxHp = savedPointMaxHp;

        atk += savedPointAtk * atkGrowth;
        pointAtk = savedPointAtk;

        critRate += savedPointCritRate * critRateGrowth;
        pointCritRate = savedPointCritRate;

        UIManageMent.Instance.ExpStatSystemUI.UpdateHealthStatUI(maxHP.ToString());
        UIManageMent.Instance.ExpStatSystemUI.UpdateAtkStatUI(atk.ToString());
        UIManageMent.Instance.ExpStatSystemUI.UpdateCritRateStatUI(critRate.ToString());
    }

    void Start()
    {
        UIManageMent.Instance.ExpStatSystemUI.UpdateHealthStatUI(maxHP.ToString());
        UIManageMent.Instance.ExpStatSystemUI.UpdateAtkStatUI(atk.ToString());
        UIManageMent.Instance.ExpStatSystemUI.UpdateCritRateStatUI(critRate.ToString());

        UIManageMent.Instance.ExpStatSystemUI.ClearEventButton();
         UIManageMent.Instance.ExpStatSystemUI.SetActionHpButton(UpgradeHP);
        UIManageMent.Instance.ExpStatSystemUI.SetActionAtkButton(UpgradeAtk);
        UIManageMent.Instance.ExpStatSystemUI.SetActionCritButton(UpgradeCritRate);
        
        
    }

    


}
