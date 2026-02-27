using System;
using UnityEngine;

public class Door : ReceiverEvent
{
    
    [SerializeField] private float openSpeed;

    [SerializeField] private Vector2 openDistance;

    private Vector3 defaultPosition;

    [SerializeField] private bool isEndOpening = false;

    void Awake()
    {
        
        defaultPosition = this.transform.position;
    }

    public override void GenerateNewID()
    {
        base.GenerateNewID();
        uniqueId = "Door_"+uniqueId;
    }

    public override void OnValidate()
    {
        if (uniqueId == null|| uniqueId == "")
        {
            uniqueId = "Door_"+System.Guid.NewGuid().ToString();
        }
    }

    

    protected override void Unlock()
    {
        isUnlocked = true;
        GameManageMent.Instance._WorldManager.AddActivatedObject(uniqueId);
    }

    public void Update()
    {
        if (isUnlocked)
        {
            if (isEndOpening)
            {
                return;
            }
            
            this.transform.position = Vector2.MoveTowards(this.transform.position, (Vector2)defaultPosition + openDistance, openSpeed * Time.deltaTime);

            if(((Vector2)this.transform.position - (Vector2)defaultPosition - openDistance).sqrMagnitude <= 0.001f)
            {
                isEndOpening = true;
            }
        }
    }


}
