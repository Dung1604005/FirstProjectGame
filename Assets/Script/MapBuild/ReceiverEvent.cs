using System;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverEvent : MonoBehaviour
{
    [SerializeField] private List<Pair<string, bool>> requiredEvents;

    protected bool isUnlocked = false;

    private void OnEnable()
    {
        
        EventManager.Instance().OnSignalSent += CheckCondition;
    }

    private void OnDisable()
    {
        
        EventManager.Instance().OnSignalSent -= CheckCondition;
    }

    public void CheckCondition(string eventName)
    {
        bool satisfied = true;
        foreach(Pair<string, bool> eventState in requiredEvents)
        {
            if (eventState.First.Equals(eventName))
            {
                eventState.Second = true;
            }
            if(eventState.Second == false)
            {
                satisfied = false;
                break;
            }
        }
        Debug.Log("Event received: " + eventName + ", satisfied: " + satisfied);
        if (satisfied)
        {
            
            Unlock();
        }
        
    }

    protected virtual void Unlock()
    {
        
    }
}
