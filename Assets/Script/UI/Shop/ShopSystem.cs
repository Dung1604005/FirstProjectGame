using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.VersionControl;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    
    [SerializeField] private BuySystem buySystem;
    public BuySystem BuySystem => buySystem;
    [SerializeField] private List<ShopSlot> shops;
    public List<ShopSlot> Shops => shops;

    [SerializeField] private int sizeShop;

    [SerializeField] private ShopSlot prefabSlot;

    [SerializeField] private RectTransform contentRectTransform;

    [SerializeField] private float cellSizeX;


    [SerializeField] private float cellSizeY;




    void Awake()
    {
        shops = new List<ShopSlot>();
         for(int i =  0; i < 20; i++)
        {
            shops.Add(Instantiate(prefabSlot, contentRectTransform.transform));
            
        }
    
    }
    public void SetItemShop(List<int> listItemIndex)
    {
        
        //Reset cua hang
        for(int i =  0; i < 20; i++)
        {
            shops[i].TurnOff();
        }
        sizeShop = listItemIndex.Count  ;
        float contentHeight = cellSizeY * sizeShop + 5f*(sizeShop + 2);
        contentRectTransform.sizeDelta = new Vector2(contentRectTransform.sizeDelta.x, contentHeight);
        for(int i = 0; i < sizeShop; i++){
            shops[i].TurnOn();
            shops[i].SetInfo(listItemIndex[i]);
        }
        // mac dinh ban dau la item1
        buySystem.TurnOnMultiBuy(listItemIndex[0]);

    }
    
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
        
    }
    public void TurnOff()
    {
        
        this.gameObject.SetActive(false);
    }

    
    
}
