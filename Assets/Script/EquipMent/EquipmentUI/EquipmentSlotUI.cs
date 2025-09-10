using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Image backGround;
    [SerializeField] private Sprite select;
    [SerializeField] private Sprite unSelect;

    [SerializeField] private TextMeshProUGUI amountText;
    
    private int index;
    public int Index => index;

    public void Awake()
    {
        backGround = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (icon == null || UIManageMent.Instance.EquipmentSystemUI.EquipMentSystem.GetItemData(index) == null)
            {
                return;
            }



            UIManageMent.Instance.EquipmentSystemUI.EquipMentSystem.TryUnEquip(index);
            if (index == GameManageMent.Instance.PlayerManager.PlayerController.SlotPlayerController.CurSlotEquip)
            {
                GameManageMent.Instance.PlayerManager.PlayerController.SlotPlayerController.UnEquipSlot();
            }
        }
    }
    public EquipmentSlotUI(Sprite _icon, int _index)
    {
        icon.sprite = _icon;
        index = _index;
    }
    public void SelectSlot()
    {
        backGround.sprite = select;
    }
    public void UnSelectSlot()
    {
        backGround.sprite = unSelect;
    }
    public void UpdateUI(Sprite _icon, int _amount, int _index)
    {
        if (_icon == null)
        {
            icon.gameObject.SetActive(false);
        }
        else
        {
            icon.gameObject.SetActive(true);
        }
        if (_amount > 0)
        {
            amountText.text = "x" + _amount.ToString();
        }
        else
        {
            amountText.text = "";
        }
        icon.sprite = _icon;
        icon.type = Image.Type.Simple;
        icon.preserveAspect = true;
        index = _index;
    }

}
