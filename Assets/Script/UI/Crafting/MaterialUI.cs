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


    void Awake()
    {
        icon.gameObject.SetActive(false);
        amountText.gameObject.SetActive(false);
    }
    public void SetInfo(int _index, int _amount)
    {
        icon.gameObject.SetActive(true);
        amountText.gameObject.SetActive(true);
        indexItem = _index;
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

        if (GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.ItemCount.TryGetValue(indexItem, out var cur))
        {
            amountText.text = cur.ToString() + "/" + amount.ToString();
            Debug.Log(cur + " " + amount);
            if (cur < amount)
            {
                Debug.Log("red");
                if (ColorUtility.TryParseHtmlString(GameConfig.COLORREDRELOAD, out var red))
                    amountText.color = red;
            }
            else
            {
                amountText.color = Color.white;
                
            }
                    
        }
        else
        {
            amountText.text = "0" + "/" + amount.ToString();
           
            if(ColorUtility.TryParseHtmlString(GameConfig.COLORREDRELOAD, out var red))
                amountText.color = red;
            
        }
        
    }
    void Start()
    {
        GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.OnChangeInventory += UpdateAmount;
    }
}
