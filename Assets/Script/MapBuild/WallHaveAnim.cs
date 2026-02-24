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

    protected override void Unlock()
    {
        animator.SetTrigger(nameAnimPlay);
        isUnlocked = true;
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
