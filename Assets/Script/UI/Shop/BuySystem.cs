using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class BuySystem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;

    [SerializeField] private TextMeshProUGUI amount;
    private int amountNumb;

    [SerializeField] private int limit;

    [SerializeField] private TextMeshProUGUI price;
    private int priceNumb;
    private int index;
    public int Index => index;

    [SerializeField] private GameObject confirmPopup;

    [SerializeField] private Image avaItemPopUp;

    [SerializeField] private TextMeshProUGUI priceItemPopUp;

    [SerializeField] private TextMeshProUGUI  itemNamePopUp;

    public event Action<int> OnSelectedItemChanged;

    public void TurnOnMultiBuy(int _index)
    {
        this.gameObject.SetActive(true);
        index = _index;
        icon.sprite = GameManageMent.Instance.ItemDataBase.ItemDatas[_index].Icon;
        itemName.text = GameManageMent.Instance.ItemDataBase.ItemDatas[_index].ItemName;
        amountNumb = 1;
        priceNumb = GameManageMent.Instance.ItemDataBase.ItemDatas[_index].Value;
        amount.text = amountNumb.ToString();
        price.text = priceNumb.ToString() +"g";
        limit = GameManageMent.Instance.ItemDataBase.ItemDatas[_index].MaxStack;
        OnSelectedItemChanged?.Invoke(_index);
    }
    public void AddItem()
    {
        if (amountNumb >= limit)
        {
            return;
        }
        priceNumb += GameManageMent.Instance.ItemDataBase.ItemDatas[index].Value;
        amountNumb += 1;
        amount.text = amountNumb.ToString();
        price.text = priceNumb.ToString() + "g";
    }
    public void MinusItem()
    {
        if (amountNumb <= 1)
        {
            return;
        }
        priceNumb -= GameManageMent.Instance.ItemDataBase.ItemDatas[index].Value;
        amountNumb -= 1;
        amount.text = amountNumb.ToString();
        price.text = priceNumb.ToString() + "g";
    }
    public void Buy()
    {
        if (GameManageMent.Instance.PlayerManager.Gold.TryBuy(priceNumb))
        {
            if (UIManageMent.Instance.InventoryUI.Inven.TryAdd(GameManageMent.Instance.ItemDataBase.ItemDatas[index], amountNumb))
            {
                GameManageMent.Instance.PlayerManager.Gold.Buy(priceNumb);
                UIManageMent.Instance.InventoryUI.Inven.Add(GameManageMent.Instance.ItemDataBase.ItemDatas[index], amountNumb);
                
            }

        }
        confirmPopup.SetActive(false);

    }
    public void Cancel()
    {
        gameObject.SetActive(false);
    }
    public void TurnOnBuyPopUp()
    {
        confirmPopup.SetActive(true);
        avaItemPopUp.sprite = icon.sprite;
        priceItemPopUp.text = price.text;
        itemNamePopUp.text = itemName.text + "x" + amount.text;
    }
    public void TurnOffBuyPopUp()
    {
        confirmPopup.SetActive(false);
    }
    void Start()
    {
        
    }
}
