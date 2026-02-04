using System;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverEvent : MonoBehaviour
{
    [SerializeField] private List<Pair<String, bool>> requiredEvents;



    private void OnEnable()
    {
        
        EventManager.Instance().OnSignalSent += CheckCondition;
    }

    private void OnDisable()
    {
        
        EventManager.Instance().OnSignalSent -= CheckCondition;
    }

    public void CheckCondition(String eventName)
    {
        bool satisfied = true;
        foreach(Pair<String, bool> eventState in requiredEvents)
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
    }
}
