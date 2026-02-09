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
        
        if (this.GetComponent<SpriteRenderer>() != null)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (this.GetComponent<Animator>() != null)
        {
            this.GetComponent<Animator>().enabled = true;
        }

        bool satisfied = true;
        foreach (Pair<string, bool> eventState in requiredEvents)
        {
            if (eventState.First.Equals(eventName))
            {
                eventState.Second = true;
            }
            if (eventState.Second == false)
            {
                satisfied = false;
                
            }
        }

        if (satisfied)
        {

            Unlock();
            if (this.GetComponent<SenderEvent>() != null)
            {
                this.GetComponent<SenderEvent>().enabled = true;
            }
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);

            }
        }

    }

    protected virtual void Unlock()
    {

    }
    void Awake()
    {
        
    }
}
