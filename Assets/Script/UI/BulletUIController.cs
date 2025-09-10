using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletUIController : MonoBehaviour
{
    [SerializeField] private Image bulletUISprite;
    public Image BulletUISprite => bulletUISprite;

    [SerializeField] private TextMeshProUGUI currentBulletText;

    [SerializeField] private TextMeshProUGUI magSizeBulletText;

    [SerializeField] private TextMeshProUGUI totalBulletText;

    public void UpdateBulletUI(Sprite _bulletUISprite, int _currentBullet, int _magSizeBullet, int _totalBullet)
    {
        bulletUISprite.sprite = _bulletUISprite;
        bulletUISprite.type = Image.Type.Simple;
        bulletUISprite.preserveAspect = true;
        currentBulletText.text = _currentBullet.ToString();
        magSizeBulletText.text = "/"+_magSizeBullet.ToString();
        totalBulletText.text = "RES: "+_totalBullet.ToString();
    }

    public void SetStateCurrentBulletColor(String color)
    {
        Color _color;
        if(ColorUtility.TryParseHtmlString(color, out _color))
        {
            currentBulletText.color = _color;
        }
    }

    public void UpdateCurrentBullet(int _currentBullet)
    {
        currentBulletText.text = _currentBullet.ToString();
    }

    public void TurnOffBulletUI()
    {
        gameObject.SetActive(false);
    }

    public void TurnOnBulletUI()
    {
        gameObject.SetActive(true);
    }

    


}
