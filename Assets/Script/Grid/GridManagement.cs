using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagement : MonoBehaviour
{
    private GridBuilder gridBuilder;
    public GridBuilder GridBuilder => gridBuilder;
    private DistanceField distanceField;
    public DistanceField DistanceField => distanceField;

    private FlowField flowField;
    public FlowField FlowField => flowField;

    void Awake()
    {
        gridBuilder = GetComponent<GridBuilder>();
        distanceField = new DistanceField(this);
        flowField = new FlowField(this);
    }


    void Start()
    {
        gridBuilder.initGrid();
        UpdateGridField();
    }
    public void UpdateGridField()
    {
        UpdateDistanceField(GameManageMent.Instance.PlayerManager.PlayerController.getPos());
        UpdateFlowField();
    }
    public void UpdateDistanceField(Vector2 playerPosition)
    {
        distanceField.CalculateDistanceField(playerPosition);
    }
    public void UpdateFlowField()
    {
        flowField.CalculateFlowField();
    }
}
