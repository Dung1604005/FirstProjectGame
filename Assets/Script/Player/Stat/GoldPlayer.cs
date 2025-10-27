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

        var gradient = new TMPro.VertexGradient(
            new Color32(0xFF, 0xF8, 0xC5, 0xFF), // top-left
            new Color32(0xFF, 0xF8, 0xC5, 0xFF), // top-right
            new Color32(0xE5, 0xA1, 0x00, 0xFF), // bottom-left
            new Color32(0xFF, 0xD8, 0x4A, 0xFF)  // bottom-right
            );
            int randomOffsetY = Random.Range(0, 6);
            int randomOffsetX = Random.Range(-3,3);

        
        GameManageMent.Instance.PoolManager.FloatingTextPool.Spawn(Camera.main.WorldToScreenPoint((Vector3)GameManageMent.Instance.PlayerManager.PlayerController.getPos()+ new Vector3(randomOffsetX/2f, randomOffsetY/2, 0f) )).SetUp("+" + amount.ToString() + " GOLD", Color.yellow, gradient);
        
        curGold += amount;
        UIManageMent.Instance.SetGoldText(curGold.ToString());
    }
    void Start()
    {
        UIManageMent.Instance.SetGoldText(curGold.ToString());
    }


}
