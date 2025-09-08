using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPlacement : MonoBehaviour
{
    [SerializeField] private GhostPreview ghostPrefab;

    public GhostPreview GhostPrefab => ghostPrefab;



    [SerializeField] private BuildableObject buildableObject;

    public void BuildModeOn(BuildableObject _buildableObject)
    {
        buildableObject = _buildableObject;
        ghostPrefab.gameObject.SetActive(true);
    }
    public void BuildModeOff()
    {
        ghostPrefab.gameObject.SetActive(false);
    }


    public void SetPos()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ghostPrefab.SetPos(pos.x, pos.y);
    }

    public bool CanPlace()
    {
        if (!ghostPrefab.CanPlace)
        {
            UIManageMent.Instance.UpdateWarning("CANT PLACE!!!");
            UIManageMent.Instance.TurnOnWarning();
        }
        return ghostPrefab.CanPlace;
    }

    public void PlaceObject()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var cloneObject = Instantiate(buildableObject, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
    }

    
}
