using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagement : MonoBehaviour
{
    [SerializeField] private GridBuilder gridBuilder;
    public GridBuilder GridBuilder => gridBuilder;

    void Awake()
    {
        gridBuilder = GetComponent<GridBuilder>();

    }
    void Start()
    {
        gridBuilder.initGrid();
    }
}
