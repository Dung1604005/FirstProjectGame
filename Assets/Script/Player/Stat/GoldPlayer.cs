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
        var gradient = new TMPro.VertexGradient(
    new Color32(0xFF, 0xAE, 0xAE, 0xFF), // top-left (Đỏ hồng sáng - Highlight)
    new Color32(0xFF, 0xAE, 0xAE, 0xFF), // top-right (Đỏ hồng sáng - Highlight)
    new Color32(0xB0, 0x00, 0x05, 0xFF), // bottom-left (Đỏ huyết đậm - Shadow)
    new Color32(0xFF, 0x33, 0x33, 0xFF)  // bottom-right (Đỏ tươi - Main Color)
);
        int randomOffsetY = Random.Range(0, 6);
        int randomOffsetX = Random.Range(-3,3);
        GameManageMent.Instance.PoolManager.FloatingTextPool.Spawn(Camera.main.WorldToScreenPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)+ new Vector3(randomOffsetX/2f, randomOffsetY/2, 0f) )).SetUp("-" + price.ToString() + " GOLD", Color.yellow, gradient);
        UIManageMent.Instance.SetGoldText(curGold.ToString());
    }
    public void AddGold(int amount)
    {
        curGold += amount;
        UIManageMent.Instance.SetGoldText(curGold.ToString());
        if(amount <= 0)
        {
            return;
        }
        var gradient = new TMPro.VertexGradient(
            new Color32(0xFF, 0xF8, 0xC5, 0xFF), // top-left
            new Color32(0xFF, 0xF8, 0xC5, 0xFF), // top-right
            new Color32(0xE5, 0xA1, 0x00, 0xFF), // bottom-left
            new Color32(0xFF, 0xD8, 0x4A, 0xFF)  // bottom-right
            );
            int randomOffsetY = Random.Range(0, 6);
            int randomOffsetX = Random.Range(-3,3);

        
        GameManageMent.Instance.PoolManager.FloatingTextPool.Spawn(Camera.main.WorldToScreenPoint((Vector3)GameManageMent.Instance.PlayerManager.PlayerController.getPos()+ new Vector3(randomOffsetX/2f, randomOffsetY/2, 0f) )).SetUp("+" + amount.ToString() + " GOLD", Color.yellow, gradient);
        
        
        
    }
    void Start()
    {
        UIManageMent.Instance.SetGoldText(curGold.ToString());
    }


}
