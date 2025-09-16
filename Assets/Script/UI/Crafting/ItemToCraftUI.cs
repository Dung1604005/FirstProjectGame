using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemToCraftUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image icon;

    [SerializeField] private TextMeshProUGUI amountText;

    [SerializeField] private int amount;
    public int Amount => amount;

    [SerializeField] private int indexItem;

    [SerializeField] private float timeToCraft;

    [SerializeField] private Image fillBorder;

    [SerializeField] private Image glowBorder;

    [SerializeField] private bool isHolding = false;
    [SerializeField] private float holdTimer = 0f;

    void Awake()
    {
        icon.gameObject.SetActive(false);
        amountText.gameObject.SetActive(false);
    }
    public void SetInfo(int _index)
    {
        
        indexItem = _index;
        icon.gameObject.SetActive(true);
        amountText.gameObject.SetActive(true);
        ItemData data = GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem];
        icon.sprite = data.Icon;
        icon.type = Image.Type.Simple;
        icon.preserveAspect = true;
        amountText.text = "x1";
    }
    private void StartCrafting()
    {
        if (!gameObject.GetComponentInParent<RecipeUI>().CanCraft())
        {
            return;
        }
        isHolding = true;
        glowBorder.gameObject.SetActive(true);
        fillBorder.fillAmount = 0f;
        holdTimer = 0f;


    }
    private void EndCrafting()
    {
        if (holdTimer >= timeToCraft )
        {
            gameObject.GetComponentInParent<RecipeUI>().Craft();
        }
        isHolding = false;
        glowBorder.gameObject.SetActive(false);
        fillBorder.fillAmount = 0f;
        holdTimer = 0f;
        
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        
        StartCrafting();

    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {

        
        EndCrafting();
    }
    void Update()
    {
        if (isHolding && holdTimer < timeToCraft)
        {
            holdTimer += Time.deltaTime;
            fillBorder.fillAmount = holdTimer / timeToCraft;
        }
        else
        {
            if (isHolding && holdTimer >= timeToCraft)
            {
                EndCrafting();
            }
        }
    }


}
