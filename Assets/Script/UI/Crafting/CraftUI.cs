using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftUI : MenuLayOutUI
{
    [SerializeField] private List<RecipeData> itemRecipeDatas;

    public List<RecipeData> ItemRecipeDatas => itemRecipeDatas;
    [SerializeField] private List<RecipeUI> recipeUIs;

    [SerializeField] private RecipeUI recipeUIPrefab;

    [SerializeField] private GameObject content;

    void Awake()
    {

    }
    void Start()
    {
        for (int i = 0; i < itemRecipeDatas.Count; i++)
        {
            var new_slot = Instantiate(recipeUIPrefab, content.transform);
            new_slot.SetInfo(itemRecipeDatas[i]);
            recipeUIs.Add(new_slot);

        }
    }
    

}
