using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpAddedItem : MonoBehaviour
{
    private Image popUpAddedItemImage;
    public Image PopUpAddedItemImage => popUpAddedItemImage;
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI amount;



    [SerializeField] private TextMeshProUGUI name;

    [SerializeField] private float existTime;
    void Awake()
    {
        popUpAddedItemImage = GetComponent<Image>();
    }

    public void SetInfo(Sprite _icon, int _amount, string _name)
    {
        Icon.sprite = _icon;
        amount.text = "X" + _amount.ToString();
        name.text = _name;
        Active();

    }

    public void TurnOn()
    {

        UIManageMent.Instance.DoFadeOut(popUpAddedItemImage, 0.5f);
        amount.DOFade(1f, 0.5f);
        name.DOFade(1f, 0.5f);
        Icon.DOFade(1f, 0.5f);
    }
    public void TurnOff()
    {

        UIManageMent.Instance.DoFadeIn(popUpAddedItemImage, 0.5f);
        amount.DOFade(0f, 0.5f);
        name.DOFade(0f, 0.5f);
        Icon.DOFade(0f, 0.5f);
    }
    private IEnumerator PopUp()
    {
        TurnOn();
        yield return new WaitForSeconds(existTime);
        TurnOff();
    }
    public void Active()
    {
        StartCoroutine(PopUp());

    }
}
