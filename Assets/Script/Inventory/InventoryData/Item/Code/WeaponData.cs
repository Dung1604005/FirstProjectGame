using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ItemData
{
    [SerializeField] private int damaged;
    public int Damaged => damaged;
    [SerializeField] private float coolDown;
    public float CoolDown => coolDown;
}
