using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
   

    
    [SerializeField] private float offSetX;
    public float OffSetX => offSetX;
    [SerializeField] private float offSetY;
    public float OffSetY => offSetY;


    private int index;
    public int Index => index;
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
                UIManageMent.Instance.InventoryUI.Inven.Remove(index, amount);
            }
            


        } 
        
        TurnOff();

    }
    public void UpdateIndex(int _index)
    {
        index = _index;
    }
    public void Drop()
    {
        UIManageMent.Instance.InventoryUI.Inven.Remove(index, 1);
        TurnOff();
    }
    public void Sell()
    {
        TurnOff();

    }
    void Start()
    {
        TurnOff();   
    }
}
