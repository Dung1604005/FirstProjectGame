using System.Collections;
using UnityEngine;

public class ButtonTrigger: SenderEvent
{
    [SerializeField] private bool haveAnim;
    private Animator anim;

    [SerializeField] private bool isPressed;


    [SerializeField] private float stayPressTime;

    void Awake()
    {
        if (haveAnim)
        {
            anim = GetComponent<Animator>();
        }
        
    }


    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag == GameConfig.PLAYER_TAG0)
        {
            if (haveAnim)
            {
                if (!isPressed)
                {
                    anim.SetTrigger("down");
                    isPressed = true;
                    SendEvent();
                    if(stayPressTime > 0)
                    {
                        StartCoroutine(StayPressed());
                    }
                }
            }
        }
    }

    IEnumerator StayPressed()
    {
        yield return new WaitForSeconds(stayPressTime);
        isPressed = false;
        anim.SetTrigger("up");
        RecallEvent();
        
    }

    
}
