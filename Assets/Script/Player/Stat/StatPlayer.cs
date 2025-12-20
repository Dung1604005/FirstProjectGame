using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPlayer : MonoBehaviour
{

    [SerializeField] private float maxHP;
    public float MaxHP => maxHP;
    [SerializeField] private float healthGrowth;
    public float HealthGrowth => healthGrowth;
    [SerializeField] private float atk;
    public float Atk => atk;
    [SerializeField] private float atkGrowth;
    public float AtkGrowth => atkGrowth;
    [SerializeField] private float speed;
    public float Speed => speed;
    [SerializeField] private float speedGrowth;
    public float SpeedGrowth => speedGrowth;

    [SerializeField] private int critRate;

    public int CritRate => critRate;

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
            UIManageMent.Instance.ExpStatSystemUI.UpdateAtkStatUI(atk.ToString());
        }

    }
    public void UpgradeCritRate()
    {
        if (CheckEnoughPoint())
        {
            GameManageMent.Instance.PlayerManager.ExpSystem.UsePoint();
            critRate += critRateGrowth;
            UIManageMent.Instance.ExpStatSystemUI.UpdateCritRateStatUI(speed.ToString());
        }

    }
    void Start()
    {
        UIManageMent.Instance.ExpStatSystemUI.UpdateHealthStatUI(maxHP.ToString());
        UIManageMent.Instance.ExpStatSystemUI.UpdateAtkStatUI(atk.ToString());
        UIManageMent.Instance.ExpStatSystemUI.UpdateCritRateStatUI(critRate.ToString());
    }


}
