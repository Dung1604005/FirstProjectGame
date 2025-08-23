using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        stat.text = Stat;
        icon.sprite = Icon;
        name_item.text = NameItem;
        description.text = Description;
    }
    void Awake()
    {
        
        
    }
    public void TurnOff()
    {
        UIManageMent.Instance.DoFadeIn(this.gameObject.GetComponent<Image>(), 0.3f);
        this.gameObject.SetActive(false);
    }
    public void TurnOn()
    {
        this.gameObject.SetActive(true);
        UIManageMent.Instance.DoFadeOut(this.gameObject.GetComponent<Image>(), 0.3f);
    }
        
}
