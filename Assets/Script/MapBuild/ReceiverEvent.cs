using System;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverEvent : MonoBehaviour, IActivatable
{
    [SerializeField] private string uniqueId;
    [SerializeField] private List<Pair<string, bool>> requiredEvents;

    protected bool isUnlocked = false;

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

    private void OnEnable()
    {

        EventManager.Instance().OnSignalSent += CheckCondition;
        EventManager.Instance().OnSignalRecall += CheckCondition;
    }

    private void OnDisable()
    {

        EventManager.Instance().OnSignalSent -= CheckCondition;
        EventManager.Instance().OnSignalRecall -= CheckCondition;
    }

    public void CheckCondition(string eventName , bool type)
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
                eventState.Second = type;
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
                this.gameObject.SetActive(true);

            }
        }

    }

    public virtual void Activate()
    {
        isUnlocked = true;
        if (this.GetComponent<SpriteRenderer>() != null)
        {
            this.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (this.GetComponent<Animator>() != null)
        {
            this.GetComponent<Animator>().enabled = true;
        }
         if (this.GetComponent<SenderEvent>() != null)
            {
                this.GetComponent<SenderEvent>().enabled = true;
            }
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
                this.gameObject.SetActive(true);

            }
    }

    protected virtual void Unlock()
    {

    }
    void Awake()
    {
        
    }
}
