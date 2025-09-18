using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class ObjectVisibilityManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> curObjectInCamera = new List<GameObject>();


    private Camera mainCamera;
    private float height;
    private float width;

    private float leftX;
    public float LeftX => leftX;
    private float rightX;
    public float RightX => rightX;
    private float topY;
    public float TopY => topY;
    private float bottmY;

    public float BottomY => bottmY;
    void Start()
    {
        mainCamera = Camera.main;
        height = 2f * mainCamera.orthographicSize;
        width = height * mainCamera.aspect;
        
    }


   void Update()
    {
        leftX = mainCamera.transform.position.x - width / 2.0f;
        rightX = mainCamera.transform.position.x + width / 2.0f;
        topY = mainCamera.transform.position.y + height / 2.0f;
        bottmY = mainCamera.transform.position.y - height / 2.0f;
        
       

    }
}
