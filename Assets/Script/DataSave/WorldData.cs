using System.Collections.Generic;
[System.Serializable]

public class WorldData
{
    public List<string> chestOpenedId = new List<string>();

    public List<int> defeatedBossId = new List<int>();

    public List<string> activatedObjectId = new List<string>();

    public float timeSaveData = 0f;

    public int idSceneData = 0;

    public WorldData() { }

    public WorldData(List<string> _chestOpenedId, List<int> _defeatedBossId, List<string> _activatedObjectId, float _timeSaveData, int _idSceneData)
    {
        chestOpenedId = new List<string>(_chestOpenedId);

        defeatedBossId = new List<int>(_defeatedBossId);

        activatedObjectId = new List<string>(_activatedObjectId);
        timeSaveData = _timeSaveData;
        idSceneData = _idSceneData;
    }

    


}
