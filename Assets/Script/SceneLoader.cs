using System;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    public static SceneLoader Instance {get; private set;}

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            if(Instance != this)
            {
                Destroy(this);
            }
        }
    }
    public void LoadScene(String sceneName)
    {
        
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void LoadSceneAdditive(String sceneName)
    {
        UIManageMent.Instance.LoadingAdditive.TurnOn();
        StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Additive));
    }

    IEnumerator LoadSceneAsync(String sceneName, LoadSceneMode loadSceneMode)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        
        while (!asyncOperation.isDone)
        {
            UIManageMent.Instance.LoadingAdditive.SetFillTarget(asyncOperation.progress);
            yield return null;
        }

    }
}
