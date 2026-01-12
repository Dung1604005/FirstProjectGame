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
        // Nhuong 1 frame cho game de thuc hien UI Loading
        yield  return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneData.NameScene, loadSceneMode);
        
        while (!asyncOperation.isDone)
        {
            UIManageMent.Instance.LoadingAdditive.SetFillTarget(asyncOperation.progress);
            yield return null;
        }


        yield return new WaitForSeconds(1f);

        

        // Load vi tri va bound
        UIManageMent.Instance.LoadingAdditive.TurnOff();
        GameManageMent.Instance.PlayerManager.PlayerController.SetPosition(startPoint);
        
        currentSceneData = sceneData;

        GameObject mapBound = GameObject.Find(sceneData.NameBounder);
        if(mapBound != null && mapBound.GetComponent<PolygonCollider2D>() != null)
        {
            GameManageMent.Instance.SetBoundMap(mapBound.GetComponent<PolygonCollider2D>());
        }
        else
        {
            Debug.LogError("CANNOT SET BOUND MAP");
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
        
        yield  return null;

        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneData.NameScene);

        while(!asyncOperation.isDone)
        {
            
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        
        currentSceneData = sceneData.ParentSceneData;
        
        
    }
    
}
