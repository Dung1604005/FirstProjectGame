using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    private int index;
    public int Index => index;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (icon == null)
            {
                return;
            }

            
            UIManageMent.Instance.EquipmentSystemUI.EquipMentSystem.TryUnEquip(index);
        }
    }
    public EquipmentSlotUI(Sprite _icon, int _index)
    {
        icon.sprite = _icon;
        index = _index;
    }
    public void UpdateUI(Sprite _icon, int _index)
    {
        if (_icon == null)
        {
            icon.gameObject.SetActive(false);
        }
        else
        {
            icon.gameObject.SetActive(true);
        }
        icon.sprite = _icon;
        index = _index;
    }

}
