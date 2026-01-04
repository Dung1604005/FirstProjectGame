using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportScenePortal : MonoBehaviour
{
    [SerializeField] private SceneData   sceneData;


    [SerializeField] private Vector3 startPoint;

    [SerializeField] private bool isLocalTeleport;

    [SerializeField] private GameObject interactKey;

    [SerializeField] private bool isInteract;

   

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            interactKey.SetActive(true);
            isInteract = true;
        }
        
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            interactKey.SetActive(false);
            isInteract = false;
            
            
        }
    }
    public void OnTriggerStay2D(Collider2D collision){
        if(Input.GetKeyDown(KeyCode.E) && isInteract){

            isInteract = false;
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
    void Start(){
        interactKey.SetActive(false);
        isInteract = false;
    }
}
