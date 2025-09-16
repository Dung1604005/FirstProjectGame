using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Script/Item/BuildableData", fileName = "BuildableData")]
public class BuildableData : ItemData
{
    [SerializeField] private float health;
    public float Health => health;


    [SerializeField] private int index_BuildableObject;
    public int Index_BuildableObject => index_BuildableObject;

    

    



}
