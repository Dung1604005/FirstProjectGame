using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UnlockedStatue : SenderEvent
{
    [SerializeField] private GameObject unlockedObject;

    [SerializeField] private GameObject maskObject;
 
    [SerializeField] private GameObject interactKey;

    [SerializeField] private float radiousInteract;

    [SerializeField] private float speedUnlock;

    [SerializeField] private Vector2 unlockDistance;

    private Vector2 maskDefaultPosition;

    private bool  isEndUnlocking = false;

    private Transform playerTransform;

    void Awake()
    {
        maskDefaultPosition = maskObject.transform.position;
    }



    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0).transform;
        if (eventSended)
        {
            Unlock();
        }
    }
    public override void GenerateNewID()
    {
        base.GenerateNewID();
        uniqueId = "UnlockedStatue_" + uniqueId;
    }
    public override void OnValidate()
    {
        if(uniqueId == null || uniqueId == "")
        {
            uniqueId =  "UnlockedStatue_" +System.Guid.NewGuid().ToString();
        }
    }



    public override void Restore()
    {
        base.Restore();
        unlockedObject.SetActive(true);
    }

    public void Unlock()
    {
        unlockedObject.SetActive(true);
        eventSended = true;
        
        SendEvent();
    }

    void Update()
    {
        if(playerTransform == null || eventSended)
        {
            interactKey.SetActive(false);
            if (eventSended)
            {
                if (isEndUnlocking)
                {
                    return;
                }

                maskObject.transform.position = Vector3.MoveTowards(maskObject.transform.position, maskDefaultPosition + unlockDistance, speedUnlock*Time.deltaTime);
                if(((Vector2)maskObject.transform.position-maskDefaultPosition -unlockDistance).sqrMagnitude <= 0.001f)
                {
                    isEndUnlocking = true;
                }
        
            }
            return;
        }
        if((playerTransform.position - this.transform.position).sqrMagnitude <= radiousInteract * radiousInteract)
        {
            interactKey.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Unlock();
            }
        }
        else
        {
            interactKey.SetActive(true);
        }
    }

    






}
