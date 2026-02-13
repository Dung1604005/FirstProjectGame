using System;
using System.Collections.Generic;
using UnityEngine;

public class SenderEvent : MonoBehaviour
{
    [SerializeField] private List<string> eventSend;

    protected bool eventSended;

    

    public void SendEvent()
    {
        
        foreach (string eventName in eventSend)
        {
            
            EventManager.Instance().OnSignalSent?.Invoke(eventName, true);
            
        }
        eventSended = true;
    }
    public void RecallEvent()
    {
        
        foreach (string eventName in eventSend)
        {
            
            EventManager.Instance().OnSignalSent?.Invoke(eventName, false);
            
        }
        eventSended = true;
    }
}
