[System.Serializable]
public class GameSaveData
{
    public PlayerData playerData;

    public EquipmentSaveData equipmentSaveData;

    public InventorySaveData inventorySaveData;

    public QuestData questData;

    
    public WorldData worldData;


    public EventSaveData eventSaveData;

    public GameSaveData()
    {
        playerData = new PlayerData();

        equipmentSaveData = new EquipmentSaveData();

        inventorySaveData = new InventorySaveData();

        questData = new QuestData();

        worldData = new WorldData();

        eventSaveData = new EventSaveData();
    }
}
