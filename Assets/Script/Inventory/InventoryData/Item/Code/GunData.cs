using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Script/Item/GunData", fileName = "Gun")]

public class GunData : WeaponData
{
    [Header("Gun Stats")]
    
    [SerializeField] private GameObject bullet;
    public GameObject Bullet => bullet;
}