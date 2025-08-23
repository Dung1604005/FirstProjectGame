using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Script/Item/MeleeData", fileName = "Melee")]
public class MeleeData : WeaponData
{

    [Header("Melee Stats")]
    
    [SerializeField] private float range;
    public float Range => range;
}