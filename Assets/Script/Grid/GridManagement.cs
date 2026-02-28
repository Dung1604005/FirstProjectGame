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

    [SerializeField] private int chunkX;
    public int ChunkX => chunkX;

    [SerializeField] private int chunkY;
    public int ChunkY => chunkY;

    [SerializeField] private float timeUpdateGrid;

    private bool isUpdating;
    public bool IsUpdating => isUpdating;

    void Awake()
    {
        GameManageMent.Instance.SetGridManageMent(this.GetComponent<GridManagement>());
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
        bool canUpdate = true;
        if(GameObject.FindGameObjectWithTag(GameConfig.PLAYER_TAG0) == null)
        {
            canUpdate = false;
        }
        while (canUpdate)
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
