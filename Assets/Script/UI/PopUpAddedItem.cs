using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpAddedItem : MonoBehaviour
{
    [SerializeField] private Image popUpAddedItemImage;
    public Image PopUpAddedItemImage => popUpAddedItemImage;

    private float defaultAlpha;
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI amount;

    private int stock;
    public int Stock => stock;



    [SerializeField] private TextMeshProUGUI name;

    public TextMeshProUGUI Name => name;

    [SerializeField] private float existTime;

    [SerializeField] private float timerExist = 0f;

    private bool state = false;
    public bool State => state;
    void Awake()
    {
        popUpAddedItemImage = GetComponent<Image>();
        defaultAlpha = popUpAddedItemImage.color.a;
        TurnOff();
    }

    public void SetInfo(Sprite _icon, int _amount, string _name)
    {
        Icon.sprite = _icon;
        timerExist = 0f;
        amount.text = "X" + _amount.ToString();
        stock = _amount;
        name.text = _name;
        TurnOn();

    }

    public void UpdateAmount(int addamount)
    {
        TurnOn();
        stock += addamount;
        amount.text = "X" + stock.ToString();
        timerExist = 0f;

    }

    public void TurnOn()
    {
        
        popUpAddedItemImage.DOFade(defaultAlpha, 0.5f).OnComplete(()=> state = true);
        amount.DOFade(1f, 0.5f);
        name.DOFade(1f, 0.5f);
        Icon.DOFade(1f, 0.5f);
    }
    
    public void TurnOff()
    {

        popUpAddedItemImage.DOFade(0, 0.5f).OnComplete(() => state = false);
        amount.DOFade(0f, 0.5f);
        name.DOFade(0f, 0.5f);
        Icon.DOFade(0f, 0.5f);
    }
    void Update()
    {
        
        
        if (timerExist >= existTime && state == true)
        {
            TurnOff();
        }
        else if(timerExist <= existTime)
        {
            timerExist += Time.deltaTime;
        }
    }
}
