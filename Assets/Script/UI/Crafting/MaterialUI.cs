using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MaterialUI : MonoBehaviour
{
    [SerializeField] private Image icon;

    [SerializeField] private TextMeshProUGUI amountText;

    [SerializeField] private int amount;
    public int Amount => amount;

    [SerializeField] private int indexItem;

    private bool enoughMaterial = true;
    public bool EnoughMaterial => enoughMaterial;


    void Awake()
    {
        icon.gameObject.SetActive(false);
        amountText.gameObject.SetActive(false);

    }
    public void SetInfo(int _index, int _amount)
    {
        GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.OnChangeInventory += UpdateAmount;
        icon.gameObject.SetActive(true);
        amountText.gameObject.SetActive(true);
        indexItem = _index;
        enoughMaterial = true;
        MaterialData data = GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem] as MaterialData;
        icon.sprite = data.Icon;
        icon.type = Image.Type.Simple;
        icon.preserveAspect = true;
        amount = _amount;
        amountText.text = "0" + "/" + _amount.ToString();
        UpdateAmount();
    }
    void UpdateAmount()
    {
        Debug.Log("Update Material UI");

        if (GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.ItemCount.TryGetValue(indexItem, out var cur))
        {
            amountText.text = cur.ToString() + "/" + amount.ToString();
            
            if (cur < amount)
            {
                enoughMaterial = false;
                if (ColorUtility.TryParseHtmlString(GameConfig.COLORREDRELOAD, out var red))
                    amountText.color = red;

            }
            else
            {
                amountText.color = Color.white;
                enoughMaterial = true;

            }
                    
        }
        else
        {
            amountText.text = "0" + "/" + amount.ToString();
            enoughMaterial = false;
            if(ColorUtility.TryParseHtmlString(GameConfig.COLORREDRELOAD, out var red))
                amountText.color = red;
            
        }
        
    }
    void Start()
    {
        
    }
}
