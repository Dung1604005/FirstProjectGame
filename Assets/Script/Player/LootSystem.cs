using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSystem : MonoBehaviour
{
    [SerializeField] private LootItem lastFloatingObject;

    public LootItem LastFloatingObject => lastFloatingObject;
    public void CheckHoverItem()
    {
        LayerMask itemMask = LayerMask.GetMask(GameConfig.ITEM_MASK);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos, itemMask);
        if (hit != null)
        {
            if (lastFloatingObject != null)
            {
                lastFloatingObject.SetStateHover(false);
            }
            ItemData itemHover = hit.gameObject.GetComponent<LootItem>().GetItemData();
            lastFloatingObject = hit.gameObject.GetComponent<LootItem>();
            lastFloatingObject.SetStateHover(true);
            if (itemHover.Type == ItemType.Gun || itemHover.Type == ItemType.Melee)
            {
                WeaponData weaponHover = itemHover as WeaponData;

                int damage = weaponHover.Damaged;
                float cd = weaponHover.CoolDown;
                string Stat = "DAME:" + damage.ToString() + "\n" + "CD:" + cd.ToString();
                UIManageMent.Instance.InventoryUI.UpdatePanelClick(itemHover.Icon, itemHover.Description, itemHover.name, Stat);
            }

            else
            {
                UIManageMent.Instance.InventoryUI.UpdatePanelClick(itemHover.Icon, itemHover.Description, itemHover.name);
            }

            UIManageMent.Instance.InventoryUI.TurnOnPanelClick();
            if (Input.GetKeyDown(KeyCode.E))
            {

                hit.gameObject.GetComponent<LootItem>().PickUp();
            }

        }
        else
        {

            if (lastFloatingObject != null)
            {
                lastFloatingObject.SetStateHover(false);
            }
            lastFloatingObject = null;
            if (UIManageMent.Instance.InventoryUI.PanelClickUI.isActiveAndEnabled)
            {
                UIManageMent.Instance.InventoryUI.TurnOffPanelClick();
            }
        }

    }
}
