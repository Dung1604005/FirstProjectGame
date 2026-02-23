using System;
using System.Collections.Generic;
using UnityEngine;

public class SenderEvent : MonoBehaviour
{
    [SerializeField] private List<string> eventSend;

    protected bool eventSended;

    [SerializeField] protected bool sendOneTime;

    

    public void SendEvent()
    {
        if(sendOneTime && eventSended)
        {
            return;
        }
        
        foreach (string eventName in eventSend)
        {
            
            EventManager.Instance().OnSignalSent?.Invoke(eventName, true);
            
        }
        eventSended = true;
    }
    public void RecallEvent()
    {
        if(sendOneTime && eventSended)
        {
            return;
        }
        
        foreach (string eventName in eventSend)
        {
            
            EventManager.Instance().OnSignalSent?.Invoke(eventName, false);
            
        }
        eventSended = true;
    }
}
