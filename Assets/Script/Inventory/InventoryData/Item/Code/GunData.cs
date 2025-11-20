using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Script/Item/GunData", fileName = "Gun")]

public class GunData : WeaponData
{
    [Header("Gun Stats")]
    
    [SerializeField] private GameObject bullet;
    [SerializeField] private Weapon gun;

    [SerializeField] private int magSize;
    public int MagSize => magSize;

    [SerializeField] private int reloadTime;

    public int ReloadTime => reloadTime;

    [SerializeField] private Sprite bulletUI;
    public Sprite BulletUI => bulletUI;
    public Weapon Gun => gun;
    public GameObject Bullet => bullet;
}