using System.Collections.Generic;
[System.Serializable]

public class WorldData
{
    public List<string> chestOpenedId;

    public List<int> defeatedBossId;

    public List<string> activatedObjectId;

    public float timeSaveData;

    public WorldData(List<string> _chestOpenedId, List<int> _defeatedBossId, List<string> _activatedObjectId, float _timeSaveData)
    {
        chestOpenedId = new List<string>(_chestOpenedId);

        defeatedBossId = new List<int>(_defeatedBossId);

        activatedObjectId = new List<string>(_activatedObjectId);
        timeSaveData = _timeSaveData;
    }


}
