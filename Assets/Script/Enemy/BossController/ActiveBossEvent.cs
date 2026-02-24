using System;
using UnityEngine;

public class ActiveBossTrigger: ReceiverEvent
{

    
    protected override void Unlock()
    {
        
        BossManagerInterface bossManager = GetComponent<BossManagerInterface>();

        if(bossManager != null && !isUnlocked)
        {
            isUnlocked = true;
            bossManager.ActiveBoss();
        }
    }
}
