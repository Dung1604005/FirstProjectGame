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

    [SerializeField] private int indexItem;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Mo panel click

            ItemData item = GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem];

            UIManageMent.Instance.InventoryUI.TurnOnPanelClick();
            if (item.Type == ItemType.Melee || item.Type == ItemType.Gun)
            {

                int damage = (item as WeaponData).Damaged;
                float cd = (item as WeaponData).CoolDown;
                string Stat = "DAME:" + damage.ToString() + "\n" + "CD:" + cd.ToString();


                UIManageMent.Instance.InventoryUI.UpdatePanelClick(item.Icon, item.Description, item.ItemName, Stat);
            }
            else
            {
                UIManageMent.Instance.InventoryUI.UpdatePanelClick(item.Icon, item.Description, item.ItemName);
            }



        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            UIManageMent.Instance.ShopSystem.BuySystem.TurnOnMultiBuy(indexItem);
        }
    }
    public void SetInfo(int _index)
    {

        indexItem = _index;
        icon.sprite = GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].Icon;
        price.text = GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem].Value.ToString();
    }

    

}
