using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackGroundUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PanelClickUI panelClickUI;
    [SerializeField] private ContextMenu contextMenu;

    public void OnPointerClick(PointerEventData eventData)
    {
        panelClickUI.TurnOff();
        contextMenu.TurnOff();
    }    
}
