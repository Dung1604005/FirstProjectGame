using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Script/Item/HpPotionData", fileName = "HpPotion")]
public class HpPotionData : ItemData
{
    [SerializeField] private float hpRecover;
    public float HpRecover => hpRecover;
}