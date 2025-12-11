using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PanelClickUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI stat;

    [SerializeField] private TextMeshProUGUI description;

    [SerializeField] private TextMeshProUGUI name_item;

    


    public void UpdateContent(Sprite Icon, string Description, string NameItem, string Stat = null)
    {
        icon.gameObject.SetActive(true);
        stat.text = Stat;
        icon.sprite = Icon;
        name_item.text = NameItem;
        description.text = Description;
    }
    void Awake()
    {
        
        
    }
    
        
}
