using System;
using System.Collections.Generic;
using UnityEngine;

public  class EventManager 
{
    private static EventManager _instance;
    public  Action<string, bool> OnSignalSent;

    public Action<string, bool> OnSignalRecall;
    protected EventManager(){


    }
    public static EventManager Instance()
    {

      if (_instance == null)
      {
        _instance = new EventManager();
      }
 
      return _instance;
    }

    
}
