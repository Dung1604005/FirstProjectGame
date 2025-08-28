using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPlayer : MonoBehaviour
{
    [SerializeField] private int curGold;
    public int CurGold => curGold;

    public bool TryBuy(int price)
    {
        if (price > curGold)
        {
            return false;
        }
        return true;
    }
    public void Buy(int price)
    {
        curGold -= price;
        UIManageMent.Instance.SetGoldText(curGold.ToString());
    }
    public void AddGold(int amount)
    {
        curGold += amount;
        UIManageMent.Instance.SetGoldText(curGold.ToString());
    }
    void Start()
    {
        UIManageMent.Instance.SetGoldText(curGold.ToString());
    }


}
