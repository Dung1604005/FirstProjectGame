using UnityEngine;
using System.IO;
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance {get; private set;}

    private string saveFileName = "MyGameSave.json";

    private GameSaveData gameSaveData;

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

    public void ClearData()
    {
        GameSaveData dataToSave = new GameSaveData();

        string jsonString = JsonUtility.ToJson(dataToSave, true);

        string fullPath = Path.Combine(Application.persistentDataPath, saveFileName);
        File.WriteAllText(fullPath, jsonString);

        Debug.Log("<color=green>ĐÃ LƯU GAME THÀNH CÔNG TẠI: </color>" + fullPath);
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

            if (loadedData == null)
            {
                ClearData();
                loadedData = new GameSaveData();
            }
            if (loadedData.worldData != null)
            {
                SceneData sceneData = SceneLoader.Instance.GetSceneDataById(loadedData.worldData.idSceneData);
                
                if (sceneData != null)
                {
                    gameSaveData = loadedData;
                    Vector3 savedPos = new Vector3(loadedData.playerData.posX, loadedData.playerData.posY, loadedData.playerData.posZ);
                    SceneLoader.Instance.LoadScene(sceneData, savedPos);
                }
            }

            
        }
        catch (System.Exception e)
        {
            Debug.LogError("Có lỗi khi load file JSON: " + e);
        }
    }
    public void LoadDataRemain()
    {
        GameSaveData loadedData= gameSaveData;
        
        GameManageMent.Instance.PlayerManager.LoadPlayerData(loadedData.playerData);
            GameManageMent.Instance.InventoryAndEquipmentManager.InventorySystem.LoadInventoryData(
                loadedData.inventorySaveData != null ? loadedData.inventorySaveData.savedItems : null);
            GameManageMent.Instance.InventoryAndEquipmentManager.EquipMentSystem.LoadEquipmentSaveData(
                loadedData.equipmentSaveData != null ? loadedData.equipmentSaveData.savedEquipment : null);
            GameManageMent.Instance.QuestManager.LoadQuestData(loadedData.questData);
            EventManager.Instance().LoadEventSaveData(
                loadedData.eventSaveData != null ? loadedData.eventSaveData.eventSaveDatas : null);
            GameManageMent.Instance._WorldManager.LoadWorldSaveData(loadedData.worldData);

            // Load scene từ idSceneData trong WorldData
            

            Debug.Log("<color=cyan>ĐÃ LOAD GAME THÀNH CÔNG!</color>");
    }
}
