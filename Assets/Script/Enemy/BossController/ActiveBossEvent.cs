using System;
using UnityEngine;

public class ActiveBossTrigger: ReceiverEvent
{
    public event Action OnActiveBossEvent;
    protected override void Unlock()
    {
        OnActiveBossEvent?.Invoke();
    }
}
