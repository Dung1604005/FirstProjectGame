using System;
using System.Collections.Generic;
using UnityEngine;

public class SenderEvent : MonoBehaviour
{
    [SerializeField] private List<String> eventSend;

    public void SendEvent()
    {
        foreach (string eventName in eventSend)
        {
            EventManager.Instance().OnSignalSent?.Invoke(eventName);
        }
    }
}
