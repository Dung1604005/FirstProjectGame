using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackGroundUI : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] private ContextMenu2 contextMenu;

    public void OnPointerClick(PointerEventData eventData)
    {
        
        contextMenu.TurnOff();
    }    
}
