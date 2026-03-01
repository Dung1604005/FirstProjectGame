
using System.Collections.Generic;
using UnityEngine;

public class MenuUIManagement : MonoBehaviour
{

    [SerializeField] private GameObject uiCanvas;

    [SerializeField] private GameObject startButton;

    [SerializeField] private GameObject settingButton;

    [SerializeField] private GameObject quitButton;

    [SerializeField] private GameObject confirmQuitNotification;

    [SerializeField] private GameObject choosePlayNotification;

    [SerializeField] private List<GameObject> uiObjectList;


    
    public void StartGame()
    {
        
    }

    public void ClearUIBeforeStart()
    {
        foreach(GameObject gameObject in uiObjectList)
        {
            gameObject.SetActive(false);
        }
    }
    public void TurnOnUIAfterStart()
    {
        foreach(GameObject gameObject in uiObjectList)
        {
            gameObject.SetActive(true);
        }
    }


    public void SetActiveAllButtonMenu(bool active)
    {
        startButton.SetActive(active);
        settingButton.SetActive(active);
        quitButton.SetActive(active);
    }
    public void OpenQuitNotification()
    {
        AudioManager.Instance.PlayUIClick();
        SetActiveAllButtonMenu(false);
        confirmQuitNotification.SetActive(true);
    }

    public void OpenChoosePlayNotification()
    {
        AudioManager.Instance.PlayUIClick();
        SetActiveAllButtonMenu(false);
        choosePlayNotification.SetActive(true);
    }

    public void NewGame()
    {
        AudioManager.Instance.PlayUIClick();
        uiCanvas.SetActive(false);
        TurnOnUIAfterStart();
        SaveLoadManager.Instance.ClearData();
        SaveLoadManager.Instance.LoadGame();
    }
    public void Continue()
    {
        AudioManager.Instance.PlayUIClick();
        uiCanvas.SetActive(false);
        TurnOnUIAfterStart();
        SaveLoadManager.Instance.LoadGame();
    }

    public void CloseQuitNotification()
    {
        AudioManager.Instance.PlayUIClick();
        SetActiveAllButtonMenu(true);
        confirmQuitNotification.SetActive(false);
    }

    public void Quit(){
        AudioManager.Instance.PlayUIClick();
        Application.Quit();

        
    }
    void Start()
    {
        ClearUIBeforeStart();
    }

    
}
