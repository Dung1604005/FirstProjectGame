using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeData", menuName = "Scripts/RecipeData")]
public class RecipeData : ScriptableObject
{
    [SerializeField] private ItemData itemToCraftData;

    public ItemData ItemToCraftData => itemToCraftData;

    [SerializeField] private MaterialData requiredMaterial1Data;

    public MaterialData RequiredMaterial1Data => requiredMaterial1Data;

    [SerializeField] private int requiredMaterial1Amount;
    public int RequiredMaterial1Amount => requiredMaterial1Amount;
    [SerializeField] private MaterialData requiredMaterial2Data;
    public MaterialData RequiredMaterial2Data => requiredMaterial2Data;
    [SerializeField] private int requiredMaterial2Amount;
    public int RequiredMaterial2Amount => requiredMaterial2Amount;




}
