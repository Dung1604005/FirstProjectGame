using UnityEngine;
using System.IO;
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance {get; private set;}

    private string saveFileName = "MyGameSave.json";

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

    [ContextMenu("Test LƯU GAME")]

    public void SaveGame()
    {
        GameSaveData dataToSave = new GameSaveData();

        dataToSave.playerData = GameManageMent.Instance.PlayerManager.GetSavePlayerData();

        dataToSave.equipmentSaveData = new EquipmentSaveData(GameManageMent.Instance.InventoryAndEquipmentManager.EquipMentSystem.GetEquipmentSaveData());

        dataToSave.inventorySaveData = new InventorySaveData(GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.GetSaveInventoryData());

        dataToSave.questData = new QuestData(GameManageMent.Instance.QuestManager.GetCompletedQuestIDs(), GameManageMent.Instance.QuestManager.GetQuestProgressSaveData()
        , GameManageMent.Instance.QuestManager.GetNpcSaveData());

        dataToSave.worldData = GameManageMent.Instance._WorldManager.GetWorldSaveData();

        dataToSave.eventSaveData = new EventSaveData(EventManager.Instance().GetEventSaveData());

        

        string jsonString = JsonUtility.ToJson(dataToSave, true);

        string fullPath = Path.Combine(Application.persistentDataPath, saveFileName);
        File.WriteAllText(fullPath, jsonString);

        Debug.Log("<color=green>ĐÃ LƯU GAME THÀNH CÔNG TẠI: </color>" + fullPath);
    }

    [ContextMenu("Test LOAD GAME")] // Nút test trên Inspector
    public void LoadGame()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, saveFileName);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("Không tìm thấy file save nào ở: " + fullPath);
            return;
        }
        try
        {
            
            string jsonString = File.ReadAllText(fullPath);
      
            GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(jsonString);

            GameManageMent.Instance.PlayerManager.LoadPlayerData(loadedData.playerData);
            GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.LoadInventoryData(loadedData.inventorySaveData.savedItems);
            GameManageMent.Instance.InventoryAndEquipmentManager.EquipMentSystem.LoadEquipmentSaveData(loadedData.equipmentSaveData.savedEquipment);
            GameManageMent.Instance.QuestManager.LoadQuestData(loadedData.questData);
            EventManager.Instance().LoadEventSaveData(loadedData.eventSaveData.eventSaveDatas);
            GameManageMent.Instance._WorldManager.LoadWorldSaveData(loadedData.worldData);



            Debug.Log("<color=cyan>ĐÃ LOAD GAME THÀNH CÔNG!</color>");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Có lỗi khi load file JSON: " + e.Message);
        }
    }
}
