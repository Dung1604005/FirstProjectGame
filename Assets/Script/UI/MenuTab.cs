using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuTab : MonoBehaviour
{
    [SerializeField] private Image background;

    [SerializeField] private TextMeshProUGUI tabName;

    void Awake()
    {
        background = GetComponent<Image>();
    }

    public void ActiveTab()
    {
        background.sprite = UIManageMent.Instance.TabBackground_Active;
        Color _color;
        if (ColorUtility.TryParseHtmlString(GameConfig.COLOR_TABNAME_ACTIVE, out _color))
        {
            tabName.color = _color;
        }
        tabName.enableVertexGradient = true;
        if (ColorUtility.TryParseHtmlString("#E6C77CFF", out _color))
        {
            tabName.colorGradient = new VertexGradient(Color.white, Color.white, _color, _color);
        }
    }
    public void InactiveTab()
    {
        background.sprite = UIManageMent.Instance.TabBackground_Inactive;
        Color _color;
        if (ColorUtility.TryParseHtmlString(GameConfig.COLOR_TABNAME_INACTIVE, out _color))
        {
            tabName.color = _color;
        }
        tabName.enableVertexGradient = false;
    }

    
}
