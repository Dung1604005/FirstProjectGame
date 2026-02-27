using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager
{
    
    private List<string> chestOpenedId= new List<string>();

    private List<int> defeatedBossId = new List<int>();

    private List<string> activatedObjectId = new List<string>(); 


    public event Action<string> OnLoadDataObject;

    public event Action<int> OnLoadDataBoss;

    

    

    public WorldData GetWorldSaveData()
    {
        WorldData worldData = new WorldData(chestOpenedId, defeatedBossId, activatedObjectId, GameManageMent.Instance.TimeManager.GetTimeSaveData());
        return worldData;
    }

    public void LoadWorldSaveData(WorldData worldData)
    {
        chestOpenedId = new List<string>(worldData.chestOpenedId);

        defeatedBossId = new List<int>(worldData.defeatedBossId);

        activatedObjectId = new List<string>(worldData.activatedObjectId);

        GameManageMent.Instance.TimeManager.LoadTimeSaveData(worldData.timeSaveData);
        for(int i = 0; i < chestOpenedId.Count; i++)
        {
            OnLoadDataObject?.Invoke(chestOpenedId[i]);
        }
        for(int i = 0; i < activatedObjectId.Count; i++)
        {
            OnLoadDataObject?.Invoke(activatedObjectId[i]);
        }
        for(int i = 0; i < defeatedBossId.Count; i++)
        {
            OnLoadDataBoss?.Invoke(defeatedBossId[i]);
        }
        
    }

    public void AddOpenedChest(string id)
    {
        
        if (!chestOpenedId.Contains(id))
        {
            chestOpenedId.Add(id);
        }
    }

    public void AddDefeatedBoss(int id)
    {
        if (!defeatedBossId.Contains(id))
        {
            defeatedBossId.Add(id);
        }
    }

    public void AddActivatedObject(string id)
    {
        if (!activatedObjectId.Contains(id))
        {
            activatedObjectId.Add(id);
        }
    }

    

    

}
