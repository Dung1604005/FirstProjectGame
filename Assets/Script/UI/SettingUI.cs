using UnityEngine;

public class SettingUI : MenuLayOutUI
{
    public void QuitGame()
    {
        AudioManager.Instance.PlayUIClick();
        Application.Quit();

        
    }
}
