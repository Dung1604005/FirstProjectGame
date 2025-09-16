using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{

    
    [SerializeField] private ItemToCraftUI itemToCraftUI;

    [SerializeField] private MaterialUI material1UI;
    [SerializeField] private MaterialUI material2UI;
    private RecipeData recipeData;

    void Awake()
    {

    }
    public void SetInfo(RecipeData _recipeData)
    {


        recipeData = _recipeData;
        itemToCraftUI.SetInfo(recipeData.ItemToCraftData.Index);
        material1UI.SetInfo(recipeData.RequiredMaterial1Data.Index, recipeData.RequiredMaterial1Amount);
        if (recipeData.RequiredMaterial2Data != null)
        {
            
            material2UI.SetInfo(recipeData.RequiredMaterial2Data.Index, recipeData.RequiredMaterial2Amount);
        }
        else
        {
            material2UI = null;
        }


    }

    public bool CanCraft()
    {

        if (material1UI.EnoughMaterial)
        {
            if (material2UI == null)
            {
                return true;
            }
            if (!material2UI.EnoughMaterial)
            {
                UIManageMent.Instance.UpdateWarning(GameConfig.CANT_CRAFT_WARNING);
                UIManageMent.Instance.TurnOnWarning();
            }
            return material2UI.EnoughMaterial;

        }
        UIManageMent.Instance.UpdateWarning(GameConfig.CANT_CRAFT_WARNING);
        UIManageMent.Instance.TurnOnWarning();
        return false;
    }
    private ItemData GetMaterial1Data() {
        return recipeData.RequiredMaterial1Data;
    }
    private ItemData GetMaterial2Data() {
        return recipeData.RequiredMaterial2Data;
    }
    private ItemData GetItemCraft()
    {
        return recipeData.ItemToCraftData;
    }
    public void Craft()
    {
        if (!GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.TryAdd(GetItemCraft(), 1))
        {
            UIManageMent.Instance.UpdateWarning(GameConfig.CANT_CRAFT_WARNING);
            UIManageMent.Instance.TurnOnWarning();
            return;
        }
        
        GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.RemoveItem(GetMaterial1Data(), recipeData.RequiredMaterial1Amount);
        if (material2UI != null)
        {
            GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.RemoveItem(GetMaterial2Data(), recipeData.RequiredMaterial2Amount);


        }
        GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.Add(GetItemCraft(), 1);
    }


}
