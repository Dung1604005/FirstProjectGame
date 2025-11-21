using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum GunType
{
    PISTOL,
    GUN,
    SHOTGUN
}
[CreateAssetMenu(menuName = "Script/Item/GunData", fileName = "Gun")]

public class GunData : WeaponData
{
    [Header("Gun Stats")]
    
    [SerializeField] private GameObject bullet;
    [SerializeField] private Weapon gun;

    [SerializeField] private GunType gunType;
    public GunType GunType => gunType;

    [SerializeField] private int magSize;
    public int MagSize => magSize;

    [SerializeField] private int reloadTime;

    public int ReloadTime => reloadTime;

    [SerializeField] private Sprite bulletUI;
    public Sprite BulletUI => bulletUI;
    public Weapon Gun => gun;
    public GameObject Bullet => bullet;
}