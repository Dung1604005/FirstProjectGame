using UnityEngine;

public class MenuUIManagement : MonoBehaviour
{

    [SerializeField] private GameObject startButton;

    [SerializeField] private GameObject settingButton;

    [SerializeField] private GameObject quitButton;

    [SerializeField] private GameObject confirmQuitNotification;

    [SerializeField] private GameObject choosePlayNotification;


    
    public void StartGame()
    {
        
    }


    public void SetActiveAllButtonMenu(bool active)
    {
        startButton.SetActive(active);
        settingButton.SetActive(active);
        quitButton.SetActive(active);
    }
    public void OpenQuitNotification()
    {
        SetActiveAllButtonMenu(false);
        confirmQuitNotification.SetActive(true);
    }

    public void OpenChoosePlayNotification()
    {
        SetActiveAllButtonMenu(false);
        choosePlayNotification.SetActive(true);
    }

    public void NewGame()
    {
        SaveLoadManager.Instance.ClearData();
        SaveLoadManager.Instance.LoadGame();
    }
    public void Continue()
    {
        SaveLoadManager.Instance.LoadGame();
    }

    public void CloseQuitNotification()
    {
        SetActiveAllButtonMenu(true);
        confirmQuitNotification.SetActive(false);
    }

    public void Quit(){
        Application.Quit();

        UnityEditor.EditorApplication.isPlaying = false;
    }

    
}
