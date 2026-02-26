using UnityEngine;

public class Chest : MonoBehaviour, IRestorable
{




    [SerializeField] private string uniqueId;
    [SerializeField] private int indexLootTable;
    [SerializeField] private int amountDrop;
    [SerializeField] private float radInteract;
    private Animator animator;

    public bool isOpened;



    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
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
        GameManageMent.Instance.DropSystem.DropItem(indexLootTable, amountDrop, this.gameObject.transform.position);

    }
    public void Restore()
    {
        isOpened = true;
        animator.enabled = true;
    }
    



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.enabled = true;

        }
    }


}
