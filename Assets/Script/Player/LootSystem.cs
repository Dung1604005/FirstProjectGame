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
            if (!GameManageMent.Instance.Interacting)
            {
                GameManageMent.Instance.SetCurSorInteract();
            }
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
                
            }

            else
            {
                
            }

            
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
            
        }

    }
}
