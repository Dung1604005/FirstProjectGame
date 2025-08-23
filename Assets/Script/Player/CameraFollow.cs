using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private PlayerController player;
    [SerializeField] private float moveSpeed;
    void Start()
    {
        
    }
    void cameraFollow()
    {
        Vector3 playerPos = new Vector3(player.getPos().x, player.getPos().y, transform.position.z);
        
        transform.position = Vector3.Lerp(transform.position, playerPos, moveSpeed*Time.deltaTime)   ;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        cameraFollow();
    }
}
