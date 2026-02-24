using UnityEngine;

public class TriggerEvent: SenderEvent
{
    

    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag == GameConfig.PLAYER_TAG0)
        {
            SendEvent();
            RecallEvent();
        }
    }
}
