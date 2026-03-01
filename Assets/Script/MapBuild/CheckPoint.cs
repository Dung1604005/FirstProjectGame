using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject keyObject;

    [SerializeField] private float radInteract;

    private Transform player ;

    void Start()
    {
        if(GameObject.FindWithTag(GameConfig.PLAYER_TAG0) != null)
        {
            player = GameObject.FindWithTag(GameConfig.PLAYER_TAG0).transform;
        }
    }
    void Update()
    {
        if(player != null && (player.position - this.transform.position).sqrMagnitude <= radInteract * radInteract)
        {
            keyObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                SaveLoadManager.Instance.SaveGame();
            }
            
        }
        else
        {
            keyObject.SetActive(false);
        }
        
    }
}
