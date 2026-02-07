using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UnlockedStatue : SenderEvent
{
    [SerializeField] private GameObject unlockedObject;

    [SerializeField] private GameObject interactKey;

    [SerializeField] private float radiousInteract;

    private Transform playerTransform;

    

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0).transform;
        if (eventSended)
        {
            Unlock();
        }
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
    }

    






}
