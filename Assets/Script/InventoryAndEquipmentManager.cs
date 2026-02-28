using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAndEquipmentManager : MonoBehaviour
{

    // Quan li trao doi giua runtime script va UI trong inventory
    private EquipMentSystem equipMentSystem;
    public EquipMentSystem EquipMentSystem => equipMentSystem;
    [Header("Equipment")]
    [SerializeField] private EquipmentSystemUI equipmentSystemUI;
    [SerializeField] private int equipSize;
    
    public EquipmentSystemUI EquipmentSystemUI => equipmentSystemUI;
    private InventorySystem inventorySystem;

    public InventorySystem InventorySystem => inventorySystem;
    [Header("Inventory")]
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private int inventorySize;



    void Awake()
    {
        equipMentSystem = new EquipMentSystem(equipSize);
        equipmentSystemUI.SetData(equipMentSystem);
        equipmentSystemUI.GenSlot();
        
        inventorySystem = new InventorySystem(inventorySize);
        inventoryUI.SetInventory(inventorySystem);

        inventoryUI.GenerateSlot(inventorySystem.All_Slots.Count);
    }
    void Update()
    {
        
        
       
    }
}
