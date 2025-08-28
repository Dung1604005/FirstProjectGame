using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameShop;
    [SerializeField] private BuySystem buySystem;
    public BuySystem BuySystem => buySystem;
    [SerializeField] private List<ShopSlot> shops;
    public List<ShopSlot> Shops => shops;

    [SerializeField] private int sizeShop;

    [SerializeField] private ShopSlot prefabSlot;




    void Start()
    {
        shops = new List<ShopSlot>();
        for (int i = 0; i < sizeShop; i++)
        {
            
            shops.Add(Instantiate(prefabSlot, this.transform));
        }
        
        Refresh();
    }
    public void Refresh()
    {

        for (int i = 0; i < sizeShop; i++)
        {
            int rand = UnityEngine.Random.Range(0, GameManageMent.Instance.ItemDataBase.ItemDatas.Count);
            shops[i].SetInfo(rand);
        }
    }
    public void TurnOn()
    {
        nameShop.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
    }
    public void TurnOff()
    {
        nameShop.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
