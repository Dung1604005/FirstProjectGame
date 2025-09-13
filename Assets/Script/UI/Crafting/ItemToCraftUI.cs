using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemToCraftUI : MonoBehaviour
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
    public void SetInfo(int _index)
    {
        Debug.Log(_index);
        indexItem = _index;
        icon.gameObject.SetActive(true);
        amountText.gameObject.SetActive(true);
        ItemData data = GameManageMent.Instance.ItemDataBase.ItemDatas[indexItem] ;
        icon.sprite = data.Icon;
        icon.type = Image.Type.Simple;
        icon.preserveAspect = true;
        amountText.text = "x1";
    }


}
