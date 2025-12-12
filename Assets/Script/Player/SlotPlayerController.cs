using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPlayerController : MonoBehaviour
{
    [SerializeField] private int curSlotEquip;
    public int CurSlotEquip => curSlotEquip;

    [SerializeField] private Weapon weapon;
    public Weapon Weapon => weapon;

    private GameObject weaponPrefab;

    
    

    private bool usingWeapon = false;

    public bool UsingWeapon => usingWeapon;
    public void EquipSlot(int slot)
    {
        UIManageMent.Instance.BulletUIController.TurnOffBulletUI();

        ItemData itemData = UIManageMent.Instance.EquipmentSystemUI.EquipMentSystem.Slots[slot].ItemData;
        ItemData lastItemData = UIManageMent.Instance.EquipmentSystemUI.EquipMentSystem.Slots[curSlotEquip].ItemData;
        if (weapon != null)
        {
            weapon = null;
            Destroy(weaponPrefab);
        }
        if (lastItemData != null && lastItemData.Type == ItemType.Buildable)
        {
            GameManageMent.Instance.BuildManager.TurnOffBuildMode();
        }
        UIManageMent.Instance.EquipmentSystemUI.Slots[curSlotEquip].UnSelectSlot();
        UIManageMent.Instance.EquipmentSystemUI.Slots[slot].SelectSlot();
        curSlotEquip = slot;
        if (itemData == null)
        {
            GameManageMent.Instance.PlayerManager.PlayerController.UnEquipWeaponAnim();
            weapon = null;
            return;
        }

        if (itemData.Type == ItemType.Gun)
        {

            GameManageMent.Instance.PlayerManager.PlayerController.EquipWeaponAnim();
            GunData gunData = itemData as GunData;

            weaponPrefab = Instantiate(gunData.Gun.gameObject, this.transform.GetChild(2).transform);
            weapon = weaponPrefab.GetComponent<Weapon>();
            Debug.Log(GameManageMent.Instance.PlayerManager.PlayerController.PlayerDir.x + " " + GameManageMent.Instance.PlayerManager.PlayerController.PlayerDir.y);
            weapon.UpdateAnim(GameManageMent.Instance.PlayerManager.PlayerController.PlayerDir.x, GameManageMent.Instance.PlayerManager.PlayerController.PlayerDir.y);
            return;
        }
        else
        {
            GameManageMent.Instance.PlayerManager.PlayerController.UnEquipWeaponAnim();
        }
        if (itemData.Type != ItemType.Melee)
        {
            weapon = null;


        }
        else
        {
            GameManageMent.Instance.PlayerManager.PlayerController.EquipWeaponAnim();
            MeleeData meleeData = itemData as MeleeData;
            weaponPrefab = Instantiate(meleeData.Melee.gameObject, this.transform.GetChild(2).transform);
            weapon = weaponPrefab.GetComponent<Weapon>();
            return;
        }

        if (itemData.Type == ItemType.HpPotion || itemData.Type == ItemType.Bullet)
        {
            itemData.UseItem();
            UIManageMent.Instance.EquipmentSystemUI.EquipMentSystem.UseSlot(slot, 1);

        }
        else if (itemData.Type == ItemType.Buildable)
        {
            BuildableData buildableData = itemData as BuildableData;
            GameManageMent.Instance.BuildManager.TurnOnBuildMode(buildableData.Index_BuildableObject);
        }
    }
    public void ChooseSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EquipSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            EquipSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            EquipSlot(5);
        }
    }

    public void UnEquipSlot()
    {
        weapon = null;
        Destroy(weaponPrefab);
        GameManageMent.Instance.PlayerManager.PlayerController.UnEquipWeaponAnim();
        

    }

    void Awake()
    {
        weapon = null;
    }
}
