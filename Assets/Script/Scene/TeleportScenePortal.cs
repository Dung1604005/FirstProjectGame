using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportScenePortal : MonoBehaviour
{
    [SerializeField] private SceneData   sceneData;


    [SerializeField] private Vector3 startPoint;

    [SerializeField] private bool isLocalTeleport;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (sceneData.TypeMap == TypeMap.SECONDARYMAP)
            {
                
                SceneLoader.Instance.LoadSceneAdditive(sceneData, startPoint);
            }
            else
            {
                if (isLocalTeleport)
                {
                    SceneLoader.Instance.BackToMainScene(startPoint);
                }
                else
                {
                    SceneLoader.Instance.LoadScene(sceneData, startPoint);
                }
                
            }
            
        }
        
    }
}
