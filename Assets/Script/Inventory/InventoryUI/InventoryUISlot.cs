using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;



public class InventoryUISlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;



    private InventoryUI inventoryUI;


    private int index;
    public int Index => index;

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemData check = inventoryUI.GetSlotData(index);
        if (check == null)
        {
            return;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Mo panel click

            ItemData item = inventoryUI.GetSlotData(index);

            Debug.Log("CLICK");
            inventoryUI.TurnOnPanelClick();
            if (item.Type == ItemType.Melee || item.Type == ItemType.Gun)
            {

                int damage = (item as WeaponData).Damaged;
                float cd = (item as WeaponData).CoolDown;
                string Stat = "DAME:" + damage.ToString() + "\n" + "CD:" + cd.ToString();


                inventoryUI.UpdatePanelClick(item.Icon, item.Description, item.ItemName, Stat);
            }
            else
            {
                inventoryUI.UpdatePanelClick(item.Icon, item.Description, item.ItemName);
            }



        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Mo menu context

            inventoryUI.ContextMenu.UpdateIndex(index);
            inventoryUI.UpdatePosContextMenu(gameObject.GetComponent<Image>().rectTransform.anchoredPosition);


        }
    }

    // Lay data khi bat dau drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        ItemData item = inventoryUI.GetSlotData(index);
        if (item != null)
        {
            // Phai set data cho eventData thi moi co OnDrop
            eventData.pointerDrag = gameObject;
            inventoryUI.SetDragIcon(image.sprite);

        }


    }
    //Lay data khi ket thuc drag
    public void OnEndDrag(PointerEventData eventData)
    {
        inventoryUI.EndDragIcon();

    }
    // Layy data khi dang drag
    public void OnDrag(PointerEventData eventData)
    {
        if (eventData != null)
        {

            inventoryUI.MoveDragIcon(eventData.position);
        }

    }
    // Lay data khi tha
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData != null)
        {
            inventoryUI.Inven.Swap(index, eventData.pointerDrag.GetComponent<InventoryUISlot>().Index);
        }
        Debug.Log("DROP");
    }

    public void Set(ItemData item, int amount, int _index)
    {
        index = _index;
        if (item == null || amount == 0)
        {

            //Tro nen trong suot
            //Icon
            image.gameObject.SetActive(false);
            text.gameObject.SetActive(false);

            return;
        }
        else
        {

            //Gan lai mau va Icon
            image.gameObject.SetActive(true);
            image.sprite = item.Icon;

            // Gan so luong
            text.gameObject.SetActive(true);
            string count = amount.ToString();
            if (amount < 10)
            {
                count = "0" + count;
            }
            text.text = count;
        }

    }
    void Start()
    {
        inventoryUI = this.transform.parent.GetComponent<InventoryUI>();

    }
}
