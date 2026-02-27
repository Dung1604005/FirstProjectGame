using System;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverEvent : MonoBehaviour, IRestorable
{
    [SerializeField] protected string uniqueId;

    
    [SerializeField] private List<Pair<string, bool>> requiredEvents;

    protected bool isUnlocked = false;

    [ContextMenu("Tạo Lại ID Mới")]

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

    private void OnEnable()
    {

        EventManager.Instance().OnSignalSent += CheckCondition;
        
    }

    private void OnDisable()
    {

        EventManager.Instance().OnSignalSent -= CheckCondition;
        
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

    public virtual void Restore(string _id)
    {
        if(_id != uniqueId)
        {
            return;
        }
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
        GameManageMent.Instance._WorldManager.AddActivatedObject(uniqueId);
    }
    void Awake()
    {
        
    }
    void Start()
    {
        GameManageMent.Instance._WorldManager.OnLoadDataObject += Restore;
    }
}
