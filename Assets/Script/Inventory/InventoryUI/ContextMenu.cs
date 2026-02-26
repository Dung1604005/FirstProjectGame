using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ContextMenuUI : MonoBehaviour
{
   

    
    [SerializeField] private float offSetX;
    public float OffSetX => offSetX;
    [SerializeField] private float offSetY;
    public float OffSetY => offSetY;


    private int index;
    public int Index => index;
    private int price;
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
    }
    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
    public void Equip()
    {
        ItemData itemData = UIManageMent.Instance.InventoryUI.Inven.GetSlotData(index).ItemData;
        int amount = UIManageMent.Instance.InventoryUI.Inven.GetSlotData(index).Count;
        if (itemData != null)
        {
            
            if (UIManageMent.Instance.EquipmentSystemUI.EquipMentSystem.TryEquip(itemData, amount)) {
                UIManageMent.Instance.InventoryUI.Inven.RemoveByIndex(index, amount, true);
            }
            


        } 
        
        TurnOff();

    }
    public void UpdateIndex(int _index)
    {
        index = _index;
        price = UIManageMent.Instance.InventoryUI.Inven.GetSlotData(index).ItemData.Value / 2;
    }
    public void Drop()
    {
        if(UIManageMent.Instance.InventoryUI.Inven.GetSlotData(index).ItemData.Type == ItemType.QuestItem)
        {
            UIManageMent.Instance.UpdateWarning(GameConfig.CANT_REMOVE_QUEST_ITEM);
            UIManageMent.Instance.TurnOnWarning();
            return;
        }
        UIManageMent.Instance.InventoryUI.Inven.RemoveByIndex(index, 1);
        TurnOff();
    }
    public void Sell()
    {
        if(UIManageMent.Instance.InventoryUI.Inven.GetSlotData(index).ItemData.Type == ItemType.QuestItem)
        {
            UIManageMent.Instance.UpdateWarning(GameConfig.CANT_REMOVE_QUEST_ITEM);
            UIManageMent.Instance.TurnOnWarning();
            return;
        }
        GameManageMent.Instance.PlayerManager.Gold.AddGold(price);
        UIManageMent.Instance.InventoryUI.Inven.RemoveByIndex(index, 1);
        TurnOff();

    }
    void Start()
    {
        TurnOff();   
    }
}
