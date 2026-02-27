using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class EventManager
{
  private static EventManager _instance;
  public Action<string, bool> OnSignalSent;

  private Dictionary<string, bool> currentEventSignal = new Dictionary<string, bool>();

  public List<string> GetEventSaveData()
  {
    List<string> eventSaveDatas = new List<string>();
    foreach(var item in currentEventSignal)
    {
      eventSaveDatas.Add(item.Key);
    }
    return eventSaveDatas;
  }

  public void LoadEventSaveData(List<string> eventSaveDatas)
  {
    currentEventSignal.Clear();
    foreach(string _event in eventSaveDatas)
    {
      OnSignalSent?.Invoke(_event, true);
    }
  }


  protected EventManager()
  {
    OnSignalSent += UpdateCurrentEventSignal;


  }
  public static EventManager Instance()
  {

    if (_instance == null)
    {
      _instance = new EventManager();
    }

    return _instance;
  }

  public void UpdateCurrentEventSignal(string eventName, bool active)
  {
    if (currentEventSignal.ContainsKey(eventName))
    {
      if (active == false)
      {
        currentEventSignal.Remove(eventName);
      }

    }
    else
    {
      if (active == true)
      {
        currentEventSignal.Add(eventName, true);
      }
    }
  }


}
