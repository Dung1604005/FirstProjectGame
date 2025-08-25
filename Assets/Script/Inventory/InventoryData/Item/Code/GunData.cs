using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Script/Item/GunData", fileName = "Gun")]

public class GunData : WeaponData
{
    [Header("Gun Stats")]
    
    [SerializeField] private GameObject bullet;
    [SerializeField] private Gun gun;
    public Gun Gun => gun;
    public GameObject Bullet => bullet;
}