using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField ]private bool buildMode = false;
    public bool BuildMode => buildMode;

    private BuildPlacement buildPlacement;

    public BuildPlacement BuildPlacement=>buildPlacement;

    [SerializeField] private List<BuildableObject> buildableObjects= new List<BuildableObject>();
    public List<BuildableObject> BuildableObjects => buildableObjects;

    void Awake()
    {
        buildPlacement = GetComponent<BuildPlacement>();
    }

    public void TurnOnBuildMode(int index)
    {
        buildMode = true;
        
        buildPlacement.BuildModeOn(buildableObjects[index]);
    }
    public void TurnOffBuildMode()
    {
        buildMode = false;
        buildPlacement.BuildModeOff();
    }

    void Update()
    {
        if (buildMode)
        {
            buildPlacement.SetPos();
        }
    }
}
