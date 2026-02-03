using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    public static SceneLoader Instance {get; private set;}


    [SerializeField]  private SceneData currentSceneData;
    public SceneData CurrentSceneData => currentSceneData;

    [SerializeField] private int totalSceneData;

    public int TotalSceneData => totalSceneData;

    [SerializeField] private SceneNavigationManager sceneNavigationManager;
    public SceneNavigationManager SceneNavigationManager => sceneNavigationManager;


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
    public void LoadScene(SceneData sceneData, Vector3 startPoint)
    {
        StartCoroutine(LoadSceneAsync(sceneData, LoadSceneMode.Single, startPoint));
    }
    public void LoadSceneAdditive(SceneData sceneData, Vector3 startPoint)
    {
        StartCoroutine(LoadSceneAsync(sceneData, LoadSceneMode.Additive, startPoint));
    }

    public void BackToMainScene(Vector3 startPoint)
    {
        if(currentSceneData.TypeMap == TypeMap.MAINMAP)
        {
            return;
        }
        
        StartCoroutine(UnLoadAsync(currentSceneData, startPoint));
    }

    IEnumerator LoadSceneAsync(SceneData sceneData, LoadSceneMode loadSceneMode, Vector3 startPoint)
    {
        // Hien thi UI Loading
        UIManageMent.Instance.LoadingAdditive.TurnOn();
        // Nhuong vai frame cho game de thuc hien UI Loading hoan toan
        yield return null;
        yield return null;
        
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneData.NameScene, loadSceneMode);
        
        // Tam dung kich hoat scene de tranh bi dong man hinh
        asyncOperation.allowSceneActivation = false;
        
        // Load scene den 90% (Unity chi cho load den 0.9 khi allowSceneActivation = false)
        while (asyncOperation.progress < 0.9f)
        {
            // Hien thi progress tu 0 -> 0.9 thanh 0 -> 0.95 de nguoi choi thay muot hon
            float displayProgress = Mathf.Clamp01(asyncOperation.progress / 0.9f) * 0.95f;
            UIManageMent.Instance.LoadingAdditive.SetFillTarget(displayProgress);
            yield return null;
        }
        
        // Scene da load xong 90%, cho phep kich hoat
        UIManageMent.Instance.LoadingAdditive.SetFillTarget(1f);
        yield return null;
        
        // Kich hoat scene (luc nay moi chay Awake, Start cua cac object)
        asyncOperation.allowSceneActivation = true;
        
        // Cho scene kich hoat hoan toan
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        

        // Load vi tri va bound
        UIManageMent.Instance.LoadingAdditive.TurnOff();
        GameManageMent.Instance.PlayerManager.PlayerController.SetPosition(startPoint);
        
        currentSceneData = sceneData;
        UIManageMent.Instance.QuestUI.QuestViewInfo.UpdatePositionArrowQuest();

        GameObject mapBound = GameObject.Find(sceneData.NameBounder);
        if(mapBound != null && mapBound.GetComponent<PolygonCollider2D>() != null)
        {
            GameManageMent.Instance.SetBoundMap(mapBound.GetComponent<PolygonCollider2D>());
        }
        else
        {
            Debug.LogError("CANNOT SET BOUND MAP");
        }
        GameManageMent.Instance.EnviromentManager.SwitchEnvironment(sceneData.EnvironmentType);
        if(sceneData.EnvironmentType == EnvironmentType.Indoor)
        {
            GameManageMent.Instance.EnviromentManager.SetInfoForIndoorEnvironment(sceneData.LightIntense, sceneData.LightColor);
        }
    }

    IEnumerator UnLoadAsync(SceneData sceneData,Vector3 startPoint)
    {
        GameManageMent.Instance.PlayerManager.PlayerController.SetPosition(startPoint);
        GameObject mapBound = GameObject.Find(sceneData.ParentSceneData.NameBounder);
        if(mapBound != null && mapBound.GetComponent<PolygonCollider2D>() != null)
        {
            GameManageMent.Instance.SetBoundMap(mapBound.GetComponent<PolygonCollider2D>());
        }
        else
        {
            Debug.LogError("CANNOT SET BOUND MAP");
        }
        GameManageMent.Instance.EnviromentManager.SwitchEnvironment(sceneData.ParentSceneData.EnvironmentType);
        
        if(sceneData.ParentSceneData.EnvironmentType == EnvironmentType.Indoor)
        {
            GameManageMent.Instance.EnviromentManager.SetInfoForIndoorEnvironment(sceneData.ParentSceneData.LightIntense, sceneData.ParentSceneData.LightColor);
        }
        yield  return null;

        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneData.NameScene);

        while(!asyncOperation.isDone)
        {
            
            yield return null;
        }


        currentSceneData = sceneData.ParentSceneData;

        UIManageMent.Instance.QuestUI.QuestViewInfo.UpdatePositionArrowQuest();
        yield return new WaitForSeconds(1f);

        
        
        

        
        
        
    }
    
}
