using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSystemUI : MonoBehaviour
{
    private EquipMentSystem equipMentSystem;
    public EquipMentSystem EquipMentSystem => equipMentSystem;

    private List<EquipmentSlotUI> slots = new List<EquipmentSlotUI>();

    public List<EquipmentSlotUI> Slots => slots;
    [SerializeField] private EquipmentSlotUI equipmentSlotUI;

    public void SetData(EquipMentSystem _equipMentSystem)
    {
        equipMentSystem = _equipMentSystem;

        equipMentSystem.OnEquipmentChange += ReFresh;

    }
    public void GenSlot()
    {
        for (int i = 0; i < equipMentSystem.SizeEquipMent; i++)
        {
            var slot = Instantiate(equipmentSlotUI, this.transform);
            slots.Add(slot);
            slots[i].UpdateUI(null, i);
        }
    }

    public void ReFresh()
    {
        for (int i = 0; i < equipMentSystem.SizeEquipMent; i++)
        {
            
            if (equipMentSystem.Slots[i].ItemData != null)
            {
                slots[i].UpdateUI(equipMentSystem.Slots[i].ItemData.Icon, i);
            }
            else
            {
                slots[i].UpdateUI(null, i);
            }
        }
    }


}
