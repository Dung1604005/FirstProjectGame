using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using UnityEngine;

public enum ItemType
{
    Consumable, Gun, Melee, Bullet
}


public abstract class ItemData : ScriptableObject
{
    [SerializeField] protected string description;
    public string Description => description;
    [SerializeField] protected bool stackable;
    public bool Stackable => stackable;

    [SerializeField] protected int maxStack;
    public int MaxStack => maxStack;
    [SerializeField] protected ItemType type;
    public ItemType Type => type;
    [SerializeField] protected string itemName;
    public string ItemName => itemName;
    [SerializeField] protected int index;
    public int Index => index;
    [SerializeField] protected Sprite icon;
    public Sprite Icon => icon;

    [SerializeField] int value;
    public int Value => value;

}

//ItemData



