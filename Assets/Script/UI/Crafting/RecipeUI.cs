using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] private RecipeData recipeData;
    public RecipeData RecipeData => recipeData;

    [SerializeField] private ItemToCraftUI itemToCraftUI;

    [SerializeField] private MaterialUI material1UI;
    [SerializeField] private MaterialUI material2UI;

    void Awake()
    {

    }
    void Start()
    {
        itemToCraftUI.SetInfo(recipeData.ItemToCraftData.Index);
        material1UI.SetInfo(recipeData.RequiredMaterial1Data.Index, recipeData.RequiredMaterial1Amount);
        if (recipeData.RequiredMaterial2Data != null)
            material2UI.SetInfo(recipeData.RequiredMaterial2Data.Index, recipeData.RequiredMaterial2Amount);
        
    }


}
