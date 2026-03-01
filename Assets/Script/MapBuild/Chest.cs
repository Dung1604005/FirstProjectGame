using System;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour, IRestorable
{




    [SerializeField] private string uniqueId;

    
    [SerializeField] private int indexLootTable;
    [SerializeField] private int amountDrop;
    [SerializeField] private float radInteract;

    [SerializeField] private GameObject keyObject;
    private Animator animator;

    public bool isOpened;



    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    void Start()
    {
        GameManageMent.Instance._WorldManager.OnLoadDataObject += Restore;
    }

    [ContextMenu("Tạo Lại ID Mới")]

    public void GenerateNewID()
    {
        uniqueId = "Chest_"+System.Guid.NewGuid().ToString();
    }

    void OnValidate()
    {
        if (uniqueId == null|| uniqueId == "")
        {
            uniqueId = "Chest_"+System.Guid.NewGuid().ToString();
        }
    }

    public void DropItem()
    {
        animator.enabled = false;
        if (isOpened)
        {
            return;
        }
        isOpened = true;
        GameManageMent.Instance._WorldManager.AddOpenedChest(uniqueId);
        GameManageMent.Instance.DropSystem.DropItem(indexLootTable, amountDrop, this.gameObject.transform.position);
        this.gameObject.SetActive(false);
    }
    public void Restore(string _id)
    {
        if(_id != uniqueId)
        {
            return;
        }
        this.gameObject.SetActive(false);
        isOpened = true;
        animator.enabled = true;
    }
    



    void Update()
    {
        
        if (GameManageMent.Instance.PlayerManager.PlayerController!= null 
        &&(GameManageMent.Instance.PlayerManager.PlayerController.getPos() - (Vector2)this.transform.position).sqrMagnitude <= radInteract*radInteract
        )
        {
            keyObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.enabled = true;
            }
            

        }
        else
        {
            keyObject.SetActive(false);
        }
    }


}
