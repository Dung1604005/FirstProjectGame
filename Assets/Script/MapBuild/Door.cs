using UnityEngine;

public class Door : ReceiverEvent
{
    [SerializeField] private float openSpeed;

    [SerializeField] private Vector2 openDistance;

    private Vector3 defaultPosition;

    void Awake()
    {
        
        defaultPosition = this.transform.position;
    }

    protected override void Unlock()
    {
        isUnlocked = true;
    }

    public void Update()
    {
        if (isUnlocked)
        {
            
            this.transform.position = Vector2.Lerp(this.transform.position, (Vector2)defaultPosition + openDistance, openSpeed * Time.deltaTime);
        }
    }


}
