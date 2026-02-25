using System;
using System.Collections.Generic;
using UnityEngine;

public class SenderEvent : MonoBehaviour, IActivatable
{
    [SerializeField] private string uniqueId;
    [SerializeField] private List<string> eventSend;

    protected bool eventSended;

    [SerializeField] protected bool sendOneTime;

    public virtual void Activate()
    {
        eventSended = true;
    }

    [ContextMenu("Tạo Lại ID Mới")]

    public void GenerateNewID()
    {
        uniqueId = System.Guid.NewGuid().ToString();
    }

    void OnValidate()
    {
        if (uniqueId == null)
        {
            uniqueId = System.Guid.NewGuid().ToString();
        }
    }

    

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
        
        
        foreach (string eventName in eventSend)
        {
            
            EventManager.Instance().OnSignalSent?.Invoke(eventName, false);
            
        }
        
    }
}
