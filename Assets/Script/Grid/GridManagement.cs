using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagement : MonoBehaviour
{
    private GridBuilder gridBuilder;
    public GridBuilder GridBuilder => gridBuilder;
    private DistanceField distanceField;
    public DistanceField DistanceField => distanceField;

    void Awake()
    {
        gridBuilder = GetComponent<GridBuilder>();
        distanceField = new DistanceField(this);
    }


    void Start()
    {
        gridBuilder.initGrid();
    }
    public void UpdateDistanceField(Vector2 playerPosition)
    {
        distanceField.CalculateDistanceField(playerPosition);
    }
}
