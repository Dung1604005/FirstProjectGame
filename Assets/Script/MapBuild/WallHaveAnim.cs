using UnityEngine;

public class WallHaveAnim : ReceiverEvent
{
    private Animator animator;

    private Collider2D collider2D;
    
    [SerializeField] private string  nameAnimPlay;


    void Awake()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
    }

    public override void GenerateNewID()
    {
        base.GenerateNewID();
        uniqueId = "Wall_"+uniqueId;
    }

    public override void OnValidate()
    {
        if (uniqueId == null|| uniqueId == "")
        {
            uniqueId = "Wall_" + System.Guid.NewGuid().ToString();
        }
    }

    protected override void Unlock()
    {
        animator.SetBool("down", false);
        animator.SetBool("up", false);
        animator.SetBool(nameAnimPlay, true);
        isUnlocked = true;
        GameManageMent.Instance._WorldManager.AddActivatedObject(uniqueId);
    }

    public override void Restore(string _id)
    {
        base.Restore(_id);
        animator.SetBool("down", true);
        
    }


    public void WallUp()
    {
        collider2D.enabled = true;
    }
    public void WallDown()
    {
        collider2D.enabled = false;
    }
}
