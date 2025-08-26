using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseData : ScriptableObject
{
    [SerializeField] private float maxHealth;
    public float MaxHealth => maxHealth;
    [SerializeField] private float atk;
    public float Atk => atk;
    [SerializeField] private float coolDown;
    public float CoolDown => coolDown;

    [SerializeField] private float rangeAtk;
    public float RangeAtk => rangeAtk;

    [SerializeField] private float rangeChase;

    public float RangeChase => rangeChase;

    [SerializeField] private float speed;
    public float Speed => speed;

    [SerializeField] private float goldValue;
    public float GoldValue => goldValue;
    [SerializeField] private float expValue;
    public float ExpValue => expValue;
}
