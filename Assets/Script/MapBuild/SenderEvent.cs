using System;
using System.Collections.Generic;
using UnityEngine;

public class SenderEvent : MonoBehaviour, IRestorable
{
    [SerializeField] protected string uniqueId;

    
    [SerializeField] private List<string> eventSend;

    [SerializeField] protected bool eventSended;

    [SerializeField] protected bool sendOneTime;

    public virtual void Restore(string _id)
    {
        if(_id != uniqueId || this == null)
        {
            return;
        }
        eventSended = true;
    }

    [ContextMenu("Tạo Lại ID Mới")]

    void Start()
    {
        GameManageMent.Instance._WorldManager.OnLoadDataObject += Restore;
    }

    public virtual void GenerateNewID()
    {
        uniqueId = System.Guid.NewGuid().ToString();
    }

    public virtual void OnValidate()
    {
        if (uniqueId == null|| uniqueId == "")
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
        GameManageMent.Instance._WorldManager.AddActivatedObject(uniqueId);
    }
    public void RecallEvent()
    {
        
        
        foreach (string eventName in eventSend)
        {
            
            EventManager.Instance().OnSignalSent?.Invoke(eventName, false);
            
        }
        
    }
}
