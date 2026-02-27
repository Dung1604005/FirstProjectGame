
using System.Collections.Generic;

[System.Serializable]
public class EventSaveData
{
    public List<string> eventSaveDatas;

    public EventSaveData(List<string> _eventSaveDatas)
    {
        eventSaveDatas = new List<string>(_eventSaveDatas);
    }
}
