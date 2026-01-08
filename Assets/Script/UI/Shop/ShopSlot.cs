using System.Collections;
using System.Collections.Generic;

using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;

    [SerializeField] private TextMeshProUGUI price;

    [SerializeField] private TextMeshProUGUI nameItem;

    [SerializeField] private int indexItem;

    [SerializeField] private GameObject borderChosen;

    [SerializeField] private GameObject bgChosen;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UIManageMent.Instance.ShopSystem.BuySystem.TurnOnMultiBuy(indexItem);
            // Mo panel click
            
        }
        
    }
    public void SetInfo(int _index)
    {

        indexItem = _index;
        icon.sprite = GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].Icon;
        price.text = GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].Value.ToString() +"g";
        nameItem.text =  GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].ItemName;
    }

    public void OnChosen()
    {
        borderChosen.SetActive(true);
        bgChosen.SetActive(true);
    }

    public void OnNotChosen(){
        borderChosen.SetActive(false);

        bgChosen.SetActive(false);
    }

    private void UpdateSelectedState(int _index)
    {
        if(_index != indexItem)
        {
            OnNotChosen();
        }
        else
        {
            OnChosen();
        }
    }
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
        if(UIManageMent.Instance.ShopSystem.BuySystem != null)
        {
            UIManageMent.Instance.ShopSystem.BuySystem.OnSelectedItemChanged += UpdateSelectedState;
        }
       
    }
    public void TurnOff()
    {
        if(UIManageMent.Instance.ShopSystem.BuySystem != null)
        {
            UIManageMent.Instance.ShopSystem.BuySystem.OnSelectedItemChanged -= UpdateSelectedState;
        }
    
        this.gameObject.SetActive(false);
    }

    

}
