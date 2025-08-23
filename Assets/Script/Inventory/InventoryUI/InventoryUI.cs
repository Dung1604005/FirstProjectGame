using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private ContextMenu contextMenu;
    public ContextMenu ContextMenu => contextMenu;

    [SerializeField] private PanelClickUI panelClickUI;
    public PanelClickUI PanelClickUI => panelClickUI;
    [SerializeField] private Image dragIcon;
    public Image DragIcon => dragIcon;
    [SerializeField] private InventoryUISlot slotUI;

    private InventorySystem inven;

    public InventorySystem Inven => inven;

    List<InventoryUISlot> inventoryUISlots = new List<InventoryUISlot>();
    public List<InventoryUISlot> InventoryUISlots => inventoryUISlots;
    public InventoryUISlot SlotUI => slotUI;
    // xu li panel click
    
    public void UpdatePosContextMenu(Vector3 pos)
    {
        pos.x += contextMenu.OffSetX;
        pos.y += contextMenu.OffSetY;
        contextMenu.gameObject.GetComponent<Image>().rectTransform.anchoredPosition = pos;
        contextMenu.TurnOn();
    }
    public void UpdatePanelClick(Sprite Icon, string Description, string NameItem, string Stat = null)
    {
        panelClickUI.UpdateContent(Icon, Description, NameItem, Stat);
    }

    public void TurnOnPanelClick()
    {
        panelClickUI.TurnOn();
    }

    public void TurnOffPanelClick()
    {
        panelClickUI.TurnOff();
    }

    // Xu li drag icon
    public void SetDragIcon(Sprite _image)
    {
        dragIcon.GetComponent<CanvasGroup>().blocksRaycasts = false;
        dragIcon.gameObject.SetActive(true);
        dragIcon.sprite = _image;
    }
    public void MoveDragIcon(Vector3 pos)
    {
        dragIcon.rectTransform.position = pos;
    }
    public void EndDragIcon()
    {
        dragIcon.GetComponent<CanvasGroup>().blocksRaycasts = true;
        dragIcon.gameObject.SetActive(false);
        dragIcon.sprite = null;
    }
    public ItemData GetSlotData(int index)
    {
        return inven.GetSlotData(index).ItemData;
    }
    public void SetInventory(InventorySystem inventory)
    {
        inven = inventory;

        // Moi lan inventory bi thay doi se refresh
        inven.OnChangeInventory += Refresh;
    }
    public void Refresh()
    {
        
        for (int i = 0; i < inven.All_Slots.Count; i++)
        {
            // Gan gia tri Icon va amount vao moi slotUI
            
            inventoryUISlots[i].Set(inven.All_Slots[i].ItemData, inven.All_Slots[i].Count, i);
        }
    }
    
    public void GenerateSlot(int size)
    {
        // Generate slotUI
        for (int i = 0; i < size; i++)
        {
            var slotui = Instantiate(slotUI, this.transform);
            inventoryUISlots.Add(slotui);

        }
        dragIcon = Instantiate(dragIcon, this.transform);
        dragIcon.GetComponent<CanvasGroup>().blocksRaycasts = true;
        for (int i = 0; i < size; i++)
        {
            inventoryUISlots[i].Set(null, 0, i);
        }

    }

    public void TurnOn()
    {
        this.gameObject.SetActive(true);
    }
    public void TurnOff()
    {
        panelClickUI.TurnOff();
        this.gameObject.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
