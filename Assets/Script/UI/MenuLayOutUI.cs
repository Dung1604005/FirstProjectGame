using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuLayOutUI : MonoBehaviour
{
    public virtual void TurnOn()
    {
        this.gameObject.SetActive(true);
    }
    
    public virtual void  TurnOff()
    {
        this.gameObject.SetActive(false);
    }
   
}
