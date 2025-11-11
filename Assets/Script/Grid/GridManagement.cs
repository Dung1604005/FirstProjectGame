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

    [SerializeField] private int chunkSize;
    public int ChunkSize => chunkSize;

    [SerializeField] private float timeUpdateGrid;

    private bool isUpdating;
    public bool IsUpdating => isUpdating;

    void Awake()
    {
        gridBuilder = GetComponent<GridBuilder>();
        distanceField = new DistanceField(this);
        flowField = new FlowField(this);
    }


    void Start()
    {
        gridBuilder.initGrid();
        StartCoroutine(UpdateGridField());
    }
    IEnumerator UpdateGridField()
    {
        while (true)
        {
            isUpdating = true;
            UpdateDistanceField(GameManageMent.Instance.PlayerManager.PlayerController.getPos());
            UpdateFlowField(GameManageMent.Instance.PlayerManager.PlayerController.getPos());

            yield return null;
            isUpdating = false;
            yield return new WaitForSeconds(timeUpdateGrid);
        }

    }
    public void UpdateDistanceField(Vector2 playerPosition)
    {
        distanceField.CalculateDistanceField(playerPosition);
    }
    public void UpdateFlowField(Vector2 playerPosition)
    {
        flowField.CalculateFlowField(playerPosition);
    }
}
